using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJumpTrigger : MonoBehaviour
{
    public DoubleJumpAbility dju;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("dialouge triggered");
        }
    }
}
