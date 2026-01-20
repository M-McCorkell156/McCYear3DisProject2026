using UnityEngine;

public class BGMManager : MonoBehaviour
{
    [SerializeField] private AudioSource BGMLow;
    [SerializeField] private AudioSource BGMMid;
    [SerializeField] private AudioSource BGMHigh;
    private void Start()
    {
        SetLow();
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
