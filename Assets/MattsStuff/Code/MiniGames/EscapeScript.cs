using System.Collections.Generic;
using UnityEngine;

public class EscapeScript : MonoBehaviour
{
    private bool char1Exit;
    private bool char2Exit;

    [SerializeField] private GridManager gridManager;
    [SerializeField] private List<GameStateManager> gameStateManagers;
    public void ActivateEscape()
    {
        if (!char1Exit)
        {
            //Debug.Log("1 on");
            gridManager.FreezeCurrentGridMover();
            gridManager.SetPoweredCharacter();
            char1Exit = true;
        }
        else if (char1Exit && !char2Exit)
        {
            //Debug.Log("2 on");
            char2Exit = true;
            foreach (GameStateManager gameManager in gameStateManagers)
            {               
                //Debug.Log("unfreeze");
            }
        }
        else
        {
            //Debug.Log("error");
        }
    }
}
