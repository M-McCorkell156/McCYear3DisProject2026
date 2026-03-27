using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool tutorialPlayed = false;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private List <GameObject> CharacterObj;
    [SerializeField] private List <GameObject> BooksObj;
    [SerializeField] private List <GameObject> TutObjs;
    [SerializeField] private List <GameObject> Level_1Objs;
    [SerializeField] private List <GameObject> Level_2Objs;
    [SerializeField] private GameObject Begin;
    [SerializeField] private float enableTime;

    private void Start()
    {
        PlayCharacter();
    }

    public void PlayBooks()
    {
        HideCharacter();
        StartCoroutine(LerpCamera(105));
        Invoke(nameof(EnableBooks), enableTime);
    }

     private IEnumerator LerpCamera(float xToRotate)
    {
        //Debug.Log("Starting LerpCamera with target X rotation: " + xToRotate);
  
        Quaternion targetRot = Quaternion.Euler(xToRotate, 0f, 0f);

        while (Mathf.Abs(Mathf.DeltaAngle(mainCamera.transform.rotation.eulerAngles.x, xToRotate)) > 1)
        {
            mainCamera.transform.rotation = Quaternion.RotateTowards(mainCamera.transform.rotation,targetRot, 20 * Time.deltaTime);

            yield return null;
        }

        mainCamera.transform.rotation = targetRot;
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
        HideAllBooks();

        switch (pageNumber)
        {
            case 1:
                ShowBook(Level_1Objs);
                break;
            case 2:
                ShowBook(Level_2Objs);
                break;

            default:
                ShowBook(TutObjs);
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
    private void HideAllBooks()
    {
        foreach (GameObject book in BooksObj)
        {
            book.SetActive(false);
        }
    }

    public void PlayCharacter()
    {
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
