using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum TimeEventType
{
    MiniGameStart,
    MiniGameEnd,
    MiniGameCancel
}
public class TimeManager : MonoBehaviour
{
    private int gameCount;
    private int gamesFinished;

    private float turnTime;

    [SerializeField] private List<float> miniGameStart;
    [SerializeField] private List<float> miniGameEnd;

    private void Update()
    {
        turnTime += Time.deltaTime;
    }

    public void RecordTime(TimeEventType currentEvent)
    {
        switch (currentEvent)
        {
            case TimeEventType.MiniGameStart:

                miniGameStart.Add(turnTime);
                break;

            case TimeEventType.MiniGameEnd:

                miniGameEnd.Add(turnTime);
                gameCount++;
                gamesFinished++;
                break;

            case TimeEventType.MiniGameCancel:
                Debug.Log("cancle game : " + gameCount);
                if (miniGameStart.Count > 0)
                {
                    miniGameStart.RemoveAt(gameCount);
                }
                gameCount++;
                break;

        }
    }

    public float CalculateTurnScore()
    {
        float turnScore = 0;

        for (int i = 0; i < miniGameStart.Count; i++)
        {
            if (miniGameStart != null || miniGameEnd != null)
            {
                turnScore += miniGameEnd[i] - miniGameStart[i];
            }
            else
            {
                turnScore += 0;
            }

        }

        miniGameStart.Clear();
        miniGameEnd.Clear();
        turnScore += gamesFinished;
        gamesFinished = 0;

        return turnScore;

    }
}
