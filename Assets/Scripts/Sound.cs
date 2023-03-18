using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float volume;
    [Range (1f, 3)]
    public float pitch;

    public bool loop; 

    [HideInInspector]
    public AudioSource source;

    [HideInInspector]
    public float spatialBlend = 1f;
    /*
    public float dopplerLevel;

    [HideInInspector]
    public float minDistance = 2;

    [HideInInspector]
    public float maxDistance = 6;*/

    public bool isPaused = false;

    public AudioClip GetAudioClip()
    {
        return clip;
    }
}