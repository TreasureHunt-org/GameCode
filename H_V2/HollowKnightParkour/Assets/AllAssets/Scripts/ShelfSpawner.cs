using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class ShelfSpawner : MonoBehaviour
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
