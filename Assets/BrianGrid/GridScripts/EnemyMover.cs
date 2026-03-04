using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public float moveSpeed = 4f;

    private GridManager grid;
    private AStarPathfinding pathfinder;
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private Animator animator;

    private Vector2Int currentGridPos;
    private Vector2Int moveToTile;
    [SerializeField] private List<Vector2Int> currentPath;
    private int pathIndex = 0;
    private bool isMoving = false;
    private bool isTurning = false;
    [SerializeField] private int moveRange;
    private int moveCount = 0;

    [SerializeField] private List<GameObject> players;
    private GameObject targetPlayer;

    [SerializeField] private GameObject MeshObj;

    private bool IsSet;

    [SerializeField] private Camera mainCamera;
    private GameObject lastCameraCharacter;

    private void Start()
    {
        grid = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<AStarPathfinding>();
        IsSet = false;
    }

    // ---------------------------------------------------------
    // Called by TurnManager during ENEMY TURN
    // ---------------------------------------------------------
    public void TakeTurn()
    {
        IsSet = true;
        currentGridPos = grid.GetClosestGridPosition(transform.position);

        //Debug.Log("Enemy taking turn. Current position: " + currentGridPos);
        moveCount = 0;

        //Debug.Log("Enemy targeting player 1");
        moveToTile = new Vector2Int();

        targetPlayer = players[0];

        moveToTile = grid.GetClosestGridPosition(targetPlayer.gameObject.transform.position);

        //else
        //{
        //    Debug.Log("Enemy targeting player 2");
        //    Vector2Int moveToTile = new Vector2Int();
        //    moveToTile = grid.GetClosestGridPosition(players[1].gameObject.transform.position);

        //    MoveTowardsPlayer(moveToTile);

        lastCameraCharacter = mainCamera.transform.parent.gameObject;
        mainCamera.transform.SetParent(transform);

        StartCoroutine(MoveCameraToCharacter(this.gameObject));

        Invoke(nameof(MoveTowardsPlayer), 1f);
        //mainCamera.transform.localPosition = new Vector3( 25f, 45f, currentCharacter.transform.position.z); 
    }

    private IEnumerator MoveCameraToCharacter(GameObject targetObj)
    {
        //Debug.Log("Moving camera to enemy at " + currentGridPos);
        Vector3 targetPos = new Vector3(25f, 45f, targetObj.transform.position.z);
        while (mainCamera.transform.localPosition != targetPos)
        {
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, targetPos, Time.deltaTime);
            yield return null;
        }
    }

    // ---------------------------------------------------------
    // Pathfind toward the player
    // ---------------------------------------------------------
    public void MoveTowardsPlayer()
    {
        //Debug.Log("Enemy at " + currentGridPos + " moving towards player at " + moveToTile);
        animator.SetBool("IsWalk", true);
        currentPath = pathfinder.FindPath(currentGridPos, moveToTile);
        if (currentPath != null && currentPath.Count > 0)
        {
            pathIndex = 0;
            isTurning = true;
            isMoving = true;
        }
    }

    // ---------------------------------------------------------
    // Move along the calculated path
    // ---------------------------------------------------------
    private void Update()
    {
        if (!IsSet)
            return;

        Vector3 targetPos;
        if (isMoving)
        {
            targetPos = grid.GetWorldPosition(currentPath[pathIndex].x, currentPath[pathIndex].y);
        }
        else
        {
            targetPos = targetPlayer.transform.position;
        }

        Vector3 direction = (targetPos - transform.position).normalized;

        if (direction != Vector3.zero)
        {
            //Debug.Log("Enemy is turning towards next tile: " + direction);
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            //Debug.Log("target dir is "+ targetRotation);
            MeshObj.transform.rotation = Quaternion.RotateTowards(MeshObj.transform.rotation, targetRotation, 360 * Time.deltaTime);
        }

        if (!isMoving || currentPath == null)
            return;

        if (currentPath.Count == 1)
        {
            //Debug.Log("Enemy already on player tile, attacking!");
            AttackPlayer();

            return;
        }

        transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, targetPos) < 0.05f)
        {
            //Debug.Log("Reached tile: " + currentPath[pathIndex]);
            currentGridPos = currentPath[pathIndex];
            pathIndex++;
            moveCount++;

            if (moveCount >= moveRange)
            {
                //Debug.Log("Enemy reached move limit for this turn.");
                EndTurn();
                Invoke(nameof(CallEndofActions), 1f);
            }

            if (pathIndex + 1 == currentPath.Count)
            {
                //Debug.Log("Enemy reached player and attacks!");
                AttackPlayer();
                isMoving = false;   
            }
        }
    }
    private void EndTurn()
    {
        mainCamera.transform.SetParent(lastCameraCharacter.transform);

        StartCoroutine(MoveCameraToCharacter(lastCameraCharacter));

        isMoving = false;
        animator.SetBool("IsWalk", false);
    }

    public void AttackPlayer()
    {
        //Debug.Log("Enemy attacks player!");        
        animator.SetBool("IsAttack", true);
        targetPlayer.GetComponent<GridMoverHealth>().TakeDamage();

        EndTurn();
        animator.SetBool("IsAttack", false);

        Invoke(nameof(CallEndofActions), 1f);
    }
    private void CallEndofActions()
    {
        actionManager.EndOfActions();
    }
}
