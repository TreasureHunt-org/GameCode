using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DownAndUpArrows : MonoBehaviour
{
    public float moveDistance = 2f;
    public float moveTime = 1f; 
    private Vector3 startPos;
    private bool movingRight = true;

    void Start()
    {
        startPos = transform.position;
        StartCoroutine(MoveArrows());
    }

    IEnumerator MoveArrows()
    {
        while (true)
        {
            Vector3 targetPos = startPos + (movingRight ? Vector3.left * moveDistance : Vector3.right * moveDistance);
            float elapsedTime = 0f;

            while (elapsedTime < moveTime)
            {
                transform.position = Vector3.Lerp(transform.position, targetPos, elapsedTime / moveTime);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.position = targetPos;
            movingRight = !movingRight; 
            yield return new WaitForSeconds(0f);
        }
    }
}
