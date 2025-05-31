using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbyssObjectsFalling : MonoBehaviour
{
    public float fallSpeed = 5f; // Speed at which the object falls
    private bool isFalling = false; // To check if the object should start falling

    void Update()
    {
        // If the object is set to fall, move it downwards
        if (isFalling)
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

    // Call this method to start falling
    public void StartFalling()
    {
        isFalling = true;
    }
}
