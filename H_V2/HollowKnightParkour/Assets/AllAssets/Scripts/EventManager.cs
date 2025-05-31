using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public static Action OnNPCDestroyed;

    // Method to trigger the NPC destroyed event
    public static void TriggerNPCDestroyed()
    {
        OnNPCDestroyed?.Invoke();
    }
}
