using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingThnigie : MonoBehaviour
{ 
    public Vector3 rotationSpeed = new Vector3(0f, 0f, 20f);
    public float fallSpeed = 5f;
    void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime, Space.Self);

        // Move straight down in world space
        transform.Translate(Vector3.down * fallSpeed * Time.deltaTime, Space.World);
    }
}
