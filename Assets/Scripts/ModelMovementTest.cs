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

    private void OnMovement(InputValue value)
    {
        movementDirection = value.Get<Vector2>().x;
    }

    private void Run()
    {
        RB.AddForce(new Vector2(movementDirection, 0) * runMaxSpeed);
    }
}
