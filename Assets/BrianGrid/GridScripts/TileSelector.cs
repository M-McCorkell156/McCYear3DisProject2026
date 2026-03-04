using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Unity.VisualScripting;

[RequireComponent(typeof(Renderer))]
public class TileSelector : MonoBehaviour
{
    private Renderer rend;
    private Color baseColor;
    private GridManager gridManager;

    [SerializeField] public int x, y;
    [SerializeField] public bool isWalkable = true;
    [SerializeField] public int moveCost = 1;

    private bool isFreeze;

    private void Awake()
    {
        rend = GetComponent<Renderer>();
        baseColor = rend.material.color;
        isFreeze = false;
        LockpickingMiniGame.freezeGridMove += FreezeSelection;
        LockpickingMiniGame.unfreezeGridMoves += UnfreezeSelection;
        TurnManager.freezeMoves += FreezeSelection;
        TurnManager.unfreezeMoves += UnfreezeSelection;
        GridManager.freeze += FreezeSelection;
        GridManager.unfreeze += UnfreezeSelection;
    }

    private void FreezeSelection()
    {
        isFreeze = true;
    }
    private void UnfreezeSelection()
    {
        isFreeze = false;
    }

    public void Init(GridManager manager, int gridX, int gridY, bool walkable, int cost)
    {
        gridManager = manager;
        x = gridX;
        y = gridY;
        isWalkable = walkable;
        moveCost = cost;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            //Debug.Log("Clicked on UI, ignoring tile click.");
            return;
        }
        
        

        if (gridManager == null || isFreeze )
        {
            //Debug.Log("Tile selection is frozen or GridManager is null.");
            return;
        }
        
        //Debug.Log($"Tile at ({x}, {y}) clicked.");
        gridManager.OnTileClicked(x, y);
    }

    public void Highlight(Color color)
    {
        if (rend != null)
            rend.material.color = color;
    }

    public void ResetColor()
    {
        if (rend != null)
            rend.material.color = baseColor;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("wall"))
        {
            this.gameObject.IsDestroyed();
        }
    }
}
