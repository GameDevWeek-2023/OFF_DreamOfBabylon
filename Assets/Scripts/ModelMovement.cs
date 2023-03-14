using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ModelMovement : MonoBehaviour
{
    
    [SerializeField]private float runSpeed;
    private Rigidbody2D RB;
    private float gravity; 
    private float movementDirection;
    private float lookDirection = 1f;
    
    [SerializeField] private float dashSpeed = 25f;
    [SerializeField] private float dashDuration = 0.5f;
    private float dashTimer;
    private float timeSinceDash;
    private bool dashing=false;

    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpForceDown = -15f;
    [SerializeField] private float maxYVelocity;
    private bool onGround = false;

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
        if ((RB.velocity.y < 0 && RB.gravityScale>0) || (RB.velocity.y > 0 && RB.gravityScale < 0))
        {
            if (RB.velocity.y < 0)
            {
                RB.AddForce(new Vector2(0, jumpForceDown));
            }
            else
            {
                RB.AddForce(new Vector2(0, jumpForceDown*(-1)));
            }
        }

        RB.velocity = new Vector2(RB.velocity.x, Mathf.Clamp(RB.velocity.y, -maxYVelocity, maxYVelocity));
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

    void OnGravity()
    {
        gameObject.transform.Rotate(Vector3.forward, 180);
        //gameObject.transform.SetPositionAndRotation(gameObject.transform.position,Quaternion.Euler(0,0,180));
        RB.gravityScale *= -1;
    }
}
