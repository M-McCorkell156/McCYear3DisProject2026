using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMover : MonoBehaviour
{
    public float moveSpeed = 4f;

    private GridManager grid;
    private AStarPathfinding pathfinder;
    [SerializeField] private ActionManager actionManager;
    [SerializeField] private ChangeSelectedCharacter changeSelectedCharacter;
    [SerializeField] private WinConditionUI winConditionUI;
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

    [SerializeField] private GameObject Killer;

    private void Start()
    {
        grid = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<AStarPathfinding>();
        IsSet = false;

        if (winConditionUI.IsTutorial())
            Killer.SetActive(false);
    }

    // ---------------------------------------------------------
    // Called by TurnManager during ENEMY TURN
    // ---------------------------------------------------------
    public void TakeTurn()
    {
        changeSelectedCharacter.FreezeSwitching();
        if (!Killer.activeSelf)
        {
            Killer.SetActive(true);
        }

        IsSet = true;
        currentGridPos = grid.GetClosestGridPosition(transform.position);

        //Debug.Log("Enemy taking turn. Current position: " + currentGridPos);
        moveCount = 0;

        //Debug.Log("Enemy targeting player 1");
        moveToTile = new Vector2Int();

        if (Mathf.Abs(Random.Range(1, 10) / 2) == 1 && changeSelectedCharacter.CanSwitch())
        {
            targetPlayer = players[1];
        }
        else
        {
            targetPlayer = players[0];
        }

        //Debug.Log(targetPlayer);
        moveToTile = grid.GetClosestGridPosition(targetPlayer.gameObject.transform.position);


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
    public void Update()
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
    public void EndTurn()
    {
        mainCamera.transform.SetParent(lastCameraCharacter.transform);

        StartCoroutine(MoveCameraToCharacter(lastCameraCharacter));

        isMoving = false;
        animator.SetBool("IsWalk", false);
    }

    public void AttackPlayer()
    {
        Debug.Log("Enemy attacks player!");
        animator.SetBool("IsAttack", true);
        targetPlayer.GetComponent<GridMoverHealth>().TakeDamage();

        EndTurn();
        Invoke(nameof(StopAttack), 0.5f);

        Invoke(nameof(CallEndofActions), 1f);
    }
    private void StopAttack()
    {
        animator.SetBool("IsAttack", false);
    }

    public void CallEndofActions()
    {
        grid.UnfreezeCurrentGridMover();
        changeSelectedCharacter.UnFreezeSwitch();
        actionManager.EndOfActions();
    }
}
