using System.Collections;
using UnityEngine;

public class StickRotating : MonoBehaviour
{
    public float moveDistance = -2f;  // Distance to move on X-axis
    public float rotationAngle = 90f; // Rotation angle on Z-axis
    public float duration = 10f; // Time in seconds to complete movement
    public float returnDelay = 2f; // Delay before returning to original position

    private bool isMoving = false; // Prevent multiple hits at once
    private Vector3 startPosition;
    private Quaternion startRotation;

    public AudioSource audio;
    public AudioClip clip, clip2;

    public GameObject door; // Reference to the door
    public Vector3 doorOpenPosition; // Position when the door is open
    private Vector3 doorClosedPosition; // Original door position

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;

        if (door != null)
        {
            doorClosedPosition = door.transform.position;
        }
    }

    public void RotateStick()
    {
        if (!isMoving) // Only trigger if it's not already moving
        {
            StartCoroutine(RotateAndMove());
        }
    }

    IEnumerator RotateAndMove()
    {
        audio.PlayOneShot(clip);
        isMoving = true; // Prevent multiple hits

        Vector3 targetPos = startPosition + new Vector3(-moveDistance, 0, 0); // Move 2 units on X-axis
        Quaternion targetRotation = Quaternion.Euler(0, 0, -rotationAngle); // Rotate on Z

        // Start moving the door at the same time as the stick
        if (door != null)
            StartCoroutine(MoveDoor(doorClosedPosition, doorOpenPosition, duration));

        // Move stick to target position
        yield return MoveAndRotate(startPosition, targetPos, startRotation, targetRotation, duration);

        // Wait before returning
        yield return new WaitForSeconds(returnDelay);
        audio.PlayOneShot(clip2);

        // Start closing the door at the same time as the stick moves back
        if (door != null)
            StartCoroutine(MoveDoor(doorOpenPosition, doorClosedPosition, duration));

        // Move stick back to original position
        yield return MoveAndRotate(targetPos, startPosition, targetRotation, startRotation, duration);

        isMoving = false; // Allow the player to hit it again
    }

    IEnumerator MoveAndRotate(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            float t = elapsedTime / time; 
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

      
        transform.position = endPos;
        transform.rotation = endRot;
    }

    IEnumerator MoveDoor(Vector3 startPos, Vector3 endPos, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            float t = elapsedTime / time;
            door.transform.position = Vector3.Lerp(startPos, endPos, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        door.transform.position = endPos;
    }
}
