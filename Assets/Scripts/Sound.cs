using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;

    public AudioClip clip;

    public AudioMixerGroup outputAudioMixerGroup;

    [Range(0f, 1f)]
    public float volume;
    [Range (1f, 3)]
    public float pitch;

    public bool loop; 

    [HideInInspector]
    public AudioSource source;

    public float spatialBlend = 1f;
    
    public float dopplerLevel;

    public float minDistance;

    public float maxDistance;

    public bool isPaused = false;

    public AudioClip GetAudioClip()
    {
        return clip;
    }
}