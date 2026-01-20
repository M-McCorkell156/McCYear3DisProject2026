using UnityEngine;

public class IntensitySetter : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            animator.SetTrigger("SetLow");
        }
        else if(Input.GetKeyDown(KeyCode.M))
        {
            animator.SetTrigger("SetMid");
        }
        else if(Input.GetKeyDown(KeyCode.H))
        {
            animator.SetTrigger("SetHigh");
        }
    }
}
