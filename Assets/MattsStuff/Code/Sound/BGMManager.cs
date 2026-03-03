using UnityEngine;
public enum BGMState
{
    Low,
    Mid,
    High
}
public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource BGMLow;
    [SerializeField] private AudioSource BGMMid;
    [SerializeField] private AudioSource BGMHigh;
    private BGMState currentState;
    private void Start()
    {
        SetBGMState(BGMState.Low);
        PlayBGM();
    }
    public void PlayBGM()
    {
        BGMLow.Play();
        BGMMid.Play();
        BGMHigh.Play();
        Invoke(nameof(PlayBGM), BGMLow.clip.length);
    }
    public void SetBGMState(BGMState newState)
    {
        currentState = newState;
        switch (currentState)
        {
            case BGMState.Low:
                SetLow();
                break;
            case BGMState.Mid:
                SetMid();
                break;
            case BGMState.High:
                SetHigh();
                break;
        }
    }
    public void SetLow()
    {
        BGMLow.volume = 0.1f;
        BGMMid.volume = 0f;
        BGMHigh.volume = 0f;
    }
    public void SetMid()
    {
        BGMLow.volume = 0f;
        BGMMid.volume = 0.1f;
        BGMHigh.volume = 0f;
    }
    public void SetHigh()
    {
        BGMLow.volume = 0f;
        BGMMid.volume = 0f;
        BGMHigh.volume = 0.1f;
    }
}
