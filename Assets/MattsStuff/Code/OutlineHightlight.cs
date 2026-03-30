using UnityEngine;

public class OutlineHightlight : MonoBehaviour
{
    private Outline Outline;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Add()
    {
        Debug.Log("Adding outline to " + this.gameObject.name);
        this.gameObject.AddComponent<Outline>();
    }

    public void Remove()
    {
        Debug.Log("Removing outline from " + this.gameObject.name);
        Destroy(this.gameObject.GetComponent<Outline>());
    }
}

