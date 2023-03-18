using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] GameObject ingameUI;
    [SerializeField] GameObject optionsMenu;
    [SerializeField] GameObject player;
    [SerializeField] GameObject dialogueComponent;
    [SerializeField] GameObject nightmareBar;
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private TextMeshProUGUI dialogueTextComponent;
    [SerializeField] private Dialog[] dialogues;
    [SerializeField] private float textSpeed;
    private bool dialogWasActive;
    private int dialogueIndex;
    private int indexInDialogue;
    float timer = 0.0f;
    

    private PlayerProgress progress;

    //private AudioManager audioManager;
    // Start is called before the first frame update

    private void Awake()
    {
        progress = SaveSystem.LoadProgress();
    }
    void Start()
    {
        if (progress != null)
        {
            AudioManager.instance.ChangeVolume(progress.audioVolume);
            CheckPoint[] cp = FindObjectsOfType<CheckPoint>();
            if(cp.Length == 0)
            {
                Debug.Log("No Check points");
            }
            else
            {
                foreach(CheckPoint c in cp)
                {
                    if(c.NumberOfCheckPoint == progress.checkPointInLevel)
                    {
                        CheckPoint.instance = c;
                        player.transform.position = CheckPoint.instance.transform.GetChild(0).transform.position;
                    }
                }
            }        }
        volumeSlider.value = AudioManager.instance.GetMasterVolume();
        //audioManager = FindObjectOfType<AudioManager>();
        dialogueTextComponent.text = string.Empty;
        if(SceneManager.GetActiveScene().buildIndex ==1 )
        {
            player.GetComponent<CubeMovementTest>().PauseInputs(true);
            Debug.Log("BuildIndex 1");
            dialogueIndex = 0;
            StartDialogue();
        }
        dialogWasActive = dialogueComponent.activeInHierarchy;
    }

    void OnEscButton(InputValue value)
    {
        if(!optionsMenu.activeInHierarchy)
            ContinueGame();
        else
        {
            optionsMenu.SetActive(false);
            ingameUI.SetActive(true);
        }
    }

    public void ContinueGame()
    {
        Debug.Log("OnEscape aufgerufen");
        if (!ingameUI.activeInHierarchy)
        {

            ingameUI.SetActive(true);
            nightmareBar.SetActive(false);
            dialogueComponent.SetActive(false);
            Debug.Log("Menu aufgerufen");
            Time.timeScale = 0;
            player.GetComponent<CubeMovementTest>().PauseInputs(true);
            player.GetComponent<Character_Action_Nightmare>().PauseInputs(true);
            if ((bool)(AudioManager.instance.FindMusic("DreamThemeIntro")?.source.isPlaying))
            {
                AudioManager.instance.Pause("DreamThemeIntro");
                AudioManager.instance.Pause("NightmareThemeIntro");
                timer += Time.deltaTime;
            }
            if ((bool)(AudioManager.instance.FindMusic("DreamTheme")?.source.isPlaying))
            {

                AudioManager.instance.Pause("DreamTheme");
                AudioManager.instance.Pause("NightmareTheme");
            }
            AudioManager.instance.StartMainMenuMusic();

        }
        else
        {
            if((bool)(AudioManager.instance.FindMusic("DreamThemeIntro")?.isPaused))
            {
                AudioManager.instance.Play("DreamThemeIntro");
                AudioManager.instance.Play("NightmareThemeIntro");
            }
            if ((bool)(AudioManager.instance.FindMusic("DreamTheme")?.isPaused))
            {
                StartCoroutine(StartBackgroundMusicAgain());
            }
            AudioManager.instance.StopMainMenuMusic();
            ingameUI.SetActive(false);
            nightmareBar.SetActive(true);
            dialogueComponent.SetActive(dialogWasActive);
            Time.timeScale = 1;
            player.GetComponent<CubeMovementTest>().PauseInputs(dialogWasActive);
            player.GetComponent<Character_Action_Nightmare>().PauseInputs(dialogWasActive);
        }
    }

    IEnumerator StartBackgroundMusicAgain()
    {
        yield return new WaitForSeconds(timer);
        AudioManager.instance.Play("DreamTheme");
        AudioManager.instance.Play("NightmareTheme");
    }

    public void BackToMainMenu()
    {
        if ((bool)(AudioManager.instance.FindMusic("DreamThemeIntro")?.source.isPlaying) || (bool)(AudioManager.instance.FindMusic("DreamThemeIntro")?.isPaused))
        {
            AudioManager.instance.Stop("DreamThemeIntro");
            AudioManager.instance.Stop("NightmareThemeIntro");
        }
        if ((bool)(AudioManager.instance.FindMusic("DreamTheme")?.source.isPlaying) || (bool)(AudioManager.instance.FindMusic("DreamTheme")?.isPaused))
        {
            AudioManager.instance.Stop("DreamTheme");
            AudioManager.instance.Stop("NightmareTheme");
        }
        Time.timeScale = 1;
        player.GetComponent<CubeMovementTest>().PauseInputs(false);
        SceneManager.LoadScene(0);
        SaveSystem.SaveProgress(new PlayerProgress(SceneManager.GetActiveScene().buildIndex, CheckPoint.instance.NumberOfCheckPoint, AudioManager.instance.GetMasterVolume()));
    }

    public void OpenOptions()
    {
        ingameUI.SetActive(false);
        optionsMenu.SetActive(true);
    }

    public void ShowPauseMenu()
    {
        ingameUI.SetActive(true);
        optionsMenu.SetActive(false);
    }

    public void ChangeMusicVolume()
    {
        Debug.Log("New Value: " + volumeSlider.value);
        AudioManager.instance?.ChangeVolume(volumeSlider.value);
    }

    // Update is called once per frame
    void Update()
    {
        if(dialogueComponent.activeInHierarchy&&(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Return)))
        {
            if(dialogueTextComponent.text == dialogues[dialogueIndex].lines[indexInDialogue].Replace("\\n", "\n"))
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                dialogueTextComponent.text = dialogues[dialogueIndex].lines[indexInDialogue].Replace("\\n", "\n");   
            }
        }
    }

    public void StartDialogue()
    {
        indexInDialogue = 0;
        dialogueComponent.SetActive(true);
        dialogWasActive = true;
        player.GetComponent<CubeMovementTest>().PauseInputs(true);
        player.GetComponent<Character_Action_Nightmare>().PauseInputs(true);
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        string s = "";
        foreach(char c in dialogues[dialogueIndex].lines[indexInDialogue].ToCharArray())
        {
            if(!s.Contains("\\"))
            {
                s = c.ToString();
            }else
            {
                s += c.ToString();
            }
            if (c == "\\".ToCharArray()[0])
            {
                continue;
            }
            //Debug.Log(s);
            s = s.Replace("\\n", "\n");
            dialogueTextComponent.text += s;
            s = "";
            //dialogueTextComponent.text.Replace("\\n", "\n");
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (indexInDialogue < dialogues[dialogueIndex].lines.Length - 1)
        {
            indexInDialogue++;
            dialogueTextComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            dialogueIndex++;
            dialogueComponent.SetActive(false);
            dialogWasActive = false;
            player.GetComponent<CubeMovementTest>().PauseInputs(dialogWasActive);
            player.GetComponent<Character_Action_Nightmare>().PauseInputs(dialogWasActive);
        }
    }
}
[System.Serializable]
public struct Dialog
{
    public string[] lines;
}