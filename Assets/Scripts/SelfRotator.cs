using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfRotator : MonoBehaviour
{
    [SerializeField]
    private float Speed = 100;

    void Update()
    {
        transform.Rotate(0, 0, Speed * Time.deltaTime);
    }
}
