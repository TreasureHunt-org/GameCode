using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsSlightlyAbove : MonoBehaviour
{
    public float moveSpeed = 1f;      // Speed of upward movement
    public float moveDistance = 1f;   // How far the arrow should move upwards
    private Vector3 startPosition;    // Starting position of the arrow
    private bool isMoving = false;    // Flag to check if the arrow is moving

    void Start()
    {
        // Store the starting position of the arrow
        startPosition = transform.position;
    }

    void Update()
    {
        // Move the arrow upward if it's in the moving state
        if (isMoving && transform.position.y < startPosition.y + moveDistance)
        {
            transform.position += Vector3.up * moveSpeed * Time.deltaTime;
        }
    }

    // Method to start moving the arrow upwards
    public void StartMoving()
    {
        if (!isMoving)
        {
            isMoving = true;  // Set flag to true so movement can begin
        }
    }

}
