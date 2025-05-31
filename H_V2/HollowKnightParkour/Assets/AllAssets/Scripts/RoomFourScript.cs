using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomFourScript : MonoBehaviour
{

    public Transform leftWall;
    public Transform rightWall;
    public Vector3 leftWallTarget;
    public Vector3 rightWallTarget;
    public float moveSpeed = 0.5f;
    public AudioSource wallDropSound;
    public AudioClip clip;

    private bool isActivated = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isActivated && other.CompareTag("Player"))
        {
            isActivated = true;
            StartCoroutine(MoveWalls());
        }
    }

    private System.Collections.IEnumerator MoveWalls()
    {
        Vector3 leftStart = leftWall.position;
        Vector3 rightStart = rightWall.position;

        float time = 0f;

        wallDropSound.PlayOneShot(clip);
        while (time < 1f)
        {
            time += Time.deltaTime * moveSpeed;
            leftWall.localPosition = Vector3.Lerp(leftStart, leftWallTarget, time);
            rightWall.localPosition = Vector3.Lerp(rightStart, rightWallTarget, time);

            yield return null;
        }

            leftWall.position = leftWallTarget;
        rightWall.position = rightWallTarget;

    }
}


