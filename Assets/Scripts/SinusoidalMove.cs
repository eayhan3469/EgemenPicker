using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinusoidalMove : GameManager
{
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float frequency = 5f;

    [SerializeField]
    private float magnitude = 5f;

    private Vector3 position;

    private void Start()
    {
        position = transform.position;
    }

    void Update()
    {
        if (DroneHasStart)
        {
            position += transform.forward * Time.deltaTime * moveSpeed;
            transform.position = position + transform.right * Mathf.Sin(Time.time * frequency) * magnitude;
        }
    }
}
