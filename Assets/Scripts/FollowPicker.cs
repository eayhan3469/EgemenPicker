using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPicker : MonoBehaviour
{
    [SerializeField]
    private Transform Picker;

    [SerializeField]
    private Vector3 Offset;

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, Picker.transform.position + Offset, 10f);
    }
}
