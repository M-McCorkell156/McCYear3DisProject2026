using UnityEngine;

public class ActionManager : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;

    [SerializeField] private GameObject handToTurn;

    [SerializeField] private Animator animitor;

    private float turnCount;
    public void TurnClock()
    {
        turnCount++;
        Quaternion newAngle = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, (turnCount / gridManager.GetPlayerTurnLimit()) * 360f);

        handToTurn.transform.localRotation = Quaternion.Lerp(Quaternion.identity, newAngle, 1f);

        if (turnCount >= gridManager.GetPlayerTurnLimit())
        {
            gridManager.ResetTurnCount();
            ResetTurnClock();
        }
    }

    public void ResetTurnClock()
    {
        turnCount = 0;
        handToTurn.transform.localRotation = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, 0);
    }   


}
