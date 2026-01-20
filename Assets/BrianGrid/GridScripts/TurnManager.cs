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

    public bool IsPlayerTurn = true;

    [SerializeField][Range(10, 60)] private float turnTimeLimit;

    [SerializeField] private GameObject handToTurn;

    private float timeCountdown;

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

        // Call enemy actions here
        EnemyPhase();
    }

    private void EnemyPhase()
    {
        animitor.SetTrigger("Chime");
        //Debug.Log("Enemy turn started!");
        // When enemies finish:
        //Debug.Log("Enemy turn ended!");
        //enemyMover.TakeTurn();
        Invoke("StartPlayerTurn", 10.0f); // Simulate enemy turn delay
    }

    public void StartPlayerTurn()
    {
        //Debug.Log("Player turn started!");
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
