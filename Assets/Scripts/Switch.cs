using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour, IResetable
{

    public enum ResetType { Never, OnUse, Timed, Immediately}
    [SerializeField] private ResetType resetType = ResetType.OnUse;
    [SerializeField] GameObject Target;
    public string OnMessage = "Open";
    public string OffMessage = "Close";
    public bool isOn;
    [SerializeField] private float resetTime;
    [SerializeField] private Sprite imgSwitchOff;
    [SerializeField] private Sprite imgSwitchOn;
    Animator animator;
    private SpriteRenderer spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        TryGetComponent<Animator>(out animator);
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();
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
        if(isOn && resetType != ResetType.Never && resetType != ResetType.Timed)
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

    void TimedReset()
    {
        SetState(false);
    }

    void SetState(bool on)
    {
        isOn = on;
        animator?.SetBool("On", on);
        if(on)
        {
            spriteRenderer.sprite = imgSwitchOn;
            if (Target != null && !string.IsNullOrEmpty(OnMessage))
            {
                Target.SendMessage(OnMessage);
            }
            switch (resetType)
            {
                case ResetType.Never:
                    break;
                case ResetType.OnUse:
                    break;
                case ResetType.Timed:
                    Invoke("TimedReset", resetTime);
                    break;
                case ResetType.Immediately:
                    TurnOff();
                    break;
                default:
                    break;
            }
        }
        else
        {
            spriteRenderer.sprite = imgSwitchOff;
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

    public void ResetToStart()
    {
        if(isOn)
            SetState(false);
    }
}
