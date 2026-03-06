using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    private bool tutorialPlayed = false;


    public void PlayMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayTutorial()
    {
        Debug.Log("Playing Tutorial :"+ SceneManager.GetActiveScene().buildIndex);
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
