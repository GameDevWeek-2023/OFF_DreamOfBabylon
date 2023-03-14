using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModelMovement : MonoBehaviour
{
    
    [SerializeField]private float runMaxSpeed; //Target speed we want the player to reach.
    private float movementDirection;

    private Rigidbody2D RB;

    [SerializeField] private float jumpForce;
    private bool justJumped = false;

    private bool onGround;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
    }

    private bool isGrounded()
    {
        RaycastHit2D hit = Physics2D.Raycast(gameObject.transform.position, Vector2.down, 0.2f, 3);
        if (hit.collider != null)
        {
            onGround = true;
        }
        else
        {
            onGround = false;
        }

        return onGround;
    }

    private void FixedUpdate()
    {
        Run();
        Debug.DrawRay(gameObject.transform.position, Vector2.down, Color.blue);
        
    }

    void OnMovement(InputValue value)
    {
        movementDirection = value.Get<Vector2>().x;
    }

    private void Run()
    {
        RB.velocity = new Vector2(movementDirection * runMaxSpeed, RB.velocity.y);
        //RB.AddForce(new Vector2(movementDirection, 0) * runMaxSpeed);
    }
    


    void OnJump(InputValue value)
    {
        if (isGrounded())
        {
            RB.AddForce(Vector2.up*jumpForce,ForceMode2D.Impulse);
        }
    }
}
