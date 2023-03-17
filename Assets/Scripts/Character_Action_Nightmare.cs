using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.Tilemaps;


public class Character_Action_Nightmare : MonoBehaviour
{
    [SerializeField]private bool inNightmare = false;
    [SerializeField]private GameObject background;
    [SerializeField]private GameObject backgroundNM;
    [SerializeField]private GameObject floor;
    [SerializeField]private GameObject floorNM;
    private bool canSwitch = true;
    private float switchCooldown = 1.1f;
    
    [SerializeField]private Slider slider;
    [SerializeField]public float fillspeed = 0.7f;


    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (slider.value < switchCooldown)
        {
            slider.value += fillspeed * Time.fixedDeltaTime;
        }
    }

    private void Start()
    {
        //progressBar.GetComponent<NightmareBarScript>();

        //SetToNightmare();
        SetToDream();
        AudioManager.instance.StartBackgroundMusic();
        
        slider.value = 1f;
    }

    private void SetToNightmare()
    {
        background.SetActive(false);
        backgroundNM.SetActive(true);
        SetTilemapActive(floor,false);
        SetTilemapActive(floorNM,true);
    }

    private void SetToDream()
    {
        inNightmare = false;
        background.SetActive(true);
        backgroundNM.SetActive(false);
        SetTilemapActive(floor,true);
        SetTilemapActive(floorNM,false);
    }

    void SetTilemapActive(GameObject go, bool active) {
        float alpha = active ? 1 : 0.25f;
        go.GetComponent<Tilemap>().color = new Color(1,1,1,alpha);
        Collider2D[] cols = go.GetComponentsInChildren<Collider2D>();
        SpriteRenderer[] sps = go.GetComponentsInChildren<SpriteRenderer>();
        ParticleSystem[] pss = go.GetComponentsInChildren<ParticleSystem>();

        foreach(Collider2D col in cols) {
            col.enabled = active;
        }

        foreach(SpriteRenderer sp in sps) {
                Color color = sp.color;
                color.a =  alpha;
                sp.color = color;
        }

       foreach(ParticleSystem ps in pss) {
            ps.gameObject.SetActive(active);
        }
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
        AudioManager.instance.SwapBackgroundAudios();
        slider.value = 0f;
        SetToDream();
        yield return new WaitForSeconds(switchCooldown);
        canSwitch = true;
    }
    
    private IEnumerator ToNightmare()
    {
        canSwitch = false;
        inNightmare = true;
        AudioManager.instance.SwapBackgroundAudios();
        slider.value = 0f;
        SetToNightmare();
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


  