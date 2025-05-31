using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowKnives : MonoBehaviour
{
    public GameObject knifePrefab;
    public Transform spawnPoint;
    public float throwForce = 10f;
    public int knifeCount = 5; // Number of knives to spawn
    public float angleIncrement = 45f; // Difference in movement direction

    public void Throwsss()
    {
        float direction = transform.localScale.x > 0 ? 1f : -1f; // Check facing direction

        for (int i = 0; i < knifeCount; i++)
        {
            float angle = i * angleIncrement; // Calculate movement direction
            Vector2 forceDirection = new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle) * direction, Mathf.Sin(Mathf.Deg2Rad * angle));

            GameObject knife = Instantiate(knifePrefab, spawnPoint.position, Quaternion.identity); // Keep original rotation
            Rigidbody2D rb = knife.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.velocity = forceDirection * throwForce; // Moves knife in different directions
            }
        }
    }
}