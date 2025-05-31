using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomThree : MonoBehaviour
{
    public GameObject[] arrows; 
    public float delayBetweenArrows = 0.1f; 
    public AudioSource audio;
    public AudioClip clip;
    private bool hasActivated = false; 

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !hasActivated) 
        {
            hasActivated = true; // Mark as activated
            StartCoroutine(ActivateArrowsOneByOne());
        }
    }

    IEnumerator ActivateArrowsOneByOne()
    {
        // Play sound once when triggered

        foreach (GameObject arrow in arrows)
        {
            arrow.SetActive(true);
            Rigidbody2D rb = arrow.GetComponent<Rigidbody2D>();
            audio.PlayOneShot(clip);
            if (rb != null)
            {
                rb.gravityScale = 20; 
            }

            yield return new WaitForSeconds(delayBetweenArrows); 
        }

        GetComponent<Collider2D>().enabled = false; 
    }
}
