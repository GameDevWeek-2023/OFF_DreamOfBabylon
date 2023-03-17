using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabbingMechanic : MonoBehaviour
{

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
                FindObjectOfType<AudioManager>().Play("Grab");

                grabbedObject = hitInfo.collider.gameObject;

                Rigidbody2D storeRB = grabbedObject.GetComponent<Rigidbody2D>();
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
            FindObjectOfType<AudioManager>().Stop("Grab");

            grabbedObject.AddComponent<Rigidbody2D>();
            Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D>();
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
