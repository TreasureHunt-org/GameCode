using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingTrigger : MonoBehaviour
{
    public JumpingAbility ju;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("dialouge triggered");
            ju.startDialouge();
        }
    }
}
