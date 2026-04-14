using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;

public class WinConditionUI : MonoBehaviour
{
    [SerializeField] private GameObject winText;

    [SerializeField] private GameObject Tire;
    [SerializeField] private GameObject TireOutline;


    [SerializeField] private GameObject Keys;
    [SerializeField] private GameObject KeysOutline;

    [SerializeField] private GameObject Bat;
    [SerializeField] private GameObject BatOutline;

    [SerializeField] private GameObject Power;

    [SerializeField] private GameObject PowerOutline;

    [SerializeField] private GameObject CanEscapeObj;

    public static event WinHandler CanLeave;

    private bool onceLeave;
    private int characterCount = 0;

    private bool gotKey;
    private bool gotBat;
    private bool gotTire;
    private bool gotPower;
    private bool canEscape;

    [SerializeField] private bool tutorial;
    [SerializeField] private GameObject Door1;
    [SerializeField] private GameObject Door2;

    [SerializeField] private MainMenu mainMenu;
    [SerializeField] private ChangeSelectedCharacter changeSelectedCharacter;

    [SerializeField] private GameObject BlockBoxes_1;
    [SerializeField] private GameObject BlockBoxes_2;

    [SerializeField] private GridManager gridManager;

    [SerializeField] private GameObject SwitchLock;

    [SerializeField] private GameObject Enemy;

    [SerializeField] private GameObject Switch; 
    [SerializeField] private GameObject PowerTxt;
    [SerializeField] private GameObject Collect;
    [SerializeField] private GameObject Run;
    void Start()
    {
        gotKey = false;
        gotBat = false;
        gotTire = false;
        gotPower = false;
        onceLeave = false;

        CanEscapeObj.SetActive(false);
        winText.SetActive(false);

        Tire.SetActive(false);
        TireOutline.SetActive(true);
        Keys.SetActive(false);
        KeysOutline.SetActive(true);
        Bat.SetActive(false);
        BatOutline.SetActive(true);
        Power.SetActive(false);
        PowerOutline.SetActive(true);

        if(tutorial)
        {
            BlockBoxes_1.SetActive(true);
            BlockBoxes_2.SetActive(true);

            Switch.SetActive(false);
        }

        GameStateManager.LockPickWin += GotKey;
        GameStateManager.SearchFinished += GotTire;
        GameStateManager.WeaponGot += GotBat;
        GameStateManager.PowerOn += GotPower;
        GameStateManager.Escape += OnEscape;

        if (tutorial)
        {
            SwitchLock.SetActive(true);
            Switch.SetActive(false);
            PowerTxt.SetActive(false);
            Collect.SetActive(false);
            Run.SetActive(false);
            //Enemy.SetActive(false);
        }
        else
        {
            SwitchLock.SetActive(false);
            changeSelectedCharacter.UnlockSwitching();
        }
    }
    public bool IsTutorial()
    {
        return tutorial;
    }   
    void GotKey()
    {
        if (!gotKey)
        {
            //Debug.Log("key");

            Keys.SetActive(true);
            KeysOutline.SetActive(false);
            gotKey = true;

            if (tutorial)
            {
                //Debug.Log("yes tut");
                changeSelectedCharacter.UnlockSwitching();
                SwitchLock.SetActive(false);
                StartCoroutine(OpenDoor(Door1));

                gridManager.ClearBlockedTiles1();
                Switch.SetActive(true);
                PowerTxt.SetActive(true);
            }
        }
    }

    private IEnumerator OpenDoor(GameObject door)
    {
        //Debug.Log(door.transform.rotation.eulerAngles.y);

        while (door.transform.rotation.eulerAngles.y < 180)
        {
            door.transform.Rotate(0, 90 * Time.deltaTime, 0);
            yield return null;
        }
    }
    void GotBat()
    {
        if (!gotBat)
        {
            Bat.SetActive(true);
            BatOutline.SetActive(false);
            gotBat = true;
        }

    }
    void GotTire()
    {
        if (!gotTire)
        {
            Tire.SetActive(true);
            TireOutline.SetActive(false);
            gotTire = true;
        }

    }

    void GotPower()
    {
        {
            if (!gotPower)
                gotPower = true;
            Power.SetActive(true);
            PowerOutline.SetActive(false);

            if (tutorial)
            {
                //Debug.Log("yes tut");
                StartCoroutine(OpenDoor(Door2));
                BlockBoxes_2.SetActive(false);
                gridManager.ClearBlockedTiles2();
                Collect.SetActive(true);
                //Enemy.SetActive(true);
            }
        }
    }

    void OnEscape()
    {
        characterCount += 1;
        //Debug.Log("EscapeNo: " + characterCount);
        if (characterCount == 2)
        {
            //Debug.Log("2 Escape");
            canEscape = true;
        }
    }

    void Update()
    {
        if (gotKey && gotTire && gotBat && gotPower && !onceLeave && winText.gameObject.activeSelf == false)
        {
            //Debug.Log("can leave");
            CanEscapeObj.SetActive(true);
            onceLeave = true;
            CanLeave();
            Run.SetActive(true);
        }
        if (onceLeave && canEscape)
        {
            winText.SetActive(true);
            StartCoroutine(LoadMainMenu());
            onceLeave = false;
        }

    }

    private IEnumerator LoadMainMenu()
    {
        yield return new WaitForSeconds(5);
        mainMenu.PlayMenu();
    }
}
