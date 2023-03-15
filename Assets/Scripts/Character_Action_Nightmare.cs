using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class Character_Action_Nightmare : MonoBehaviour
{
    [SerializeField]private bool inNightmare = false;
    private GameObject background;
    private GameObject backgroundNM;

    private void Start()
    {
        background = GameObject.Find("Background");
        backgroundNM = GameObject.Find("Background_NM");
    }

    void OnNightmare(InputValue value)
    {
        if (inNightmare) 
            {
                // The player changes to normal dream
                inNightmare = false;
                background.SetActive(true);
                backgroundNM.SetActive(false);

            } 
            else 
            {
                // The player changes to nightmare
                inNightmare = true;
                background.SetActive(false);
                backgroundNM.SetActive(true);

            }

    }    

}
