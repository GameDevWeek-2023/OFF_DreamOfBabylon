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
    private GameObject floor;
    private GameObject floorNM;

    private void Start()
    {
        background = GameObject.Find("Background");
        backgroundNM = GameObject.Find("Background_NM");
        floor = GameObject.Find("Floortiles");
        floorNM = GameObject.Find("Floortiles_NM");

        SetToNightmare();
        SetToDream();
        AudioManager.instance.StartBackgroundMusic();
    }

    private void SetToNightmare()
    {
        background.SetActive(false);
        backgroundNM.SetActive(true);
        floor.SetActive(false);
        floorNM.SetActive(true);
    }

    private void SetToDream()
    {
        inNightmare = false;
        background.SetActive(true);
        backgroundNM.SetActive(false);
        floor.SetActive(true);
        floorNM.SetActive(false); 
    }


    void OnNightmare(InputValue value)
    {
        if (inNightmare) 
            {
                // The player changes to normal dream
                SetToDream();
            } 
            else 
            {
                // The player changes to nightmare
                inNightmare = true;
                SetToNightmare();
            }
        AudioManager.instance.SwapBackgroundAudios();

    }    

}


  