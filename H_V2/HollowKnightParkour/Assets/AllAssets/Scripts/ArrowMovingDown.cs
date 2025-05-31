using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovingDown : MonoBehaviour
{
    public float moveSpeed = 5f;  // Speed at which the arrow moves

    private bool isMoving = false;

    private void Update()
    {
        if (isMoving)
        {
            // Move the arrow upwards (convert Vector2 to Vector3)
            transform.position += new Vector3(0f, Vector2.down.y, 0f) * moveSpeed * Time.deltaTime;
        }
    }

    // Method to start the movement
    public void StartMoving()
    {
        isMoving = true;
    }
}



