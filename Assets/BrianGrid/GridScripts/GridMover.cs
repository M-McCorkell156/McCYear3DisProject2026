using UnityEngine;
using System.Collections.Generic;
public class GridMover : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Grid Reference")]
    public GridManager gridManager;

    private Vector2Int currentGridPos;
    private Vector3 targetWorldPos;
    private bool isMoving = false;

    private List<Vector2> interactbleTiles;

    [Header("Animator")]
    [SerializeField] private Animator animator;
    [Header("Matt's amazing stuff")]
    [SerializeField] private bool isFreeze;
    [SerializeField] private GameObject puzzleIcon;
    [SerializeField] private GameObject searchableIcon;
    [SerializeField] private GameObject weaponIcon;
    [SerializeField] private GameObject powerIcon;

    [SerializeField] private GameStateManager ThisCharactersGSM;

    [SerializeField] private GameObject MeshObj;

    private bool canExit;

    public void FreezeGridMoves()
    {
        isFreeze = true;
        //Debug.Log("freeze");
    }

    public void UnfreezeGridMoves()
    {
        isFreeze = false;
        //Debug.Log("unfreeze");
    }
    private void Start()
    {
        puzzleIcon.SetActive(false);
        searchableIcon.SetActive(false);
        weaponIcon.SetActive(false);
        powerIcon.SetActive(false);

        canExit = false;

        currentGridPos = gridManager.GetClosestGridPosition(transform.position);
        targetWorldPos = gridManager.GetWorldPosition(currentGridPos.x, currentGridPos.y);
        transform.position = targetWorldPos;

        gridManager.SetPlayerPosition(currentGridPos);
        
        LockpickingMiniGame.freezeGridMove += FreezeGridMoves;
        LockpickingMiniGame.unfreezeGridMoves += UnfreezeGridMoves;  
        GridManager.freeze += FreezeGridMoves;
        GridManager.unfreeze += UnfreezeGridMoves;
        WinConditionUI.CanLeave += SetEscape;
    }

    private void Update()
    {
        if (isFreeze) 
        {
            //Debug.Log("frozen");
            return;
        }

        if (isMoving)
        {
            //Debug.Log("moving");
            transform.position = Vector3.MoveTowards(transform.position, targetWorldPos, moveSpeed * Time.deltaTime);
            Vector3 direction = (targetWorldPos - transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                //Debug.Log("target dir is "+ targetRotation);
                MeshObj.transform.rotation = Quaternion.RotateTowards(MeshObj.transform.rotation, targetRotation, 360 * Time.deltaTime);
            }

            if (Vector3.Distance(transform.position, targetWorldPos) < 0.01f)
            {
                transform.position = targetWorldPos;
                isMoving = false;
                animator.SetBool("isWalk",false);
                gridManager.OnPlayerArrivedAtTile(currentGridPos);
            }
        }
    }

    public void MoveToTile(int x, int y)
    {
        if (isMoving) return;
        if (!gridManager.IsValidTile(x, y)) return;

        targetWorldPos = gridManager.GetWorldPosition(x, y);
        currentGridPos = new Vector2Int(x, y);
        isMoving = true;
        animator.SetBool("isWalk",true);
        gridManager.SetPlayerPosition(currentGridPos);
        CheckForInteractive(currentGridPos);
    }

    public Vector2Int GetCurrentGridPos() => currentGridPos;

    private void CheckForInteractive(Vector2Int currentGridPos)
    {
        foreach (Vector2 interactiveGridPos in gridManager.Puzzletiles)
        {
            if (currentGridPos == interactiveGridPos)
            {
                puzzleIcon.SetActive(true);
            }
            else
            {
                puzzleIcon.SetActive(false);
            }
        }

        foreach (Vector2 interactiveGridPos in gridManager.SearchableTilesList)
        {
            if (currentGridPos == interactiveGridPos)
            {
                searchableIcon.SetActive(true);
            }
            else
            {
                searchableIcon.SetActive(false);
            }
        }

        foreach (Vector2 interactiveGridPos in gridManager.WeaponTile)
        {
            if (currentGridPos == interactiveGridPos)
            {
                weaponIcon.SetActive(true);
            }
            else
            {
                weaponIcon.SetActive(false);
            }
        }

        bool isActive = false; 

        foreach (Vector2 interactiveGridPos in gridManager.Powertiles)
        {
            if (currentGridPos == interactiveGridPos && !isActive)
            {
                isActive = true;
                powerIcon.SetActive(true);
            }
            else if (!isActive)
            {
                powerIcon.SetActive(false);
            }
        }
        foreach (Vector2 interactiveGridPos in gridManager.EscapeTiles)
        {
            if (currentGridPos == interactiveGridPos && canExit)
            {
                Debug.Log("on Escape and can");
                gridManager.ReplaceInteractibleTile();
                ThisCharactersGSM.CharacterEscape();            
            }
        }
    }
    private void SetEscape()
    {
        Debug.Log("escape allowed");
        canExit = true;
    }
}
