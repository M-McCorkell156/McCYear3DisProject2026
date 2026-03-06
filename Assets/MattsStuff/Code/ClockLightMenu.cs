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
        StartCoroutine(MoveClock());
        lightOff(); 
    }

    private IEnumerator MoveClock()
    {
        Quaternion newAngle;
        while (timeCountdown > 0)
        {
            //Debug.Log("Countdown: " + timeCountdown);
            newAngle = Quaternion.Euler(Quaternion.identity.x, Quaternion.identity.y, (timeCountdown / turnTimeLimit) * -360f);
            handToTurn.transform.localRotation = Quaternion.Lerp(Quaternion.identity, newAngle, 1f);
            animitor.SetTrigger("Tick");
            if (turnTimeLimit / timeCountdown == 2)
            {
                //Debug.Log("Light on");
                lightOn();
                Invoke(nameof(lightOff), 0.2f);
            }
            yield return new WaitForSeconds(1.0f);
            timeCountdown--;
        }
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
