using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class GridManager : MonoBehaviour
{
    [Header("Grid Settings")]
    public int width = 10;
    public int height = 10;
    public float cellSize = 1f;

    [Header("Tile Prefabs")]
    public GameObject normalTilePrefab;
    public GameObject mudTilePrefab;
    public GameObject wallTilePrefab;
    [HideInInspector] public TileSelector[,] tiles;

    [Header("References")]
    [SerializeField] private GridMover currentGridMover;
    private Characters currentFrozenCharacter;

    [SerializeField] private GameObject currentCharacterObject;
    private GameObject poweredCurrentCharacterObj;

    [SerializeField] private ChangeSelectedCharacter changeSelectedCharacter;

    [Header("Matts Amazing Tile Stuff")]
    [Range(2, 6)]
    [SerializeField] private int PlayerTileRange;
    [Range(1, 3)]
    [SerializeField] private int playerTurnLimit;
    [SerializeField] private int thisPlayerTurnCount;
    [SerializeField] private int storedPlayer1TurnCount;
    [SerializeField] private int storedPlayer2TurnCount;
    private int totalTurnCount;


    [SerializeField] private bool isTimeOn;
    [SerializeField] private bool isMoving;
    [SerializeField] private ActionManager actionManager;

    [SerializeField] private TextMeshProUGUI plyAshTurnCount;
    [SerializeField] private TextMeshProUGUI plyJoeTurnCount;

    [SerializeField] public List<Vector2> SearchableTilesList = new List<Vector2>();
    [SerializeField] public List<Vector2> WeaponTile = new List<Vector2>();
    [SerializeField] public List<Vector2> Puzzletiles = new List<Vector2>();
    [SerializeField] public List<Vector2> Powertiles = new List<Vector2>();
    [SerializeField] public List<Vector2> EscapeTiles = new List<Vector2>();
    public List<Vector2> Walltiles = new List<Vector2>();

    [SerializeField] private List<Vector2> BlockedTiles1;
    [SerializeField] private List<Vector2> BlockedTiles2;

    public static event FreezeTileMove freeze;

    public static event FreezeTileMove unfreeze;
    private void Awake()
    {
        GenerateGrid();
        storedPlayer1TurnCount = 0;
        storedPlayer2TurnCount = 0;
        UpdateUI();
        LockpickingMiniGame.freezeGridMove += ClearHighlights;
        LockpickingMiniGame.unfreezeGridMoves += ClearHighlights;
        TurnManager.freezeMoves += ClearHighlights;
        TurnManager.unfreezeMoves += ClearHighlights;
        GridManager.freeze += ClearHighlights;
        GridManager.unfreeze += ClearHighlights;
        currentFrozenCharacter = Characters.None;
        SetCurrentCharacter();
    }

    public void AddWallTile(string tileName)
    {
        GameObject tileObj = GameObject.Find(tileName);
        if (tileObj != null)
        {
            Vector3 pos = tileObj.transform.position;
            Vector2Int gridPos = GetClosestGridPosition(pos);
            Walltiles.Add(new Vector2(gridPos.x, gridPos.y));
        }
    }

    public void ReplaceWallTile()
    {
        foreach (Vector2 wallGridPos in Walltiles)
        {
            GameObject wallTileObj = tiles[(int)wallGridPos.x, (int)wallGridPos.y].gameObject;
            Destroy(wallTileObj.GetComponent<WallColide>());
            TileSelector selector = wallTileObj.GetComponent<TileSelector>();
            if (selector != null)
            {
                selector.moveCost = 5;
                selector.isWalkable = false;
                selector.Highlight(Color.red);
            }
        }
    }

    public void RefreshWallTiles()
    {
        foreach (Vector2 wallGridPos in Walltiles)
        {
            GameObject wallTileObj = tiles[(int)wallGridPos.x, (int)wallGridPos.y].gameObject;
            TileSelector selector = wallTileObj.GetComponent<TileSelector>();
            if (selector != null)
            {
                wallTileObj.AddComponent<WallColide>();
                selector.ResetColor();
                selector.moveCost = 5;
                selector.isWalkable = true;
            }
        }
    }

    private void GenerateGrid()
    {
        tiles = new TileSelector[width, height];
        Vector3 startPos = transform.position;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3 worldPos = startPos + new Vector3(x * cellSize, 0, y * cellSize);
                GameObject tileObj = Instantiate(normalTilePrefab, worldPos, Quaternion.identity, transform);
                tileObj.name = $"Tile_{x}_{y}";
                tileObj.AddComponent<BoxCollider>().isTrigger = true;
                tileObj.AddComponent<WallColide>();

                TileSelector selector = tileObj.GetComponent<TileSelector>();
                if (selector == null)
                    selector = tileObj.AddComponent<TileSelector>();

                selector.Init(this, x, y, true, 1);
                tiles[x, y] = selector;

                foreach (Vector2 intGridPos in Puzzletiles)
                {
                    if (intGridPos.x == x && intGridPos.y == y)
                    {
                        tileObj.AddComponent<InteractiveTile>().tileType = TileType.Puzzle;
                        //tileObj.AddComponent<Outline>();
                    }
                }

                foreach (Vector2 intGridPos in SearchableTilesList)
                {
                    if (intGridPos.x == x && intGridPos.y == y)
                    {
                        tileObj.AddComponent<InteractiveTile>().tileType = TileType.Searchable;
                        //tileObj.AddComponent<Outline>();
                    }
                }

                foreach (Vector2 intGridPos in WeaponTile)
                {
                    if (intGridPos.x == x && intGridPos.y == y)
                    {
                        tileObj.AddComponent<InteractiveTile>().tileType = TileType.Weapon;
                        //tileObj.AddComponent<Outline>();
                    }
                }

                foreach (Vector2 intGridPos in Powertiles)
                {
                    if (intGridPos.x == x && intGridPos.y == y)
                    {
                        tileObj.AddComponent<InteractiveTile>().tileType = TileType.Power;
                        //tileObj.AddComponent<Outline>();
                    }
                }

                foreach (Vector2 intGridPos in EscapeTiles)
                {
                    if (intGridPos.x == x && intGridPos.y == y)
                    {
                        tileObj.AddComponent<InteractiveTile>().tileType = TileType.Escape;
                        //tileObj.AddComponent<Outline>();
                    }
                }


            }
        }
    }

    public Vector3 GetWorldPosition(int x, int y)
    {
        return transform.position + new Vector3(x * cellSize, 0, y * cellSize);
    }

    public Vector2Int GetClosestGridPosition(Vector3 worldPosition)
    {
        int x = Mathf.RoundToInt((worldPosition.x - transform.position.x) / cellSize);
        int y = Mathf.RoundToInt((worldPosition.z - transform.position.z) / cellSize);
        return new Vector2Int(Mathf.Clamp(x, 0, width - 1), Mathf.Clamp(y, 0, height - 1));
    }

    public bool IsValidTile(int x, int y)
    {
        return x >= 0 && y >= 0 && x < width && y < height && tiles[x, y] != null && tiles[x, y].isWalkable;
    }

    public void ResetTurnCount()
    {
        storedPlayer1TurnCount = 0;
        storedPlayer2TurnCount = 0;
        thisPlayerTurnCount = 0;
        UpdateUI();
    }

    private void UpdateUI()
    {
        switch (changeSelectedCharacter.GetCharacter())
        {
            case Characters.Ashley:
                plyAshTurnCount.text = playerTurnLimit - thisPlayerTurnCount + "/" + playerTurnLimit;
                plyJoeTurnCount.text = playerTurnLimit - storedPlayer2TurnCount + "/" + playerTurnLimit;
                break;

            case Characters.Joe:
                plyAshTurnCount.text = playerTurnLimit - storedPlayer1TurnCount + "/" + playerTurnLimit;
                plyJoeTurnCount.text = playerTurnLimit - thisPlayerTurnCount + "/" + playerTurnLimit;
                break;
        }
    }
    public int GetPlayerTurnLimit()
    {
        if(changeSelectedCharacter.CanSwitch())
            return playerTurnLimit * 2;
        else
            return playerTurnLimit;
    }

    public void OnTileClicked(int x, int y)
    {
        //Debug.Log($"Tile clicked at ({x}, {y})");

        if (currentGridMover == null)
            return;

        if (!IsValidTile(x, y))
            return;

        if (isMoving)
            return;

        Vector2Int center = currentGridMover.GetCurrentGridPos();

        for (int v = 0; v < width; v++)
        {
            for (int w = 0; w < height; w++)
            {
                int distance = Mathf.Abs(v - center.x) + Mathf.Abs(y - center.y);

                if (distance + tiles[v, w].moveCost <= PlayerTileRange && v == x && y == w && thisPlayerTurnCount < playerTurnLimit)
                {
                    //Debug.Log("Click unlocked");
                    isMoving = true;
                    thisPlayerTurnCount++;
                    UpdateUI();

                    if (!isTimeOn)
                    {
                        actionManager.TurnClock();
                    }

                    currentGridMover.MoveToTile(x, y);
                    Invoke("UnlockClick", 0.2f);
                    return;
                }
            }
        }
    }
    public void ClearBlockedTiles1()
    {
        foreach (Vector2 blockedPos in BlockedTiles1)
        {
                tiles[(int)blockedPos.x,(int)blockedPos.y].isWalkable = true;
                tiles[(int)blockedPos.x, (int)blockedPos.y].ResetColor();          
        }
    }

    public void ClearBlockedTiles2()
    {
        foreach (Vector2 blockedPos in BlockedTiles2)
        {
            tiles[(int)blockedPos.x, (int)blockedPos.y].isWalkable = true;
            tiles[(int)blockedPos.x, (int)blockedPos.y].ResetColor();
        }
    }


    private void UnlockClick()
{
    //Debug.Log("Click unlocked");
    isMoving = false;
}
public void HighlightRange(Vector2Int center, int range)
{
    ClearHighlights();
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            int distance = Mathf.Abs(x - center.x) + Mathf.Abs(y - center.y);
            //Debug.Log("Distance: " + distance  + " Center: " + center + " Tile: (" + x + "," + y + ")");
            if (distance + tiles[x, y].moveCost <= range && thisPlayerTurnCount < playerTurnLimit)
            {
                if (tiles[x, y] != null)
                {
                    if (tiles[x, y].isWalkable && tiles[x, y].GetComponent<InteractiveTile>() == null)
                    {
                        tiles[x, y].Highlight(Color.cyan);
                    }
                    else if (tiles[x, y].GetComponent<InteractiveTile>() != null)
                    {
                        tiles[x, y].Highlight(Color.magenta);
                    }
                    else
                        tiles[x, y].Highlight(Color.red);
                }
            }
        }
    }
}

public void ClearHighlights()
{
    for (int x = 0; x < width; x++)
    {
        for (int y = 0; y < height; y++)
        {
            if (tiles[x, y] == null)
                continue;
            if (tiles[x, y] != null && tiles[x, y].GetComponent<InteractiveTile>() == null)
                tiles[x, y].ResetColor();

            tiles[x, y].moveCost = 1;
        }
    }
}

public void OnPlayerArrivedAtTile(Vector2Int pos)
{
    HighlightRange(pos, PlayerTileRange); // Example: highlight around player
}

public void SetPlayerPosition(Vector2Int pos)
{
    HighlightRange(pos, PlayerTileRange);
}

public void ResetHighlights()
{
    ClearHighlights();
    HighlightRange(currentGridMover.GetCurrentGridPos(), PlayerTileRange);
}

public void SetCurrentCharacter()
{
    currentCharacterObject = changeSelectedCharacter.GetCurrentCharacterObject();
    currentGridMover = currentCharacterObject.GetComponent<GridMover>();

    switch (changeSelectedCharacter.GetCharacter())
    {
        case Characters.Ashley:
            storedPlayer2TurnCount = thisPlayerTurnCount;
            thisPlayerTurnCount = 0 + storedPlayer1TurnCount;
            break;
        case Characters.Joe:
            storedPlayer1TurnCount = thisPlayerTurnCount;
            thisPlayerTurnCount = 0 + storedPlayer2TurnCount;
            break;
    }

    if (changeSelectedCharacter.GetCharacter() == currentFrozenCharacter)
    {
        freeze();
        //Debug.Log("current char freeze");

    }
    else
    {
        unfreeze();
        //Debug.Log("current char unfreeze");
    }
}

public void SetPoweredCharacter()
{
    poweredCurrentCharacterObj = currentCharacterObject;
}

public GameStateManager GetPoweredGameStateManager()
{
    SetCurrentCharacter();
    if (poweredCurrentCharacterObj.gameObject.GetComponentInChildren<GameStateManager>() != null)
    {
        return poweredCurrentCharacterObj.gameObject.GetComponentInChildren<GameStateManager>();
    }
    else
    {
        return null;
    }
}
public void FreezeCurrentGridMover()
{
    //Debug.Log("freeze");
    Characters currentCharacter;
    currentCharacter = changeSelectedCharacter.GetCharacter();
    switch (currentCharacter)
    {
        case Characters.Ashley:
            currentFrozenCharacter = Characters.Ashley;
            break;
        case Characters.Joe:
            currentFrozenCharacter = Characters.Joe;
            break;
    }

    currentGridMover.FreezeGridMoves();

    if (freeze != null)
        freeze();

}

public void UnfreezeCurrentGridMover()
{
    //Debug.Log("unfreeze");
    currentFrozenCharacter = Characters.None;
    Characters currentCharacter;
    currentCharacter = changeSelectedCharacter.GetCharacter();

    switch (currentCharacter)
    {
        case Characters.Ashley:
            if (currentGridMover)
                currentGridMover.UnfreezeGridMoves();

            if (unfreeze != null) unfreeze();
            changeSelectedCharacter.SelectJoe();
            SetCurrentCharacter();
            currentGridMover.UnfreezeGridMoves();
            if (unfreeze != null) unfreeze();
            changeSelectedCharacter.SelectAshley();
            SetCurrentCharacter();
            break;
        case Characters.Joe:
            if (currentGridMover)
                currentGridMover.UnfreezeGridMoves();
            if (unfreeze != null) unfreeze();
            changeSelectedCharacter.SelectAshley();
            SetCurrentCharacter();
            currentGridMover.UnfreezeGridMoves();
            if (unfreeze != null) unfreeze();
            changeSelectedCharacter.SelectJoe();
            SetCurrentCharacter();
            break;
    }

}

public void ReplaceInteractibleTile()
{
    //Debug.Log("replace");
    Vector2Int currentGridPos = currentGridMover.GetCurrentGridPos();
    GameObject currentTileObj = tiles[currentGridPos.x, currentGridPos.y].gameObject;
    //Debug.Log("Destroying " + currentTileObj);
    Destroy(currentTileObj.GetComponent<InteractiveTile>());
    ResetHighlights();
}


}
