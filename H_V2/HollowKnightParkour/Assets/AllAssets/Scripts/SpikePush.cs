using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePush : MonoBehaviour
{
    public float bounceForce = 10f; 
    public TestingMovement testingMovement;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Case 1: The sword hits the spike
        if (collision.CompareTag("Sword"))
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); 
            if (playerObj != null)
            {
                Rigidbody2D rb = playerObj.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    testingMovement.audioSource.PlayOneShot(testingMovement.SpikeHit);
                    testingMovement.canDashInAir = true;
                    testingMovement.howManyJumping = 0;
                    rb.velocity = new Vector2(rb.velocity.x, bounceForce); // Apply bounce force
                }
            }
        }
        else if (collision.CompareTag("Player"))
        {
            Debug.Log("player dies");
        }
    }
}
