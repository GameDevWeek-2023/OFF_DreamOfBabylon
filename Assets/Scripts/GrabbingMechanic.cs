using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabbingMechanic : MonoBehaviour
{
    [SerializeField] AudioSource grab;

    [SerializeField] private Transform grabPoint;

    [SerializeField] private Transform dropPoint;

    [SerializeField] private Transform rayPoint;

    [SerializeField] private float rayDistance;

    private GameObject grabbedObject;

    [SerializeField]private GameObject player;

    private int layerIndex;
    private Vector2 direction;
    private bool isGrabbing=false;
    
    //store Settings Rigidbody
    private PhysicsMaterial2D rbMaterial;
    private float rbMass;
    private float rbDrag;
    private CollisionDetectionMode2D rbCollDect;
    private RigidbodyInterpolation2D rbInterpol;
    private bool rbFreezeRot;
    private float rbGravityScale;
    private bool pauseInputs = false;

    public bool PauseInputs
    {
        get { return pauseInputs;}
        set { pauseInputs = value; }
    }


    // Start is called before the first frame update
    void Start()
    {
        layerIndex = LayerMask.NameToLayer("GrabObjects");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnInteract(InputValue value)
    {

        if (pauseInputs)
        {
            return;
        }

        if (player.transform.localScale.x > 0f)
        {
            direction = Vector2.right;
        }
        else
        {
            direction = Vector2.left;
        }

        if (grabbedObject == null)
        {
            RaycastHit2D hitInfo = Physics2D.Raycast(rayPoint.position, direction, rayDistance);
            if (hitInfo.collider != null && hitInfo.collider.gameObject.layer == layerIndex)
            {
                grab.Play();

                grabbedObject = hitInfo.collider.gameObject;

                Rigidbody2D storeRB = grabbedObject.GetComponent<Rigidbody2D>();
                rbGravityScale = storeRB.gravityScale;
                rbMaterial= storeRB.sharedMaterial;
                rbMass = storeRB.mass;
                rbDrag = storeRB.drag;
                rbCollDect = storeRB.collisionDetectionMode;
                rbInterpol = storeRB.interpolation;
                rbFreezeRot = storeRB.freezeRotation;
                
                Destroy(grabbedObject.GetComponent<Rigidbody2D>());
                
                grabbedObject.transform.position = grabPoint.position;
                grabbedObject.transform.SetParent(transform);
            }
        }
        else
        {
            grab.Stop();

            grabbedObject.AddComponent<Rigidbody2D>();
            Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D>();
            rb.gravityScale = rbGravityScale;
            rb.sharedMaterial = rbMaterial;
            rb.mass = rbMass;
            rb.drag = rbDrag;
            rb.collisionDetectionMode = rbCollDect;
            rb.interpolation = rbInterpol;
            rb.freezeRotation = rbFreezeRot;
                
            grabbedObject.transform.position = dropPoint.position;
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
        }
    }
}
