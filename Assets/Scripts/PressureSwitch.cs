using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PressureSwitch : Switch
{
    int numberColliding = 0;
    public List<GameObject> onSwitch = new List<GameObject>();

    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //numberColliding++;
        if(!onSwitch.Contains(collision.gameObject))
        {
            onSwitch.Add(collision.gameObject);
            FindObjectOfType<AudioManager>().Play("PressureSwitch");
        }
        TurnOn();
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
    }
}
