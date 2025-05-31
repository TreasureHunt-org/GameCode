using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw2 : MonoBehaviour
{
    public float speed = 3f; 
    private int direction = -1; 
    void Update()
    {
        transform.Translate(Vector2.right * speed * direction * Time.deltaTime);

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Railway"))
        {
            direction *= -1;
        }
    }
}
