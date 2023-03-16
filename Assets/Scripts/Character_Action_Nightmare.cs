using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class Character_Action_Nightmare : MonoBehaviour
{
    [SerializeField]private bool inNightmare = false;
    private GameObject background;
    private GameObject backgroundNM;
    private GameObject floor;
    private GameObject floorNM;
    private bool canSwitch = true;
    private float switchCooldown = 1.1f;
    
    [SerializeField]private Slider slider;
    public float fillspeed = 1.1f;

    private void Update()
    {
        if (slider.value < switchCooldown)
        {
            slider.value += fillspeed * Time.deltaTime;
        }
    }

    private void Start()
    {
        //progressBar.GetComponent<NightmareBarScript>();
        background = GameObject.Find("Background");
        backgroundNM = GameObject.Find("Background_NM");
        floor = GameObject.Find("Floortiles");
        floorNM = GameObject.Find("Floortiles_NM");

        SetToNightmare();
        SetToDream();
        AudioManager.instance.StartBackgroundMusic();
        
        slider.value = 1f;
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
        if (inNightmare && canSwitch) 
            {
                StartCoroutine(ToDream());
                // The player changes to normal dream
                //SetToDream();
            } 
            else if (!inNightmare && canSwitch)
            {
                StartCoroutine(ToNightmare());
                // The player changes to nightmare
                //inNightmare = true;
                //SetToNightmare();
            }
        //AudioManager.instance.SwapBackgroundAudios();

    }
    
    private IEnumerator ToDream()
    {
        canSwitch = false;
        inNightmare = false;
        background.SetActive(true);
        backgroundNM.SetActive(false);
        floor.SetActive(true);
        floorNM.SetActive(false);
        AudioManager.instance.SwapBackgroundAudios();
        slider.value = 0f;
        yield return new WaitForSeconds(switchCooldown);
        canSwitch = true;
    }
    
    private IEnumerator ToNightmare()
    {
        canSwitch = false;
        inNightmare = true;
        background.SetActive(false);
        backgroundNM.SetActive(true);
        floor.SetActive(false);
        floorNM.SetActive(true);
        AudioManager.instance.SwapBackgroundAudios();
        slider.value = 0f;
        yield return new WaitForSeconds(switchCooldown);
        canSwitch = true;
    }

    public bool IsCurrentThemeNightmare()
    {
        return inNightmare;
    }
    
    /*public void IncrementProgress(float newProgress)
    {
        slider.value = 0f;
        targetProgress = slider.value + newProgress;
    }*/

}


  