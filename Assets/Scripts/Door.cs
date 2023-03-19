using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] AudioSource door;
    [SerializeField] private bool isOpen;
    [SerializeField] private int knocksToOpen = 1;
    [SerializeField] private Sprite imgOpenDoor;
    [SerializeField] private Sprite imgClosedDoor;

    private Collider2D collider;
    private Animator animator;
    private int knocks = 0;

    private SpriteRenderer spriteRenderer;
  
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

        if (isOpen) 
        {
            // set img open
            spriteRenderer.sprite = imgOpenDoor;
        }
        else 
        {
            // set img closed
            spriteRenderer.sprite = imgClosedDoor;

        }
        collider = GetComponent<Collider2D>();
        TryGetComponent<Animator>(out animator);
        //animator = GetComponent<Animator>();
    }

    public void Open()
    {
        knocks++;
        if (!isOpen && knocks >= knocksToOpen)
        {
            spriteRenderer.sprite = imgOpenDoor;
            door.Play();
            SetState(true);
        }
            
    }

    public void Close()
    {
        Debug.Log("trying to Close");
        if(knocks>0)
            knocks--;
        if(isOpen && knocks < knocksToOpen)
        {
            SetState(false);
            Debug.Log("success");
            spriteRenderer.sprite = imgClosedDoor;
            door.Play();            
            return;
        }
        Debug.Log("failed");
    }

    public void Toggle()
    {
        if(isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void SetState(bool open)
    {
        isOpen = open;
        collider.isTrigger = open;
        animator?.SetBool("Open", open);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
