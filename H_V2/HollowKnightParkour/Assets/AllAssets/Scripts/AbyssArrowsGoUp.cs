using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbyssArrowsGoUp : MonoBehaviour
{
    public float moveSpeed = 5f; 
    public float moveDistance = 5f; 
    private Vector3 startingPosition; 
    private bool isActivated = false; 
    public AudioSource audio;
    public AudioClip clip;

    void Start()
    {
        audio.clip = clip;
        startingPosition = transform.position;
    }

    void Update()
    {
        if (isActivated)
        {
            
            float distanceTraveled = Vector3.Distance(startingPosition, transform.position);

            if (distanceTraveled < moveDistance)
            {
                transform.position += Vector3.up * moveSpeed * Time.deltaTime;
            }
            else
            {
                
                isActivated = false;
            }
        }
    }

   
    public void ActivateArrow()
    {
        if(!isActivated)
        audio.PlayOneShot(clip);
        isActivated = true; 
    }
}
