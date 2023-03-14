using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModelMovementTest : MonoBehaviour
{
    private float runMaxSpeed;

    private float runAcceleration;

    private float runAccelAmount;

    private float runDecceleration;

    private float runDeccelAmount;

    private Vector2 movementDirection;
    
    private Rigidbody2D RB { get; set; }

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

    private void OnMovement(InputAction.CallbackContext context)
    {
        movementDirection = context;
    }
}
