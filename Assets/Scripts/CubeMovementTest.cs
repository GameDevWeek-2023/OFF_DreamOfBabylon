using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeMovementTest : MonoBehaviour
{
    private float horizontal;

    [SerializeField]private float speed = 8f;
    
    [SerializeField]private float jumpPower = 16f;

    private bool isFacingRight = true;

    private bool jumping = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    [SerializeField]private float canceledJumpMultiplier = 0.5f;

    private bool canDash = true;
    private bool isDashing;
    [SerializeField]private float dashingPower = 24f;
    [SerializeField]private float dashingTime = 0.2f;
    [SerializeField]private float dashingCooldown = 1f;
    List<Collider2D> inColliders = new List<Collider2D>();
    
    //LadderMovementVariables
    private float vertical;
    private float ladderSpeed;
    private bool isLadder;
    private bool isClimbing;
    private float gravity;
    private DragonBones.UnityArmatureComponent armatureComponent;

    private float coyoteTime = 0.05f;
    private float coyoteTimeCounter;

        // Start is called before the first frame update
    void Start()
    {
        gravity = rb.gravityScale;
        armatureComponent = GetComponentInChildren<DragonBones.UnityArmatureComponent>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }
        
        if (isDashing)
        {
            return;
        }
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if(rb.velocity.x == 0f && IsGrounded()&& armatureComponent.animation.lastAnimationName != "Idle")
        {
            armatureComponent.animation.Play("Idle");
            armatureComponent.animation.timeScale = 1;
        }else if(rb.velocity.x != 0f && IsGrounded() && armatureComponent.animation.lastAnimationName != "Run")
        {
            armatureComponent.animation.Play("Run");
            armatureComponent.animation.timeScale = 1.5f;
        }
        if (!isFacingRight && horizontal > 0f)
        {
            Flip();
        }else if (isFacingRight && horizontal < 0f)
        {
            Flip();
        }
        if (isLadder && Mathf.Abs(vertical) >= 0f)
        {
            isClimbing = true;
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }

        if (isClimbing)
        {
            rb.gravityScale = 0f;
            rb.velocity = new Vector2(rb.velocity.x, vertical * speed);
        }
        else
        {
            rb.gravityScale = gravity;
        }
        
    }

    void OnLadderUpDown(InputValue value)
    {
        vertical = value.Get<float>();
    }

    void OnMovement(InputValue value)
    {
        horizontal = value.Get<Vector2>().x;
    }

    /*public void Movement(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }*/

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    void OnJump(InputValue value)
    {
        if (isDashing)
        {
            return;
        }
        if (value.Get<float>() > 0 && coyoteTimeCounter>0)//IsGrounded()
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if (value.Get<float>() <= 0 && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * canceledJumpMultiplier);
            coyoteTimeCounter = 0f;
        }
        armatureComponent.animation.Play("Jump", 1);
        armatureComponent.animation.timeScale = 2;
    }

    /*public void Jump(InputAction.CallbackContext context)
    {
        if (isDashing)
        {
            return;
        }

        if (context.performed && IsGrounded())
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        }

        if (context.canceled && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * canceledJumpMultiplier);
        }
    }*/

    void OnDashing(InputValue value)
    {
        if (canDash)
        {
            StartCoroutine(Dash());
        }
    }

    /*public void Dashing(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            Debug.Log(context.performed);
            StartCoroutine(Dash());
        }

        
    }*/

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        armatureComponent.animation.Play("Dash", 1);
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void OnInteract(InputValue value)
    {
        inColliders.ForEach(n => n.SendMessage("Use", SendMessageOptions.DontRequireReceiver));
    }

    /*public void Interact(InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            inColliders.ForEach(n => n.SendMessage("Use", SendMessageOptions.DontRequireReceiver));
        }
    }*/

    private void OnTriggerEnter2D(Collider2D collision)
    {
        inColliders.Add(collision);
        if (collision.CompareTag("Ladder"))
        {
            isLadder = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        inColliders.Remove(collision);
        if (collision.CompareTag("Ladder"))
        {
            isLadder = false;
            isClimbing = false;
        }
    }
}
