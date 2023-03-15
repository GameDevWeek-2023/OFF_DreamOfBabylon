using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{

    [SerializeField] private bool isOpen;
    [SerializeField] private int knocksToOpen = 1;
    private Collider2D collider;
    private Animator animator;
    private int knocks = 0;



    // Start is called before the first frame update
    void Start()
    {
        collider = GetComponent<Collider2D>();
        TryGetComponent<Animator>(out animator);
        //animator = GetComponent<Animator>();
    }

    public void Open()
    {
        knocks++;
        if (!isOpen && knocks >= knocksToOpen)
            SetState(true);
    }

    public void Close()
    {
        Debug.Log("trying to Close");
        knocks--;
        if(isOpen && knocks < knocksToOpen)
        {
            SetState(false);
            Debug.Log("success");
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
