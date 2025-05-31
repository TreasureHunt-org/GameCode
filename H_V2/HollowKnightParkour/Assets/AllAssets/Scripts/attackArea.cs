using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackArea : MonoBehaviour
{

    void Start()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Railway"))
        {
            Debug.Log("sad");
        }
    }
 
    void Update()
    {
        
    }
}
