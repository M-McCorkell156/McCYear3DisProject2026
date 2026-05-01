using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool tutorialPlayed = false;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List<GameObject> CharacterObj;
    [SerializeField] private List<GameObject> BooksObj;
    [SerializeField] private List<GameObject> TutObjs;
    [SerializeField] private List<GameObject> DefaultObjs;
    [SerializeField] private List<GameObject> Level_1Objs;
    [SerializeField] private List<GameObject> Level_2Objs;
    [SerializeField] private GameObject Begin;
    [SerializeField] private float enableTime;

    private float currentLvlSelection;
    [SerializeField] private bool isMenu;

    private void Start()
    {
        if (isMenu)
            PlayCharacter();
    }

    public void PlayBooks()
    {
        //Debug.Log("Playing Books");
        HideCharacter();
        StartCoroutine(LerpCamera(105));
        Invoke(nameof(EnableBooks), enableTime);
    }

    private IEnumerator LerpCamera(float xToRotate)
    {
        //Debug.Log("Starting LerpCamera with target X rotation: " + xToRotate);
        Quaternion targetRot = Quaternion.Euler(xToRotate, 0f, 0f);

        //Debug.Log("Target rotation for camera: " + targetRot.eulerAngles);
        //Debug.Log("Target rotation for camera:: " + Quaternion.Euler(xToRotate, 0f, 0f));

        while (Mathf.Abs(Mathf.DeltaAngle(mainCamera.transform.localRotation.eulerAngles.x, xToRotate)) > 1)
        {
            //Debug.Log("Lerping camera. Current rotation: " + mainCamera.transform.localRotation.eulerAngles);
            mainCamera.transform.localRotation = Quaternion.RotateTowards(mainCamera.transform.localRotation, targetRot, 50 * Time.deltaTime);

            yield return null;
        }
        //Debug.Log("Finished LerpCamera. Final rotation: " + mainCamera.transform.localRotation.eulerAngles);
        mainCamera.transform.localRotation = targetRot;
       //Debug.Log("Set camera to target rotation: " + mainCamera.transform.localRotation.eulerAngles);
        yield return null;

    }
    private void EnableBooks()
    {
        foreach (GameObject book in BooksObj)
        {
            book.SetActive(true);
        }
    }
    private void HideBooks()
    {
        HideBegin();

        foreach (GameObject book in BooksObj)
        {
            book.SetActive(false);
        }
    }

    public void TurnPage(int pageNumber)
    {
        HidePages();

        switch (pageNumber)
        {
            case 1:
                ShowBook(Level_1Objs);
                break;
            case 2:
                ShowBook(Level_2Objs);
                break;
            case 3:
                ShowBook(TutObjs);
                break;
            default:
                ShowBook(DefaultObjs);
                break;
        }

        ShowBegin();
    }

    private void ShowBegin()
    {
        Begin.SetActive(true);
    }

    private void HideBegin()
    {
        Begin.SetActive(false);
    }

    private void ShowBook(List<GameObject> bookList)
    {
        foreach (GameObject book in bookList)
        {
            book.SetActive(true);
        }
    }
    private void HidePages()
    {
        foreach (GameObject book in TutObjs)
        {
            book.SetActive(false);
        }
        foreach (GameObject book in Level_1Objs)
        {
            book.SetActive(false);
        }
        foreach (GameObject book in Level_2Objs)
        {
            book.SetActive(false);
        }
        foreach (GameObject book in DefaultObjs)
        {
            book.SetActive(false);
        }
    }

    public void PlayCharacter()
    {
        Debug.Log("Playing Character");
        HideBooks();
        StartCoroutine(LerpCamera(55));
        Invoke(nameof(EnableCharacter), enableTime);
    }
    private void EnableCharacter()
    {
        foreach (GameObject character in CharacterObj)
        {
            character.SetActive(true);
        }
    }
    private void HideCharacter()
    {
        foreach (GameObject character in CharacterObj)
        {
            character.SetActive(false);
        }
    }


    public void PlayMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayTutorial()
    {
        Debug.Log("Playing Tutorial :" + SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        tutorialPlayed = true;
    }

    public void PlayLevel_1()
    {
        if (CheckForPlay())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
        }
    }

    public void PlayLevel_2()
    {
        if (CheckForPlay())
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 3);
        }
    }
    public void SelectLvl1()
    {
        currentLvlSelection = 1;
    }

    public void SelectLvlT()
    {
        currentLvlSelection = 0;
    }

    public void PlaYGame(
        )
    {
        if (currentLvlSelection == 1)
        {
            PlayLevel_1();
        }
        else
        {
            PlayTutorial();
        }
    }

    private bool CheckForPlay()
    {
        //if (!tutorialPlayed)
        //{
        //    PlayTutorial();
        //    return false;
        //}
        return true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
