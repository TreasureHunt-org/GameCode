using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawning : MonoBehaviour
{
    void Start()
    {
        if (TestingMovement.instance != null)
        {
            TestingMovement.instance.transform.position = transform.position;
        }
    }
}
