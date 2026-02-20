using UnityEngine;
using System.Collections;

public class WallColide : MonoBehaviour
{
    void Start()
    {
        CheckForWalls();
    }

    public void CheckForWalls()
    {
        //Debug.Log("Checking for walls...");
        Collider[] colliders = Physics.OverlapBox(transform.position, transform.localScale / 2, Quaternion.identity);

        GridManager gridManager = FindObjectOfType<GridManager>();


        foreach (Collider col in colliders)
        {
            if (col.gameObject.CompareTag("Wall"))
            {
                if (gridManager != null)
                {
                    //Debug.Log("Wall detected: " + col.gameObject.name);
                    gridManager.AddWallTile(this.gameObject.name);
                }
            }
        }

        gridManager.ReplaceWallTile();


    }
}
