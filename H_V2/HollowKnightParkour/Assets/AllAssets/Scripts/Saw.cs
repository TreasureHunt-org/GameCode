using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Saw : MonoBehaviour
{
    [SerializeField] private float moveSpeed;

    private int pointsIndex;

    public Transform[] Points;


    void Start()
    {
        transform.position = Points[pointsIndex].transform.position;
    }

 
    void Update()
    {
        if (pointsIndex <= Points.Length - 1)
        {
            transform.position = Vector2.MoveTowards(transform.position, Points[pointsIndex].transform.position, moveSpeed * Time.deltaTime);

            if (transform.position == Points[pointsIndex].transform.position)
            {
                pointsIndex += 1;
            }

            if (pointsIndex == Points.Length)
            {
                pointsIndex = 0;
            }
        }
    }
}
