using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseableSwitch : Switch
{

    [SerializeField] AudioSource usableSwitch;
    public void Use()
    {
        Toggle();
        usableSwitch.Play();
    }
}
