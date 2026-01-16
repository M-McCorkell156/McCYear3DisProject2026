using UnityEngine;
using System.Collections.Generic;

public delegate void FreezeTileMove();
public class PowerScript : MonoBehaviour
{
    [SerializeField] private Animator animitor;

    private bool switch1Active;
    private GameObject switch1;

    private bool switch2Active;
    private GameObject switch2;

    private float switchCount;
    private bool powerOn;

    [SerializeField] private List<GameStateManager> gameStateManagers;
    [SerializeField] private GridManager gridManager;

    private void Awake()
    {
        ResetSwitches();
    }
    public void ResetSwitches()
    {
        //Debug.Log("Reset");
        switch1Active = false;
        switch2Active = false;

        foreach (GameStateManager gameManager in gameStateManagers)
        {
            gridManager.UnfreezeCurrentGridMover();
            //Debug.Log("unfreeze");
        }
    }

    private void Update()
    {
        //Debug.Log("Update");

        if (switch1Active && switch2Active)
        {
            powerOn = true;
            //Debug.Log("PowerOn");
            foreach (GameStateManager gameManager in gameStateManagers)
            {
                gameManager.CompletePowerGame();
                //Debug.Log("gamemanaged");
            }
        }
    }


    public void ActivateSwitch()
    {
        if (!switch1Active)
        {
            //Debug.Log("1 on");
            animitor.SetBool("FlipSwitch", true);
            gridManager.FreezeCurrentGridMover();
            gridManager.SetPoweredCharacter();
            switch1Active = true;
        }
        else if (switch1Active && !switch2Active)
        {
            //Debug.Log("2 on");
            switch2Active = true;
            foreach (GameStateManager gameManager in gameStateManagers)
            {
                gridManager.UnfreezeCurrentGridMover();
                animitor.SetBool("isSucess", true);
                //Debug.Log("unfreeze");
            }
        }
        else
        {
            //Debug.Log("error");
        }
    }

    public void CanclePower()
    {
        ResetSwitches();
        animitor.SetBool("isFail",true);

        if (switch1Active)
        {
            foreach (GameStateManager gameManager in gameStateManagers)
            {
                if (gridManager.GetPoweredGameStateManager() == null)
                {
                    break;
                }
                else if (gameManager == gridManager.GetPoweredGameStateManager())
                {
                    gameManager.DeactivatePowerGame();
                }
            }
        }

    }
}
