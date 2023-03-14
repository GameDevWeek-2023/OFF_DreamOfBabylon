using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 2;
    private PlayerControls input = null;
    private float horizontal;
    private Transform playerTransform;
    private Rigidbody2D rb;
    Vector2 Direction;
    // Start is called before the first frame update
    private void Awake()
    {
        input = new PlayerControls();
        rb = gameObject.GetComponent<Rigidbody2D>();
        if(rb == null)
        {
            Debug.LogWarning("No Player RigidBody found");
        }
        playerTransform = gameObject.transform;
        Direction = new Vector2(0, 0);
    }
    void Start()
    {
        
    }
    public void OnMovementPerformed(InputAction.CallbackContext value)
    {
        //horizontal = value.ReadValue<>();
        //rb.AddForce()
    } 
    public void OnMovementCancelled(InputAction.CallbackContext value)
    {
        horizontal = 0;


    }

    private void OnEnable()
    {
        input.Enable();
        input.Input.Movement.performed += OnMovementPerformed;
        input.Input.Movement.canceled += OnMovementCancelled;
    }

    private void OnDisable()
    {
        input.Disable();
        input.Input.Movement.performed -= OnMovementPerformed;
        input.Input.Movement.canceled -= OnMovementCancelled;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        Debug.Log(horizontal);
    }
}
