using UnityEngine.Audio;
using System; 
using UnityEngine;
using System.Collections;
using System.IO;

public class AudioManager : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManager instance;
    private float masterVolume = 1;
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
            /*s.source.spatialBlend = s.spatialBlend;
            s.source.dopplerLevel = s.dopplerLevel;
            s.source.minDistance = s.minDistance;
            s.source.maxDistance = s.maxDistance;*/
        } 
    }
    public void StartMainMenuMusic()
    {
        Sound mainMenuMusicIntro = FindMusic("MainMenuMusicIntro");
        Sound mainMenuMusicLoop = FindMusic("MainMenuMusicLoop");
        mainMenuMusicIntro.source.Play();
        mainMenuMusicLoop.source.PlayScheduled(AudioSettings.dspTime + mainMenuMusicIntro.GetAudioClip().length);
    }
    public void StopMainMenuMusic()
    {
        Sound mainMenuMusicIntro = FindMusic("MainMenuMusicIntro");
        Sound mainMenuMusicLoop = FindMusic("MainMenuMusicLoop");
        mainMenuMusicIntro.source.Stop();
        mainMenuMusicLoop.source.Stop();
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
        dreamTheme.source.PlayScheduled(AudioSettings.dspTime + windUpMusicBox.GetAudioClip().length + dreamThemeIntro.GetAudioClip().length);
        nightmareTheme.source.PlayScheduled(AudioSettings.dspTime + windUpMusicBox.GetAudioClip().length + dreamThemeIntro.GetAudioClip().length);
    }

    public float GetMasterVolume()
    {
        return masterVolume;
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
        s.isPaused = false;
    }

    public void Pause(string name)
    {
        Sound s = FindMusic(name);
        s.source.Pause();
        s.isPaused = true;
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

    public void ChangeVolume(float newVolume)
    {
        this.masterVolume = newVolume;
        foreach (Sound s in sounds)
        {

            s.source.volume = s.volume * masterVolume;
        }
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
            while (timeElapsed < timeToFade)
            {
                if (dreamThemeIntro.source.isPlaying)
                {
                    nightmareThemeIntro.volume = 0.1f;
                    dreamThemeIntro.volume = 0;
                    nightmareThemeIntro.source.volume = Mathf.Lerp(0, 0.1f*masterVolume, timeElapsed / timeToFade);
                    dreamThemeIntro.source.volume = Mathf.Lerp(0.1f*masterVolume, 0, timeElapsed / timeToFade);
                }
                nightmareTheme.volume = 0.1f;
                dreamTheme.volume = 0;
                nightmareTheme.source.volume = Mathf.Lerp(0, 0.1f*masterVolume, timeElapsed / timeToFade);
                dreamTheme.source.volume = Mathf.Lerp(0.1f*masterVolume, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            while (timeElapsed < timeToFade)
            {
                if (nightmareThemeIntro.source.isPlaying)
                {
                    nightmareThemeIntro.volume = 0;
                    dreamThemeIntro.volume = .1f;
                    dreamThemeIntro.source.volume = Mathf.Lerp(0, 0.1f*masterVolume, timeElapsed / timeToFade);
                    nightmareThemeIntro.source.volume = Mathf.Lerp(0.1f*masterVolume, 0, timeElapsed / timeToFade);
                }
                nightmareTheme.volume = 0;
                dreamTheme.volume = 0.1f;
                dreamTheme.source.volume = Mathf.Lerp(0, 0.1f*masterVolume, timeElapsed / timeToFade);
                nightmareTheme.source.volume = Mathf.Lerp(0.1f*masterVolume, 0, timeElapsed / timeToFade);
                timeElapsed += Time.deltaTime;
                yield return null;
            }
        }
        nightmare = !nightmare;
    }
}
