using UnityEngine.Audio;
using System; 
using UnityEngine;
using System.Collections;
using System.IO;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;

    private bool nightmare = false;

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return; 
        }
        DontDestroyOnLoad(gameObject);

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        } 
    }

    private void Start()
    {
        Play("DreamTheme");
    }

    public void Play(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound: {name} not found.");
            return;
        }
        s.source.Play();
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound: {name} not found.");
            return;
        }
        s.source.Stop();
    }

    public void SwapBackgroundAudios()
    {
        StopAllCoroutines();
        StartCoroutine(FadeBackgroundAudios());
    }
    
    private IEnumerator FadeBackgroundAudios()
    {
        float timeToFade = 1f;
        float timeElapsed = 0;
        Sound s1 = Array.Find(sounds, sound => sound.name == "DreamTheme");
        Sound s2 = Array.Find(sounds, sound => sound.name == "NightmareTheme");
        if (nightmare == false)
        {
            Play("NightmareTheme");
            while(timeElapsed < timeToFade)
            {
                s2.source.volume = Mathf.Lerp(0, 1, timeElapsed/timeToFade);
                s1.source.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            Stop("DreamTheme");
            nightmare = true;
        }
        else
        {
            Play("DreamTheme");
            while (timeElapsed < timeToFade)
            {
                s1.source.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                s2.source.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
            Stop("NightmareTheme");
            nightmare = false;
        }
    }
}
