using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Character_Action_Nightmare : MonoBehaviour
{
    public bool inNightmare = false; 

    
    // Start is called before the first frame update
    void Start()
    {
   
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Nightmare(InputAction.CallbackContext context)
    {
        if (context.performed) 
        {
            if (inNightmare) 
            {
                // The player changes to normal dream
                inNightmare = false;
                GameObject.Find("Background").SetActive(true);
                GameObject.Find("Background_NM").SetActive(false);

            } 
            else 
            {
                // The player changes to nightmare
                inNightmare = true;
                GameObject.Find("Background").SetActive(false);
                GameObject.Find("Background_NM").SetActive(true);

            } 
        }
        
    }    

}
