using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private bool isOpen;
    private Collider2D collider;
    private Animator animator;



    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        TryGetComponent<Animator>(out animator);
        //animator = GetComponent<Animator>();
    }

    public void Open()
    {
        if (!isOpen)
            SetState(true);
    }

    public void Close()
    {
        if(isOpen)
        {
            SetState(false);
        }
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
