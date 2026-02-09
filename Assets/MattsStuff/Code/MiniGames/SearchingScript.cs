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
    [SerializeField] private TimeManager timeManager;

    [SerializeField] private Image searchProgressBar;
    public static event WinHandler SearchFinished;

    void OnEnable()
    {
        //Debug.Log("awake");
        isCancled = false;
        StartSearchCountdown();
        searchProgressBar.fillAmount = 0f;
    }
    private void StartSearchCountdown()
    {
        StartCoroutine(SearchProgress());
        timeManager.RecordTime (TimeEventType.MiniGameStart);
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
                SearchFinished();
            }
            animitor.SetBool("isSucess", true);
            timeManager.RecordTime(TimeEventType.MiniGameEnd);
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
        if (isSearching)
        {
            timeManager.RecordTime(TimeEventType.MiniGameCancel);

            animitor.SetBool("isFail", true);
            isCancled = true;
            isSearching = false;
            searchProgressBar.fillAmount = 0f;
            animitor.SetBool("isFail", false);
            animitor.SetBool("isSucess", false);
            animitor.SetBool("isSearching", false);

        }
    }
}
