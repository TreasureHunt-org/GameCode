using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backGroundMusic : MonoBehaviour
{
    public AudioSource audiosource;
    public AudioClip clip;
  
    void Start()
    {
        audiosource.clip = clip;
        audiosource.Play();
    }

    
    void Update()
    {
        
    }
}
