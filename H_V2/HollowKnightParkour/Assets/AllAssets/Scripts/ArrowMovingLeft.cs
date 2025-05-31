using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowMovingLeft : MonoBehaviour
{

    public float moveSpeed = 5f;  // Speed at which the arrow moves

    private bool isMoving = false;

    private void Update()
    {
        if (isMoving)
        {
            // Move the arrow to the left (convert Vector2 to Vector3)
            transform.position += new Vector3(Vector2.left.x, 0f, 0f) * moveSpeed * Time.deltaTime;
        }
    }
    // Method to start the movement
    public void StartMoving()
    {
        isMoving = true;
    }
}
