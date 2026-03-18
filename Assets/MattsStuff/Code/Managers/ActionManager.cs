using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    [SerializeField] private EnemyMover enemyMover;

    [SerializeField] private GameObject handToTurn;

    [SerializeField] private Animator animitor;

    private float turnCount;
    public void TurnClock()
    {
        turnCount++;
        Quaternion newAngle = Quaternion.Euler((turnCount / gridManager.GetPlayerTurnLimit()) * 360f, Quaternion.identity.y, Quaternion.identity.z);

        handToTurn.transform.localRotation = Quaternion.Lerp(Quaternion.identity, newAngle, 1f);

        if (turnCount >= gridManager.GetPlayerTurnLimit())
        {
            gridManager.ClearHighlights();
            
            Invoke("EnemyPhase", 0.5f); 
        }
    }
    public void EnemyPhase()
    {
        enemyMover.TakeTurn();
    }
    public void EndOfActions()
    {
        gridManager.ResetTurnCount();
        ResetTurnClock();
        gridManager.ResetHighlights();
    }

    public void ResetTurnClock()
    {
        turnCount = 0;
        handToTurn.transform.localRotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, 0);
    }
}
