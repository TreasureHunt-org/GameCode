using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseIcon : MonoBehaviour
{
    private void Update()
    {
        
    }
 
    public Texture2D customCursor; 
    public Vector2 hotspot = Vector2.zero; 

    void Start()
    {
        //Cursor.SetCursor(customCursor, hotspot, CursorMode.Auto);
    }
}
