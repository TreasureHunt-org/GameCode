using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sword : MonoBehaviour
{
    public float bounceForce = 10f; 
    public LayerMask spikeLayer; 

    private Rigidbody2D rb;
    private bool isAttackingDown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        Debug.Log("f");
        if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.X))
        {
            Debug.Log("he's hittin");
            isAttackingDown = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isAttackingDown && collision.CompareTag("Spikes"))
        {
            Debug.Log("he's hitting a spike");
            rb.velocity = new Vector2(rb.velocity.x, bounceForce); // Apply bounce
            isAttackingDown = false; // Reset attack
        }
    }
}
