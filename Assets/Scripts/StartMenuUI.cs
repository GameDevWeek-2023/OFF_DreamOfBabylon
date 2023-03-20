using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
//using UnityEngine.UIElements;

public class StartMenuUI : MonoBehaviour
{
    [SerializeField] Button playButton;
    [SerializeField] Button quitButton;
    [SerializeField] GameObject continueButton;
    [SerializeField] GameObject deleteSavesButton;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] GameObject Credits;
    [SerializeField] EventSystem eventSystem;
    //[SerializeField] Slider volumeSlider;
    [SerializeField] ScriptableObjectScript progressHolder;
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
            progressHolder.level = progress.level;
            progressHolder.checkPointInLevel = progress.checkPointInLevel;
            progressHolder.dialogIndex = progress.dialogIndex;
            continueButton.SetActive(true);
            eventSystem.firstSelectedGameObject = continueButton;
            deleteSavesButton.SetActive(true);
        }
        else 
        {
            progressHolder.level = 1;
            progressHolder.checkPointInLevel = 0;
            progressHolder.newGame = true;
            continueButton.SetActive(false);
            eventSystem.firstSelectedGameObject = playButton.gameObject;
            deleteSavesButton.SetActive(false);
        }
        //volumeSlider.value = AudioManager.instance.GetMasterVolume();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        AudioManager.instance.StopMainMenuMusic();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        progressHolder.newGame = true;
        progressHolder.dialogIndex = 0;
    }

    public void ContinueGame()
    {
        AudioManager.instance.StopMainMenuMusic();
        SceneManager.LoadScene(progress.level);
        progressHolder.newGame = false;
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
        Credits.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }

    public void ShowCredits()
    {
        MainMenu.SetActive(false);
        Credits.SetActive(true);
    }

    public void DeleteSaves()
    {
        if(progress!=null)
        {
            SaveSystem.DeleteSaves();
            continueButton.SetActive(false);
            deleteSavesButton.SetActive(false);
            progressHolder.newGame = true;
            progressHolder.level = 1;
            progressHolder.checkPointInLevel = 0;
        }
    }

}
