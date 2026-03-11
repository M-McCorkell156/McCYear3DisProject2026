using UnityEngine;

public class AddOutline : MonoBehaviour
{
    private Outline Outline;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
            this.gameObject.AddComponent<Outline>();
   }
}
