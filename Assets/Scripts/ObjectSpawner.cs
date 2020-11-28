using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour
{
    [SerializeField]
    private float MoveSpeed = 5f;

    [SerializeField]
    private float Frequency = 5f;

    [SerializeField]
    private float Magnitude = 5f;

    [SerializeField]
    private int ObjectCount = 10;

    [SerializeField]
    private float SpawnFrequency = 2f;

    [SerializeField]
    private GameObject Object;

    [SerializeField]
    private Transform ObjectsParent;

    private Vector3 position;
    private bool spawnHasStart;

    private void Start()
    {
        position = transform.position;
        spawnHasStart = false;
    }

    //private void SpawnObjects()
    //{
    //    spawnHasStart = true;

    //    for (int i = 0; i < ObjectCount; i++)
    //    {
    //        Instantiate(Object, position, Quaternion.identity, ObjectsParent);
    //        Task.Run(SpawnObjectsAsync).Wait();
    //    }
    //}

    //private async Task SpawnObjectsAsync()
    //{
    //    await Task.Delay(100);
    //    await Task.CompletedTask;
    //}

    //void Update()
    //{
    //    if (GameManager.Instance.DroneHasStart)
    //    {
    //        if (!spawnHasStart)
    //            SpawnObjects();

    //        position += transform.forward * Time.deltaTime * MoveSpeed;
    //        transform.position = position + transform.right * Mathf.Sin(Time.time * Frequency) * Magnitude;
    //    }
    //}
}
