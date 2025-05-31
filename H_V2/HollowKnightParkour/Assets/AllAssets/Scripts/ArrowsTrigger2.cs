using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsTrigger2 : MonoBehaviour
{
    public GameObject[] objectsToActivate; // The array of objects to activate
    public GameObject spikeSound;
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider that hit has the "Player" tag
        if (other.CompareTag("Player"))
        {
            spikeSound.SetActive(true);
            // Activate all objects and make them fall
            foreach (GameObject obj in objectsToActivate)
            {
                obj.SetActive(true); // Activate the object
                // Start making the object fall
                AbyssObjectsFalling fallingScript = obj.GetComponent<AbyssObjectsFalling>();
                if (fallingScript != null)
                {
                    fallingScript.StartFalling(); // Call the StartFalling method to start falling
                }
            }
        }
    }
}
