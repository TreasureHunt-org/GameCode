using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trampolineBehavior2 : MonoBehaviour
{
    public class Trampoline : MonoBehaviour
    {
        public Vector2 jumpDirection = Vector2.up; // Default direction is up
        public float jumpForce = 20f;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                Rigidbody2D playerRigidbody = collision.gameObject.GetComponent<Rigidbody2D>();
                Vector2 force = jumpDirection.normalized * jumpForce;
                playerRigidbody.velocity = Vector2.zero; // Reset velocity before applying force
                playerRigidbody.AddForce(force, ForceMode2D.Impulse);
            }
        }
    }

}
