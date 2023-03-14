using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

public class ModelMovement : MonoBehaviour
{
    
    [SerializeField]private float runSpeed; //Target speed we want the player to reach.
    [SerializeField] private float dashSpeed = 25f;
    [SerializeField] private float dashDuration = 0.5f;
    private float dashTimer;
    private float movementDirection;

    private Rigidbody2D RB;
    private float gravity; 


    [SerializeField] private float jumpForce;
    //private bool justJumped = false;
    private float timeSinceDash;
    private bool onGround = false;
    private bool dashing=false;
    private float lookDirection = 1f;
    private void Awake()
    {
        
        dashTimer = dashDuration;
    }

    void Start()
    {
        RB = GetComponent<Rigidbody2D>();
        gravity = RB.gravityScale;
    }

    private void FixedUpdate()
    {
        if (!dashing)
        {
            Run();   
        }
        else
        {
            dashTimer -= Time.fixedDeltaTime;
            Debug.Log(dashTimer);
            if (dashTimer <= 0)
            {
                dashing = false;
                RB.gravityScale = gravity;
                dashTimer = dashDuration;
            }
        }

    }

    void OnMovement(InputValue value)
    {
        movementDirection = value.Get<Vector2>().x;
        if (movementDirection != 0)
        {
            lookDirection = movementDirection;
        }
    }

    private void Run()
    {
        RB.velocity = new Vector2(movementDirection * runSpeed, RB.velocity.y);
        //RB.AddForce(new Vector2(movementDirection, 0) * runMaxSpeed);
    }
    


    void OnJump(InputValue value)
    {
        if (onGround && (value.Get<float>() > 0))
        {
            RB.AddForce(Vector2.up*jumpForce,ForceMode2D.Impulse);
            onGround = false;
        }
    }
    
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "ground")
        {
            onGround = true;
            //kyoteTimer = 0;
        }
    }

    void OnDash()
    {
        if (dashing)
            return;
        dashing = true;
        RB.velocity = new Vector2(lookDirection * dashSpeed, 0);
        RB.gravityScale = 0;
    }
}
