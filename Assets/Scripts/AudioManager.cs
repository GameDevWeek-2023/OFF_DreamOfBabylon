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

    public void StartBackgroundMusic()
    {
        Sound windUpMusicBox= FindMusic("WindUpMusicBox");
        Sound dreamThemeIntro = FindMusic("DreamThemeIntro");
        Sound nightmareThemeIntro = FindMusic("NightmareThemeIntro");
        Sound dreamTheme = FindMusic("DreamTheme");
        Sound nightmareTheme = FindMusic("NightmareTheme");
        windUpMusicBox.source.Play();
        dreamThemeIntro.source.PlayScheduled(AudioSettings.dspTime + windUpMusicBox.GetAudioClip().length);
        nightmareThemeIntro.source.PlayScheduled(AudioSettings.dspTime + windUpMusicBox.GetAudioClip().length);
        dreamTheme.source.PlayScheduled(AudioSettings.dspTime + dreamThemeIntro.GetAudioClip().length);
        nightmareTheme.source.PlayScheduled(AudioSettings.dspTime + dreamThemeIntro.GetAudioClip().length);
    }

    public Sound FindMusic(string name)
    {
        Sound s = Array.Find(sounds, sound => sound.name == name);
        if (s == null)
        {
            Debug.LogWarning($"Sound: {name} not found.");
            return null;
        }
        return s;
    }

    public void Play(string name)
    {
        Sound s = FindMusic(name);
        s.source.Play();
        
    }

    public void Stop(string name)
    {
        Sound s = FindMusic(name);
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
        Sound dreamThemeIntro = Array.Find(sounds, sound => sound.name == "DreamThemeIntro");
        Sound nightmareThemeIntro = Array.Find(sounds, sound => sound.name == "NightmareThemeIntro");
        Sound dreamTheme = Array.Find(sounds, sound => sound.name == "DreamTheme");
        Sound nightmareTheme = Array.Find(sounds, sound => sound.name == "NightmareTheme");
        if (nightmare == false)
        {
            
            if (dreamThemeIntro.source.isPlaying)
            {
                while (timeElapsed < timeToFade)
                {
                    nightmareThemeIntro.source.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                    dreamThemeIntro.source.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
            }
            while (timeElapsed < timeToFade)
            {
                nightmareTheme.source.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                dreamTheme.source.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            if (nightmareThemeIntro.source.isPlaying)
            {
                while (timeElapsed < timeToFade)
                {
                    dreamThemeIntro.source.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                    nightmareThemeIntro.source.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                    timeElapsed += Time.deltaTime;
                    yield return null;
                }
            }
            while (timeElapsed < timeToFade)
            {
                dreamTheme.source.volume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);
                nightmareTheme.source.volume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        nightmare = !nightmare;
    }
}
