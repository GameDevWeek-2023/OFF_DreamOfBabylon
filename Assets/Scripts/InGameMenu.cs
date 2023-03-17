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
    //private AudioManager audioManager;
    // Start is called before the first frame update
    void Start()
    {
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
        }
        else
        {
            ingameUI.SetActive(false);
            nightmareBar.SetActive(true);
            dialogueComponent.SetActive(dialogWasActive);
            Time.timeScale = 1;
            player.GetComponent<CubeMovementTest>().PauseInputs(dialogWasActive);
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        player.GetComponent<CubeMovementTest>().PauseInputs(false);
        SceneManager.LoadScene(0);
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
        }
    }
}
[System.Serializable]
public struct Dialog
{
    public string[] lines;
}