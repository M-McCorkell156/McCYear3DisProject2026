using System.Collections;
using UnityEngine;

public class ClockLightMenu : MonoBehaviour
{
    [SerializeField] private Animator animitor;
    [SerializeField] private GameObject handToTurn;
    [SerializeField] private float timeCountdown;
    [SerializeField] private float turnTimeLimit;
    [SerializeField] private GameObject Light;
    private void Start()
    {
        StartClock();
        timeCountdown = turnTimeLimit;
        lightOff(); 
    }

    private IEnumerator MoveClock()
    {
        //Debug.Log("Starting Clock");
        Quaternion newAngle;
        while (timeCountdown > 0)
        {
            //Debug.Log("Countdown: " + timeCountdown);
            newAngle = Quaternion.Euler((timeCountdown / turnTimeLimit) * -360f, Quaternion.identity.y,Quaternion.identity.z);
            handToTurn.transform.localRotation = Quaternion.Lerp(Quaternion.identity, newAngle, 1f);
            animitor.SetTrigger("Tick");

            if (Mathf.Round(turnTimeLimit / timeCountdown) == 2)
            {
                //Debug.Log("Light on");
                lightOn();
                Invoke(nameof(lightOff), 0.2f);
            }

            yield return new WaitForSeconds(1.0f);
            timeCountdown--;
        }
        StartClock();
    }

    private void StartClock()
    {
        timeCountdown = turnTimeLimit;
        StartCoroutine(MoveClock());
    }
    private void lightOn()
    {
        Light.SetActive(true);
    }
    private void lightOff()
    {
        Light.SetActive(false);
    }
}
