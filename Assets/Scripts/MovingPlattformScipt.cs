using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlattformScipt : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private int startingPoint;
    [SerializeField] private Transform[] points;
    private int i;

    private void Start()
    {
        transform.position = points[startingPoint].position;
    }

    private void Update()
    {
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);
    }

    
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if (transform.position.y < collision.transform.position.y-0.2f)
            {
                collision.transform.SetParent(transform);
            }
        }

        
    }
    
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            collision.transform.SetParent(null);
        }
        
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        
        if (collision.collider.CompareTag("Player"))
        {
            var horizontal = collision.gameObject.GetComponent<CubeMovementTest>().Horizontal;
            if (horizontal == 0)
            {
                Debug.Log("Is still Platform" );
                collision.transform.SetParent(transform);    
            }

            
        }
    }
}
