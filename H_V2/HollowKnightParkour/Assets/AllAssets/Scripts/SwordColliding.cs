using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordColliding : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Sword"))
        {
            StickRotating stick = GetComponent<StickRotating>();
            if (stick != null)
            {
                stick.RotateStick();
            }
            AlphabetStickRotatingAndLogic stick2 = GetComponent<AlphabetStickRotatingAndLogic>();
            if (stick2 != null)
            {
                stick2.RotateStick();
            }
            EnterKey stick3 = GetComponent<EnterKey>();
            if(stick3 != null)
            {
                stick3.RotateStick();
            }
        }
    }

}
