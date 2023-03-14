using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModelMovement : MonoBehaviour
{
    
    [SerializeField]private float runMaxSpeed; //Target speed we want the player to reach.
    private float movementDirection;

    private Rigidbody2D RB;
    
    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void FixedUpdate()
    {
        Run();
    }

    void OnMovement(InputValue value)
    {
        movementDirection = value.Get<Vector2>().x;
    }

    private void Run()
    {
        RB.AddForce(new Vector2(movementDirection, 0) * runMaxSpeed);
    }
    

    void OnJump(InputValue value)
    {
        
    }
}
