using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;



public class PressureSwitch : Switch
{

    [SerializeField] AudioSource pressureSwitch;

    int numberColliding = 0;
    public List<GameObject> onSwitch = new List<GameObject>();
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //numberColliding++;
        if(!onSwitch.Contains(collision.gameObject))
        {
            if (onSwitch.Count == 0)
            {
                pressureSwitch.Play();
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

        }

        Debug.Log("Exit. On Switch: " + onSwitch.Count);
    }
}
