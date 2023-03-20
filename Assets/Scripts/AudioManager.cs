using UnityEngine.Audio;
using System; 
using UnityEngine;
using System.Collections;
using System.IO;
using Unity.VisualScripting;

public class AudioManager : MonoBehaviour
{

    [SerializeField] AudioMixer mixer;
    public Sound[] sounds;

    public static AudioManager instance;
    float masterVolume;
    float backgroundVolume;

    Sound currentlyPlayingIntro;
    Sound currentlyPlaying;
    Sound currentlyNotPlayingIntro;
    Sound currentlyNotPlaying;
    bool inNightmare;
    public const string MASTER_KEY = "masterVolume";

    void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        LoadVolume();

        foreach (Sound s in sounds)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.outputAudioMixerGroup = s.outputAudioMixerGroup;
            

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.spatialBlend = s.spatialBlend;
            s.source.dopplerLevel = s.dopplerLevel;
            s.source.minDistance = s.minDistance;
            s.source.maxDistance = s.maxDistance;
        }
        backgroundVolume = FindMusic("DreamTheme").source.volume;
        
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
        inNightmare = false; 
        Sound windUpMusicBox= FindMusic("WindUpMusicBox");
        Sound dreamThemeIntro = FindMusic("DreamThemeIntro");
        Sound nightmareThemeIntro = FindMusic("NightmareThemeIntro");
        Sound dreamTheme = FindMusic("DreamTheme");
        Sound nightmareTheme = FindMusic("NightmareTheme");
        if (dreamTheme.source.volume < nightmareTheme.source.volume)
        {
            backgroundVolume = nightmareTheme.source.volume;
        }
        dreamThemeIntro.source.volume = backgroundVolume;
        dreamTheme.source.volume = backgroundVolume;
        nightmareThemeIntro.source.volume = 0;
        nightmareTheme.source.volume = 0;
        windUpMusicBox.source.Play();
        dreamThemeIntro.source.PlayScheduled(AudioSettings.dspTime + windUpMusicBox.GetAudioClip().length);
        nightmareThemeIntro.source.PlayScheduled(AudioSettings.dspTime + windUpMusicBox.GetAudioClip().length);
        dreamTheme.source.PlayScheduled(AudioSettings.dspTime + windUpMusicBox.GetAudioClip().length + dreamThemeIntro.GetAudioClip().length);
        nightmareTheme.source.PlayScheduled(AudioSettings.dspTime + windUpMusicBox.GetAudioClip().length + dreamThemeIntro.GetAudioClip().length);
    }

    public void setInNightmare(bool inNightmare)
    {
        this.inNightmare = inNightmare;
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


        if (inNightmare == false)
        {
            currentlyPlayingIntro = nightmareThemeIntro;
            currentlyPlaying = nightmareTheme;
            currentlyNotPlayingIntro = dreamThemeIntro;
            currentlyNotPlaying = dreamTheme;
        }
        else
        {
            currentlyPlayingIntro = dreamThemeIntro;
            currentlyPlaying = dreamTheme;
            currentlyNotPlayingIntro = nightmareThemeIntro;
            currentlyNotPlaying = nightmareTheme;
        }
       
        while (timeElapsed < timeToFade)
        {
            if (currentlyPlayingIntro.source.isPlaying)
            {
                currentlyPlayingIntro.volume = backgroundVolume;
                currentlyNotPlayingIntro.volume = 0;
                currentlyPlayingIntro.source.volume = Mathf.Lerp(0, backgroundVolume*masterVolume, timeElapsed / timeToFade);
                currentlyNotPlayingIntro.source.volume = Mathf.Lerp(backgroundVolume*masterVolume, 0, timeElapsed / timeToFade);
            }
            currentlyPlaying.volume = backgroundVolume;
            currentlyNotPlaying.volume = 0;
            currentlyPlaying.source.volume = Mathf.Lerp(0, backgroundVolume*masterVolume, timeElapsed / timeToFade);
            currentlyNotPlaying.source.volume = Mathf.Lerp(backgroundVolume*masterVolume, 0, timeElapsed / timeToFade);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        inNightmare = !inNightmare;
    }

    void LoadVolume() //Volume saved in VolumeSettings.cs
    {
        masterVolume = PlayerPrefs.GetFloat(MASTER_KEY, 1f);
        mixer.SetFloat(VolumeSettings.MIXER_MASTER, Mathf.Log10(masterVolume) * 20);
    }
}
