using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public float PickerSpeed;
    public bool DroneHasStart;
    public bool LockPicker;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        PickerSpeed = 5f; //TODO: Level'a göre değişecek
        DroneHasStart = false;
        LockPicker = false;
    }
    
    void Update()
    {
        
    }
}
