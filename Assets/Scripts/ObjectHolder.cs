using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectHolder : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI CounterText;

    public int NumberOfObjectForPass { get; set; }

    private int CatchedObjectCount;

    private void Start()
    {
        UpdateTexts();
    }

    private void Update()
    {
        UpdateTexts();
    }

    private void UpdateTexts()
    {
        CounterText.text = String.Format("{0}/{1}", CatchedObjectCount, NumberOfObjectForPass);
    }

    public bool IsSuccess()
    {
        return CatchedObjectCount >= NumberOfObjectForPass;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Object")
        {
            CatchedObjectCount++;
            UpdateTexts();
        }
    }
}
