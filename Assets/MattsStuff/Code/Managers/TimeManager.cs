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

    private List<float> miniGameStart;
    private List<float> miniGameEnd;

    public void RecordTime(TimeEventType currentEvent)
    {
        switch (currentEvent)
        {
            case TimeEventType.MiniGameStart:
                miniGameStart.Add(Time.deltaTime);
                break;
            case TimeEventType.MiniGameEnd:
                miniGameEnd.Add(Time.deltaTime);
                gameCount++;
                break;
            case TimeEventType.MiniGameCancel:
                if(miniGameStart.Count > 0)
                {
                    miniGameStart.RemoveAt(gameCount - 1);
                }
                gameCount++;
                break;
        }
    }

    public float CalculateTurnScore()
    {
        float turnScore; 
        foreach(float startTime in miniGameStart)
        {
            turnScore =+ startTime;
        }
        return turnScore = 0;    
    }
}
