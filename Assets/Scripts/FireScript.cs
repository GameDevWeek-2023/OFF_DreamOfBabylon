using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class FireScript : MonoBehaviour
{

    [SerializeField] AudioSource fire;
    [SerializeField] private bool inNightmare = false;

    private void Update()
    {
        if (gameObject.GetComponentInChildren<ParticleSystem>().isPlaying)
        {
            if(!((bool)(fire.isPlaying)))
            {
                fire.Play();
            }
        }
        else
        {
            fire.Stop();
        }
    }
}
