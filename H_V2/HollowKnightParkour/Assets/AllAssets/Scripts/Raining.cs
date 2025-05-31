using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raining : MonoBehaviour
{
    public GameObject objectPrefab;
    public float spawnInterval = 5f;

    void Start()
    {
        InvokeRepeating(nameof(SpawnObject), 0f, spawnInterval);
    }

    void SpawnObject()
    {
        Instantiate(objectPrefab, transform.position, Quaternion.identity);
    }
}
