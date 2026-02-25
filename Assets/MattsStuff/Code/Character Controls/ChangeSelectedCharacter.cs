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
    public Characters character;
    private bool canSwitch;

    private void Start()
    {
        currentCharacter = playableCharacters[0];
        gridMover = currentCharacter.GetComponent<GridMover>();
        mainCamera.transform.SetParent(currentCharacter.transform);
        mainCamera.transform.localPosition = new Vector3(25f, 45f, currentCharacter.transform.position.z);
        gridManager.SetCurrentCharacter();
        gridMover.UnfreezeGridMoves();
    }
    public void UnlockSwitching()
    {
        canSwitch = true;
    }

    public bool CanSwitch()
    {
        return canSwitch;
    }
    public void ChangeCharacter()
    {
        //Debug.Log("Changing Character");
        if (!canSwitch)
            return;

        if (currentCharacter == playableCharacters[(int)character])
            return;

        switch (character)
        {
            case Characters.Ashley:
                currentCharacter = playableCharacters[0];
                break;
            case Characters.Joe:
                currentCharacter = playableCharacters[1];
                break;
        }

        gridMover.FreezeGridMoves();
        
        gridMover = currentCharacter.GetComponent<GridMover>();
        gridManager.SetCurrentCharacter();

        gridManager.ResetHighlights();

        mainCamera.transform.SetParent(currentCharacter.transform);
        StartCoroutine(MoveCameraToCharacter());
        //mainCamera.transform.localPosition = new Vector3( 25f, 45f, currentCharacter.transform.position.z); 
    }

    private IEnumerator MoveCameraToCharacter()
    {
        Vector3 targetPos = new Vector3(25f, 45f, currentCharacter.transform.position.z);
        while (mainCamera.transform.localPosition != targetPos)
        {
            mainCamera.transform.localPosition = Vector3.Lerp(mainCamera.transform.localPosition, targetPos, Time.deltaTime);
            yield return null;
        }
        gridMover.UnfreezeGridMoves();
    }

    public void SelectAshley()
    {
        character = Characters.Ashley;
    }
    public void SelectJoe()
    {
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
