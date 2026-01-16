using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class SearchingScript : MonoBehaviour
{
    [SerializeField] private Animator animitor;

    private bool isSearching;
    private bool isCancled;
    [SerializeField] private GameStateManager gameStateManager;

    [SerializeField] private Image searchProgressBar;
    public static event WinHandler SearchFinished;

    void OnEnable()
    {
        Debug.Log("awake");
        isCancled = false;
        StartSearchCountdown();
        searchProgressBar.fillAmount = 0f;
    }
    private void StartSearchCountdown()
    {
        StartCoroutine(SearchProgress());
        animitor.SetBool("isSearching", true);
    }

    private IEnumerator SearchProgress()
    {
        isSearching = true;
        searchProgressBar.fillAmount = 0f;
        
        //Debug.Log("Searching...");
        yield return new WaitForSeconds(2f);

        isSearching = false;
        if (isCancled)
        {
            gameStateManager.DeactivateSearchMiniGame();
        }
        else
        {
            if (SearchFinished != null)
            {
                animitor.SetBool("isSucess", true);
                SearchFinished();
            }
            gameStateManager.CompleteSearchMiniGame();
        }

    }

    void Update()
    {
        if (isSearching && !isCancled)
        {
            searchProgressBar.fillAmount += 0.0014f;
        }
    }

    public void CancleSearch()
    {
        isCancled = true;
        isSearching = false;
        searchProgressBar.fillAmount = 0f;
        animitor.SetBool("isFail", true);
    }
}
