using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using Unity.Mathematics;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{

    [SerializeField] private Animator animitor;
    public static TurnManager Instance;
    [SerializeField] private LockpickingMiniGame lockpickingMiniGame;
    [SerializeField] private List<SearchingScript> searchingScripts;
    [SerializeField] private PowerScript powerScript;
    [SerializeField] private EnemyMover enemyMover;
    [SerializeField] private TimeManager timeManager;

    public bool IsPlayerTurn = true;

    [Range(10, 60)] public float turnTimeLimit;

    [SerializeField] private GameObject handToTurn;

    private float timeCountdown;

    public static event FreezeTileMove freezeMoves;

    public static event FreezeTileMove unfreezeMoves;

    [SerializeField] private GridManager gridManager;

    private void Awake()
    {
        Instance = this;
        StartPlayerTurn();
    }

    public void EndTurn()
    {

        //Debug.Log("Force ending turn.");
        handToTurn.transform.localRotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, 0);
        IsPlayerTurn = false;
        lockpickingMiniGame.FinishGame();
        foreach (SearchingScript searchingScript in searchingScripts)
        {
            searchingScript.CancleSearch();
        }

        powerScript.CanclePower();
        gridManager.ResetTurnCount();

        // Call enemy actions here
        EnemyPhase();
    }

    private void EnemyPhase()
    {
        animitor.SetTrigger("Chime");

        if (freezeMoves != null)
        {
            freezeMoves();
        }

        //Debug.Log(timeManager.CalculateTurnScore());
        //Debug.Log("Enemy turn started!");
        // When enemies finish:
        //Debug.Log("Enemy turn ended!");
        //enemyMover.TakeTurn();
        Invoke("StartPlayerTurn", 5.0f); // Simulate enemy turn delay
    }

    public void StartPlayerTurn()
    {
        //Debug.Log("Player turn started!");
        if (unfreezeMoves != null)
        {
            unfreezeMoves();
        }

        timeCountdown = turnTimeLimit;
        handToTurn.transform.localRotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, 0);
        StartCoroutine(StartCountdown());
        IsPlayerTurn = true;
    }

    public IEnumerator StartCountdown()
    {
        Quaternion newAngle;
        while (timeCountdown > 0)
        {
            //Debug.Log("Countdown: " + timeCountdown);         
            newAngle = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, (timeCountdown / turnTimeLimit) * -360f);
            handToTurn.transform.localRotation = Quaternion.Lerp(Quaternion.identity, newAngle, 1f);
            animitor.SetTrigger("Tick");
            yield return new WaitForSeconds(1.0f);
            timeCountdown--;
        }
        //Debug.Log("Turn time over!");
        EndTurn();
    }

}
