using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class PressureSwitch : Switch
{

    [SerializeField] AudioSource pressureSwitch;
    [SerializeField] private Sprite imgSwitchOff;
    [SerializeField] private Sprite imgSwitchOn;
    private SpriteRenderer spriteRenderer;

    int numberColliding = 0;
    public List<GameObject> onSwitch = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //numberColliding++;
        if(!onSwitch.Contains(collision.gameObject))
        {
            if (onSwitch.Count == 0)
            {
                pressureSwitch.Play();
                spriteRenderer.sprite = imgSwitchOn;
            }
            onSwitch.Add(collision.gameObject);
        }
        TurnOn();
        Debug.Log("Enter. On Switch: " + onSwitch.Count);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        //numberColliding--;
        if (onSwitch.Contains(collision.gameObject))
        {
            onSwitch.Remove(collision.gameObject);
        }
        if(onSwitch.Count == 0)
        {
            TurnOff();
            spriteRenderer.sprite = imgSwitchOff;

        }

        Debug.Log("Exit. On Switch: " + onSwitch.Count);
    }
}
