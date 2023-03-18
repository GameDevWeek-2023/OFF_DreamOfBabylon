using System;
using System.Collections;
using System.Collections.Generic;
// using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeMovementTest : MonoBehaviour, IResetable
{
    private float horizontal;

    [SerializeField]private float speed = 8f;
    
    [SerializeField]private float jumpPower = 16f;

    private bool isFacingRight = true;

    private bool jumping = false;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private LayerMask grabObjectLayer;
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
    private bool pauseInputs = false;
    private float gravity;
    private DragonBones.UnityArmatureComponent armatureComponent;
    private List<IResetable> resetList;

    [SerializeField]private float coyoteTime = 0.05f;
    private float coyoteTimeCounter;
    //private AudioManager _audioManager;

    public float Horizontal { get => horizontal; set => horizontal = value; }

    // Start is called before the first frame update

    private void Awake()
    {
        pauseInputs = false;
        
    }
    void Start()
    {
        //Time.timeScale = 0.3f;
        gravity = rb.gravityScale;
        armatureComponent = GetComponentInChildren<DragonBones.UnityArmatureComponent>();
        resetList = new List<IResetable>();
        var compenentsInScene = FindObjectsOfType<Component>();
        foreach(var component in compenentsInScene)
        {
            var componentInterface = component.GetComponent<IResetable>();
            if(componentInterface!=null)
            {
                resetList.Add(componentInterface);
            }
        }
        //resetList = FindObjectsByType<IResetable>();
        //_audioManager = FindObjectOfType<AudioManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsGrounded() || IsOnBox())
        {
            coyoteTimeCounter = coyoteTime;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
            AudioManager.instance?.Stop("Footsteps");
        }
        
        if (isDashing || pauseInputs)
        {
            return;
        }
        if (IsGrounded() && armatureComponent.animation.lastAnimationName == "Jump_Down_loop")
        {
            AudioManager.instance?.Play("Landing");
            if (rb.velocity.x != 0)
            {
                AudioManager.instance?.Play("Footsteps");
            }
        }
        if(rb.velocity.x == 0f && (IsGrounded() || IsOnBox())&& armatureComponent.animation.lastAnimationName != "Idle")
        {
            armatureComponent.animation.Play("Idle");
            armatureComponent.animation.timeScale = 0.8f;
        }else if(rb.velocity.x != 0f && (IsGrounded() || IsOnBox()) && armatureComponent.animation.lastAnimationName != "Run")
        {
            armatureComponent.animation.Play("Run");
            armatureComponent.animation.timeScale = 1.5f;
        }else if(rb.velocity.y<0 && !(IsGrounded() || IsOnBox()) && armatureComponent.animation.lastAnimationName != "Jump_Down_loop")
        {
            armatureComponent.animation.Play("Jump_Down_loop");
            //armatureComponent.animation.timeScale = 1;
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
        if (isDashing || pauseInputs)
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
        
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);

        
        
    }

    void OnLadderUpDown(InputValue value)
    {
        if(pauseInputs)
        {
            return;
        }
        vertical = value.Get<float>();
    }

    void OnMovement(InputValue value)
    {
        if(pauseInputs)
        {
            return;
        }
        horizontal = value.Get<Vector2>().x;
        if (transform.parent != null && transform.parent.CompareTag("Plattform") && horizontal != 0 )
        {
            transform.SetParent(null);
        }

        if (((IsGrounded() || IsOnBox())&& value.Get<Vector2>().x == 0) || !(IsGrounded() || IsOnBox()))
        {
            AudioManager.instance?.Stop("Footsteps");
        } else if ((IsGrounded() || IsOnBox()) && value.Get<Vector2>().x != 0)
        {
            AudioManager.instance?.Play("Footsteps"); 
        }

        
        
    }

    void OnReset(InputValue value)
    {
        if(pauseInputs)
        {
            return;
        }
        foreach(IResetable r in resetList)
        {
            r.Reset();
        }
    }

    /*public void Movement(InputAction.CallbackContext context)
    {
        horizontal = context.ReadValue<Vector2>().x;
    }*/

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
    }
    
    private bool IsOnBox()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, grabObjectLayer);
    }

    private void Flip()
    {
        if(pauseInputs)
        {
            return;
        }
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
        if (isDashing || pauseInputs)
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
        armatureComponent.animation.Play("Jump_up", 1);
        armatureComponent.animation.timeScale = 0.7f;
    }
    
    public void PauseInputs(bool pause)
    {
        pauseInputs = pause;
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
        if (canDash && !pauseInputs)
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
        AudioManager.instance?.Play("Dash");
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        armatureComponent.animation.Play("Dash", 1);
        armatureComponent.animation.timeScale = 1.4f;
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

    private void OnEscButton(InputValue value)
    {
        horizontal = 0f;
        AudioManager.instance?.Stop("Footsteps");
    }

    public void Reset()
    {
        gameObject.transform.position = CheckPoint.instance.GetRespornPosition();
    }
}
