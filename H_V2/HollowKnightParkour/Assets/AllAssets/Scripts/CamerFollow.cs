using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerFollow : MonoBehaviour
{



    public CinemachineVirtualCamera virtualCamera; 

    void Start()
    {
        // Check if the PlayerManager instance exists and the virtual camera is set up
        if (virtualCamera != null && TestingMovement.instance != null)
        {
            // Set the virtual camera to follow the player from the PlayerManager instance
            virtualCamera.Follow = TestingMovement.instance.transform;
        }
    }
}


