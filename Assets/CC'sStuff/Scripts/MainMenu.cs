using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool tutorialPlayed = false;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private GameObject CharacterObj;
    [SerializeField] private GameObject BooksObj;

    private void Start()
    {
        PlayCharacter();
    }

    public void PlayBooks()
    {
        HideCharacter();
        StartCoroutine(LerpCamera(105));
        Invoke(nameof(EnableBooks), 1.5f);
    }

    private IEnumerator LerpCamera(float xToRotate)
    {
        Debug.Log("Starting LerpCamera with target X rotation: " + xToRotate);
        while (Mathf.Abs(mainCamera.transform.rotation.eulerAngles.x - xToRotate) > 0.1f)
        {
            mainCamera.transform.rotation = Quaternion.Lerp(mainCamera.transform.rotation, Quaternion.Euler(xToRotate, 0, 0), Time.deltaTime * 2);
            Debug.Log(mainCamera.transform.rotation.eulerAngles.x);
        }
        yield return null;
    }
    private void EnableBooks()
    {
        BooksObj.SetActive(true);
    }
    private void HideBooks()
    {
        BooksObj.SetActive(false);
    }

    public void PlayCharacter()
    {
        HideBooks();
        StartCoroutine(LerpCamera(55));
        Invoke(nameof(EnableCharacter), 1.5f);
    }
    private void EnableCharacter()
    {
        CharacterObj.SetActive(true);
    }
    private void HideCharacter()
    {
        CharacterObj.SetActive(false);
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
