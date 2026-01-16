using TMPro;
using UnityEngine;
using UnityEngine.ProBuilder.MeshOperations;
using UnityEngine.UI;


public class LockpickingMiniGame : MonoBehaviour
{
    [Header("Objects")]
    [SerializeField] private GameObject centreOfPick;
    [Header("Game State Manager")]
    [SerializeField] private GameStateManager anyGameStateManager;
    [Header("Canvas")]
    [SerializeField] private Image progressBar;
    [SerializeField] private Image breakBar;
    [SerializeField] private GameObject WinText;
    [SerializeField] private GameObject LostText;
    [Header("Debug Angles")]
    [SerializeField][Range(-90, 90)] private float greenSpotAngle;
    [SerializeField][Range(-90, 90)] private float currentAngle;
    [SerializeField] private float percentLeftToGreen;
    [SerializeField] private float percentRightToGreen;
    [Header("Limits and Distances")]
    [SerializeField][Range(20, 40)] private float FullIndicatorDistance;
    [SerializeField] private float UnlockIndicatorDistance;

    [SerializeField] private float unlockProgress;
    [SerializeField] private float breakingProgress;
    [SerializeField] private float breakThreshold;
    [SerializeField] private float unlockThreshold;

    [Header("Timers")]
    [SerializeField][Range(0.5f, 5f)] private float finishTime;

    private float percentageTurn; //0-S1

    private Vector3 screenPosition;
    private float centreOfScreen;

    private bool isPicking;
    private bool gameOver;

    public delegate void FreezeHandler();
    public static event FreezeHandler freezeGridMove;
    public static event FreezeHandler unfreezeGridMoves;
    public static event FreezeHandler LockPickWin;
    public static event WinHandler LockPickWinEvent;

    [Header("Audio Animitor")]
    [SerializeField] private Animator animitor;

    private void OnEnable()
    {
        GameSetUp();
        freezeGridMove?.Invoke();
    }
    private void FixedUpdate()
    {
        if (gameOver)
            return;
        #region  Rotation with mouse input

        //Mouse Input
        screenPosition = Input.mousePosition;
        //Debug.Log(screenPosition);

        if (!isPicking)
        {
            if (screenPosition.x < 0 || screenPosition.x > Screen.width || screenPosition.y < 0 || screenPosition.y > Screen.height)
            {
                //Not on screen do nothing
                return;
            }

            //right side of screen
            else if (screenPosition.x > Screen.width / 2)
            {
                percentageTurn = Mathf.InverseLerp(centreOfScreen, Screen.width, screenPosition.x);
                //Debug.Log(percentageTurn);
                currentAngle = percentageTurn * 90f;
            }

            //left side of screen
            else if (screenPosition.x < Screen.width / 2)
            {
                percentageTurn = Mathf.InverseLerp(centreOfScreen, 0, screenPosition.x);
                //Debug.Log(percentageTurn);
                currentAngle = (-percentageTurn * 90f);

            }

            centreOfPick.transform.rotation = Quaternion.Euler(centreOfPick.transform.rotation.eulerAngles.x, centreOfPick.transform.rotation.eulerAngles.y, currentAngle);

            //Debug.Log(currentAngle);
            #endregion
        }

        #region Unlocking Mechanism

        //Debug.Log(pick.transform.rotation.eulerAngles.z);

        if (breakingProgress >= breakThreshold)
        {
            animitor.SetBool("isFail", true);
            gameOver = true;
            //Debug.Log("Broke");
            LostText.SetActive(true);
            unfreezeGridMoves();
            Invoke("FinishGame", finishTime);
        }

        else if (isPicking && currentAngle >= greenSpotAngle - FullIndicatorDistance && currentAngle <= greenSpotAngle + FullIndicatorDistance)
        {

            percentLeftToGreen = Mathf.InverseLerp(greenSpotAngle - FullIndicatorDistance, greenSpotAngle, currentAngle);
            percentRightToGreen = Mathf.InverseLerp(greenSpotAngle + FullIndicatorDistance, greenSpotAngle, currentAngle);

            //Debug.Log("Percent Left to Green: " + percentLeftToGreen);
            //Debug.Log("Percent Right to Green: " + percentRightToGreen);
            //Debug.Log(percentLeftToGreen * 100);
            //Debug.Log(percentRightToGreen * 100);

            //Unlocking area
            if (currentAngle >= greenSpotAngle - UnlockIndicatorDistance)
            {
                unlockProgress += 1f;
                progressBar.fillAmount = unlockProgress / unlockThreshold;
                animitor.SetBool("isUnlocking", true);
                animitor.SetBool("isBreaking", false);
                //Debug.Log("Yes");
            }
            //Left side percent
            else if (unlockProgress < (percentLeftToGreen * 100f) && percentRightToGreen == 1f)
            {
                unlockProgress += 1f;
                progressBar.fillAmount = unlockProgress / unlockThreshold;
                animitor.SetBool("isUnlocking", true);
                animitor.SetBool("isBreaking", false);
                //Debug.Log("Unlocking Left");
            }

            else if (unlockProgress < (percentRightToGreen * 100f) && percentLeftToGreen == 1f)
            {
                unlockProgress += 1f;
                progressBar.fillAmount = unlockProgress / unlockThreshold;
                animitor.SetBool("isUnlocking", true);
                animitor.SetBool("isBreaking", false);
                //Debug.Log("Unlocking Right");

            }
            else
            {
                //Debug.Log("Breaking");
                breakingProgress += 0.5f;
                breakBar.fillAmount = breakingProgress / breakThreshold;
                animitor.SetBool("isUnlocking", false);
                animitor.SetBool("isBreaking", true);

            }


            //Debug.Log("Unlocking");

            if (unlockProgress >= unlockThreshold)
            {
                animitor.SetBool("isSucess", true);
                gameOver = true;
                WinText.SetActive(true);

                anyGameStateManager.CompletePickMiniGame();

                Invoke("FinishGame", finishTime);
            }
        }

        else if (isPicking && (currentAngle < greenSpotAngle - FullIndicatorDistance || currentAngle > greenSpotAngle + FullIndicatorDistance))
        {
            //Debug.Log("Breaking");
            breakingProgress += 0.5f;
            breakBar.fillAmount = breakingProgress / breakThreshold;
            animitor.SetBool("isUnlocking", false);
            animitor.SetBool("isBreaking", true);
        }

        else
        {
            unlockProgress = 0f;
            progressBar.fillAmount = 0f;
            //Debug.Log("Locked");
        }
        #endregion
    }

    private void Update()
    {
        if (gameOver)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            isPicking = true;
            //Debug.Log(isPicking);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPicking = false;
            //Debug.Log(isPicking);
        }
    }

    private void GameSetUp()
    {
        gameOver = false;
        isPicking = false;
        //Debug.Log("New Green Spot Angle: " + greenSpotAngle);
        unlockProgress = 0f;
        progressBar.fillAmount = 0f;
        breakingProgress = 0f;
        breakBar.fillAmount = 0f;
        centreOfScreen = Screen.width / 2;
        greenSpotAngle = Random.Range(-90f, 90f);

        WinText.SetActive(false);
        LostText.SetActive(false);
    }
    public void FinishGame()
    {
        if(unfreezeGridMoves != null)
        unfreezeGridMoves();
        
        this.gameObject.SetActive(false);

    }
}
