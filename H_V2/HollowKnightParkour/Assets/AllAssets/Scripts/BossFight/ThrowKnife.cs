using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnife : MonoBehaviour
{
    public GameObject knifePrefab;
    public Transform spawnPoint;
    public float throwForce = 10f;

   

    public void Throw()
    {
        float direction = transform.localScale.x > 0 ? -1f : 1f; // Check facing direction
        GameObject knife = Instantiate(knifePrefab, spawnPoint.position, spawnPoint.rotation);
        Rigidbody2D rb = knife.GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.velocity = new Vector2(direction * throwForce, 0f); // Moves knife left or right
        }
    }
}
