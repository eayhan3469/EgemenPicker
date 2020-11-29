using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform Picker;

    [SerializeField]
    private TextMeshProUGUI CurrentLevelText;

    [SerializeField]
    private TextMeshProUGUI NextLevelText;

    [SerializeField]
    private List<Image> PartImages;

    [SerializeField]
    private GameObject GameFailedPanel;

    [SerializeField]
    private GameObject GameSuccessPanel;

    [SerializeField]
    private GameObject GameStartPanel;

    public static GameManager Instance { get; private set; }

    public float PickerSpeed;
    public bool DroneHasStart;
    public bool LockPicker;
    public int LevelMultiplier;

    private int CurrentLevel;
    private int CurrentPart;
    private Level Level;
    private bool GameFailed;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        PickerSpeed = 7f; //TODO: Level'a göre değişecek
        DroneHasStart = false;
        LockPicker = true;

        //PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", 1);

        CurrentPart = 1;
        CurrentLevel = PlayerPrefs.GetInt("Level");
        LevelMultiplier = (CurrentLevel / 10) + 1;
        GameStartPanel.SetActive(true);

        if (CurrentLevel > 8)
        {
            if (!PlayerPrefs.HasKey("LevelModel"))
            {
                PlayerPrefs.SetInt("LevelModel", UnityEngine.Random.Range(1, 9));
            }

            var levelObject = Resources.Load<GameObject>(String.Format("Levels/Level{0}", PlayerPrefs.GetInt("LevelModel")));
            Instantiate(levelObject);
        }
        else
        {
            var levelObject = Resources.Load<GameObject>(String.Format("Levels/Level{0}", CurrentLevel));
            Instantiate(levelObject);
        }
    }

    private void Start()
    {
        Level = FindObjectOfType<Level>();
        UpdateUI();
    }

    private void UpdateUI()
    {
        CurrentLevelText.text = String.Format("{0}", CurrentLevel);
        NextLevelText.text = String.Format("{0}", CurrentLevel + 1);

        for (int i = 0; i < CurrentPart - 1; i++)
            PartImages[i].color = Color.yellow;
    }

    public IEnumerator CheckGameOver()
    {
        yield return new WaitForSeconds(2.0f);

        if (LockPicker && !GameFailed)
        {
            Debug.Log("ÇALIŞTI");
            var currentObjectHolder = Level.ObjectHolders.Where(x => x.Key.PartNumber == CurrentPart).FirstOrDefault();

            var isSuccess = currentObjectHolder.Value.IsSuccess();

            if (isSuccess)
            {
                Debug.Log("Success" + CurrentPart);
                StartCoroutine(BridgeUp());
                CurrentPart++;
                StartCoroutine(DoorUp());
                Camera.main.transform.parent = null;
                Picker.localScale *= 1.2f;
                Camera.main.transform.parent = Picker;
                Picker.localPosition = new Vector3(Picker.localPosition.x, Picker.localPosition.y * 1.2f, Picker.localPosition.z);
                UpdateUI();
                LockPicker = false;
                Destroy(currentObjectHolder.Key.transform.Find("Objects").gameObject);
                PlayerPrefs.DeleteKey("LevelModel");

                var nextObjectHolder = Level.ObjectHolders.Where(x => x.Key.PartNumber == CurrentPart).FirstOrDefault();

                if (nextObjectHolder.Key != null && nextObjectHolder.Key.gameObject.GetComponentInChildren<ObjectSpawner>() != null)
                    DroneHasStart = true;

                if (CurrentPart == 4)
                {
                    GameSuccessPanel.SetActive(true);
                    LockPicker = true;
                    PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level") + 1);
                }
            }
            else
            {
                GameFailed = true;
                LockPicker = true;
                GameFailedPanel.SetActive(true);
            }
        }
    }

    IEnumerator BridgeUp()
    {
        var sector = Level.Parts[CurrentPart - 1].transform.Find("Sector");
        var bridge = Level.Parts[CurrentPart - 1].transform.Find("Bridge");

        while (Mathf.Abs(bridge.position.y - sector.position.y) > 0.01f)
        {
            bridge.transform.position = Vector3.MoveTowards(bridge.position, new Vector3(bridge.position.x, sector.position.y, bridge.position.z), Time.deltaTime * 10f);
            yield return null;
        }
    }

    IEnumerator DoorUp()
    {
        var door = Level.Parts[CurrentPart - 1].transform.Find("Door");
        var doorHolder = door.transform.GetChild(0);

        while (doorHolder.eulerAngles.z < 80f)
        {
            doorHolder.eulerAngles = Vector3.Lerp(doorHolder.eulerAngles, new Vector3(0f, 0f, 90f), Time.deltaTime);
            yield return null;
        }
    }

    public void OnRestartGameClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnNextLevelClicked()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnStartClicked()
    {
        GameStartPanel.SetActive(false);
        LockPicker = false;
    }
}
