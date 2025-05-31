using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class AlphabetStickRotatingAndLogic : MonoBehaviour
{
    public float moveDistance = -2f;  // Distance to move on X-axis
    public float rotationAngle = 90f; // Rotation angle on Z-axis
    public float duration = 10f; // Time in seconds to complete movement
    public float returnDelay = 2f; // Delay before returning to original position

    private bool isMoving = false; // Prevent multiple hits at once
    private Vector3 startPosition;
    private Quaternion startRotation;

    public AudioSource audio;
    public AudioClip clip;

    public char assignedLetter;

    private void Start()
    {
        startPosition = transform.position;
        startRotation = transform.rotation;
    }

    public void RotateStick()
    {
        if (!isMoving) // Only trigger if it's not already moving
        {
            StartCoroutine(RotateAndMove());
            Challenge.Instance.AddLetter(assignedLetter);
        }
    }

    IEnumerator RotateAndMove()
    {
        audio.PlayOneShot(clip);
        isMoving = true; // Prevent multiple hits

        Vector3 targetPos = startPosition + new Vector3(-moveDistance, 0, 0); // Move 2 units on X-axis
        Quaternion targetRotation = Quaternion.Euler(0, 0, -rotationAngle); // Rotate on Z

      

        // Move stick to target position
        yield return MoveAndRotate(startPosition, targetPos, startRotation, targetRotation, duration);

        // Wait before returning
        yield return new WaitForSeconds(returnDelay);

   

        // Move stick back to original position
        yield return MoveAndRotate(targetPos, startPosition, targetRotation, startRotation, duration);

        isMoving = false; // Allow the player to hit it again
    }

    IEnumerator MoveAndRotate(Vector3 startPos, Vector3 endPos, Quaternion startRot, Quaternion endRot, float time)
    {
        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            float t = elapsedTime / time; // Normalize time (0 to 1)
            transform.position = Vector3.Lerp(startPos, endPos, t);
            transform.rotation = Quaternion.Lerp(startRot, endRot, t);

            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }

        // Ensure final values are set correctly
        transform.position = endPos;
        transform.rotation = endRot;
    }

    
}
