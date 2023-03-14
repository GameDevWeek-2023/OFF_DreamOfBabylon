using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSoundtest : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            AudioManager.instance.SwapBackgroundAudios();
        }
    }
}
