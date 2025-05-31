using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrampolineBehavior : MonoBehaviour
{
    private float bouncee = 38f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Vector2 bounceDirection = new Vector2(transform.up.x, transform.up.y);
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bouncee, ForceMode2D.Impulse);
        }
    }

}
