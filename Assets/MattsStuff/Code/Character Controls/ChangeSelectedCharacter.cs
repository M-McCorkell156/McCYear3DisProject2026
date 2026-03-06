using System.Collections;
using UnityEngine;
public enum Characters
{
    Ashley,
    Joe,
    None
}
public class ChangeSelectedCharacter : MonoBehaviour
{

    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject currentCharacter;

    private GridMover gridMover;
    [SerializeField] private GridManager gridManager;
    [SerializeField] private GameObject[] playableCharacters;
    [SerializeField] private GameObject[] CameraPos;
    private GameObject MoveToCamera;

    public Characters character;
    private bool canSwitch;
    private bool shouldSwitch;

    private void Start()
    {
        canSwitch = true;
        shouldSwitch = false;
        currentCharacter = playableCharacters[0];
        gridMover = currentCharacter.GetComponent<GridMover>();
        mainCamera.transform.SetParent(currentCharacter.transform);
        mainCamera.transform.localPosition = new Vector3(25f, 45f, currentCharacter.transform.position.z);
        gridManager.SetCurrentCharacter();
        gridMover.UnfreezeGridMoves();
    }
    public void UnlockSwitching()
    {
        //Debug.Log("Unlocking Switching");
        shouldSwitch = true;
    }

    public void LockSwitching()
    {
        shouldSwitch = false;
    }

    public void FreezeSwitching()
    {
        canSwitch = false;
    }

    public void UnFreezeSwitch()
    {
        canSwitch = true;
    }

    public bool CanSwitch()
    {
        return canSwitch;
    }
    public bool ShouldSwitch()
    {
        return shouldSwitch;
    }
    public void ChangeCharacter()
    {
        //Debug.Log(currentCharacter);
        Debug.Log(canSwitch +":"+ shouldSwitch);

        if (!canSwitch || !shouldSwitch)
            return;

        //Debug.Log("Changing Character to :" + character);

        
        switch (character)
        {
            case Characters.Ashley:
                currentCharacter = playableCharacters[0];
                //MoveToCamera = CameraPos[0];
                break;
            case Characters.Joe:
                currentCharacter = playableCharacters[1];
                //MoveToCamera = CameraPos[1];
                break;
        }
        //Debug.Log("Changing to: " + currentCharacter);
        gridMover.FreezeGridMoves();
        
        gridMover = currentCharacter.GetComponent<GridMover>();
        gridManager.SetCurrentCharacter();

        gridManager.ResetHighlights();

        mainCamera.transform.SetParent(currentCharacter.transform);

        StartCoroutine(MoveCameraToCharacter());

        //mainCamera.transform.localPosition = new Vector3( 25f, 45f, currentCharacter.transform.position.z); 
        //Debug.Log("Character Changed to: " + currentCharacter);
    }

    private IEnumerator MoveCameraToCharacter()
    {
        Vector3 targetPos = new Vector3(25f, 45f, currentCharacter.transform.position.z);
        float distance = Vector2.Distance(targetPos, mainCamera.transform.localPosition);

        while (distance >= 0.2f)
        {
            distance = Vector2.Distance(targetPos, mainCamera.transform.localPosition);
            //Debug.Log("Distance: " + distance);
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, targetPos, 0.008f);
            yield return null;
        }

        //Debug.Log("Camera moved to: " + currentCharacter);
        mainCamera.transform.localPosition = targetPos;
        gridMover.UnfreezeGridMoves();
        yield return null;
    }

    public void SelectAshley()
    {
        if(shouldSwitch)
            character = Characters.Ashley;
    }
    public void SelectJoe()
    {
        if(shouldSwitch)
            character = Characters.Joe;
    }

    public GameObject GetCurrentCharacterObject()
    {
        return currentCharacter;
    }

    public Characters GetCharacter()
    {
        return character;
    }
}
