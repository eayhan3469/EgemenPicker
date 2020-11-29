using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private Transform Picker;

    public static GameManager Instance { get; private set; }

    public float PickerSpeed;
    public bool DroneHasStart;
    public bool LockPicker;
    public int LevelMultiplier;

    private int CurrentLevel;
    private int CurrentPart;
    private Level Level;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        PickerSpeed = 7f; //TODO: Level'a göre değişecek
        DroneHasStart = false;
        LockPicker = false;

        PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("Level"))
            PlayerPrefs.SetInt("Level", 1);

        CurrentPart = 1;
        CurrentLevel = PlayerPrefs.GetInt("Level");
        LevelMultiplier = (CurrentLevel / 10) + 1;
    }

    private void Start()
    {
        Level = FindObjectOfType<Level>();
    }

    private void Update()
    {
        if (Level.ObjectHolders.FirstOrDefault(x => x.Key.PartNumber == CurrentPart).Value.IsSuccess())
        {
            Debug.Log("Success");
            StartCoroutine(BridgeUp());
            CurrentPart++;
            StartCoroutine(DoorUp());
            Picker.localScale *= 1.2f;
            Picker.localPosition = new Vector3(Picker.localPosition.x, Picker.localPosition.y * 1.2f, Picker.localPosition.z);
        }

        if (CurrentPart == 3)
        {
            DroneHasStart = true;
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

        LockPicker = false;
    }
}
