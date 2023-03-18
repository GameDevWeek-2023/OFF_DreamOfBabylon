using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//using UnityEngine.UIElements;

public class StartMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject deleteSavesButton;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] Slider volumeSlider;
    private PlayerProgress progress;
    // Start is called before the first frame update
    void Start()
    {
        if(!((bool)(AudioManager.instance?.FindMusic("MainMenuMusicIntro")?.source.isPlaying) || (bool)(AudioManager.instance?.FindMusic("MainMenuMusicLoop")?.source.isPlaying)))
            AudioManager.instance?.StartMainMenuMusic();
        progress = SaveSystem.LoadProgress();
        if (progress != null)
        {
            AudioManager.instance.ChangeVolume(progress.audioVolume);
            continueButton.SetActive(true);
            deleteSavesButton.SetActive(true);
        }
        else 
        {
            continueButton.SetActive(false);
            deleteSavesButton.SetActive(false);
        }
        volumeSlider.value = AudioManager.instance.GetMasterVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        AudioManager.instance.StopMainMenuMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void ContinueGame()
    {
        AudioManager.instance.StopMainMenuMusic();
        SceneManager.LoadScene(progress.level);
    }

    public void ShowOptions()
    {
        MainMenu.SetActive(false);
        OptionsMenu.SetActive(true);
    }
    public void ShowMainMenu ()
    {
        MainMenu.SetActive(true);
        OptionsMenu.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void DeleteSaves()
    {
        if(progress!=null)
        {
            SaveSystem.DeleteSaves();
            continueButton.SetActive(false);
            deleteSavesButton.SetActive(false);
        }
    }

    public void ChangeMusicVolume()
    {
        Debug.Log("New Value: " + volumeSlider.value);
        AudioManager.instance?.ChangeVolume(volumeSlider.value);
    }

}
