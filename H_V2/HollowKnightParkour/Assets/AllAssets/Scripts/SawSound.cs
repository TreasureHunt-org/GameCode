using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawSound : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip clip;
   
    void Start()
    {
        audioSource.clip = clip;
    }


    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")){
            audioSource.Play();
        }
    }
}
