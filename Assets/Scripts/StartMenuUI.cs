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
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject OptionsMenu;
    [SerializeField] Slider volumeSlider;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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

    public void ChangeMusicVolume()
    {
        Debug.Log("New Value: " + volumeSlider.value);
    }

}
