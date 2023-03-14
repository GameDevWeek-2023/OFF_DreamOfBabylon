using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModelMovementTest : MonoBehaviour
{
    [SerializeField]private float runMaxSpeed;

    private float runAcceleration;

    private float runAccelAmount;

    private float runDecceleration;

    private float runDeccelAmount;

    private float movementDirection;
    
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
    private void OnValidate()
    {
        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        #endregion
    }

    private void FixedUpdate()
    {
        Run();
    }

    private void OnMovement(InputAction.CallbackContext context)
    {
        movementDirection = context.ReadValue<Vector2>().x;
    }

    private void Run()
    {
        float targetSpeed = movementDirection * runMaxSpeed;
        float accelRate;
        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
        float speedDif = targetSpeed - RB.velocity.x;
        float movement = speedDif * accelRate;
        RB.AddForce(movement * Vector2.right, ForceMode2D.Force);
    }
}
