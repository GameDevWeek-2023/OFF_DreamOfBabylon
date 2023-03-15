using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] GameObject Target;
    public string OnMessage;
    public string OffMessage;
    public bool isOn;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void TurnOn()
    {
        if(!isOn)
        {
            SetState(true);
        }
    }
    public void TurnOff()
    {
        if(isOn)
        {
            SetState(false);
        }
    }
    public void Toggle()
    {
        if (isOn)
        {
            TurnOff();
        }
        else
        {
            TurnOn();
        }
    }

    void SetState(bool on)
    {
        isOn = on;
        animator.SetBool("On", on);
        if(on)
        {
            if(Target != null && !string.IsNullOrEmpty(OnMessage))
            {
                Target.SendMessage(OnMessage);
            }
        }
        else
        {
            if (Target != null && !string.IsNullOrEmpty(OffMessage))
            {
                Target.SendMessage(OffMessage);
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
