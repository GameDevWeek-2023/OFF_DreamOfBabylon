using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GrabbingMechanic : MonoBehaviour
{

    [SerializeField] private Transform grabPoint;

    [SerializeField] private Transform rayPoint;

    [SerializeField] private float rayDistance;

    private GameObject grabbedObject;

    [SerializeField]private GameObject player;

    private int layerIndex;
    private Vector2 direction;

    private Rigidbody2D boxrb;
    private bool isGrabbing=false;

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
                grabbedObject = hitInfo.collider.gameObject;
                Destroy(grabbedObject.GetComponent<Rigidbody2D>());
                grabbedObject.transform.position = grabPoint.position;
                grabbedObject.transform.SetParent(transform);
            }
        }
        else
        {
            grabbedObject.AddComponent<Rigidbody2D>();
            Rigidbody2D rb = grabbedObject.GetComponent<Rigidbody2D>();
            rb.sharedMaterial = new PhysicsMaterial2D("BoxMaterial");
            rb.mass = 10f;
            rb.drag = 0.3f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
            rb.freezeRotation = true;
                
            grabbedObject.transform.SetParent(null);
            grabbedObject = null;
        }
    }
}
