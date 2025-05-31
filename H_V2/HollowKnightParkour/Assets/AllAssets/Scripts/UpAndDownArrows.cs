using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ArrowMovement : MonoBehaviour
{
    public float moveDistance = 2f; // Distance the arrows move left and right
    public float moveTime = 1f; // Time for each movement
    public AudioSource audioo;
    public AudioClip clip;
    private Vector3 startPos;
    private bool movingRight = true;
    public ArrowsTrigger2 trigger;
    void Start()
    {
        startPos = transform.position;
        StartCoroutine(MoveArrows());
   
        audioo.clip = clip;
    }

    IEnumerator MoveArrows()
    {
        while (true)
        {
            Vector3 targetPos = startPos + (movingRight ? Vector3.right * moveDistance : Vector3.left * moveDistance);
            float elapsedTime = 0f;
            if (trigger.spikeSound.gameObject.activeInHierarchy == true)
                audioo.PlayOneShot(clip);

            while (elapsedTime < moveTime)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;
            movingRight = !movingRight; // Switch direction
            yield return new WaitForSeconds(0f); 
        }
    }
}


