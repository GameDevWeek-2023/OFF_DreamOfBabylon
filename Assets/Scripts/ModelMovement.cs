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
    private BoxCollider2D bC;

    [SerializeField] private float jumpForce;
    private bool justJumped = false;

    private bool onGround = false;

    private void Awake()
    {
        RB = GetComponent<Rigidbody2D>();
        bC = GetComponent<BoxCollider2D>();
    }

    void Start()
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
        RB.velocity = new Vector2(movementDirection * runMaxSpeed, RB.velocity.y);
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
}
