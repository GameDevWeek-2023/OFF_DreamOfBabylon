using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] GameObject ingameUI;
    [SerializeField] GameObject player;
    [SerializeField] GameObject dialogueComponent;
    [SerializeField] private TextMeshProUGUI dialogueTextComponent;
    [SerializeField] private Dialog[] dialogues;
    [SerializeField] private float textSpeed;
    private int dialogueIndex;
    private int indexInDialogue;
    // Start is called before the first frame update
    void Start()
    {
        dialogueTextComponent.text = string.Empty;
        if(SceneManager.GetActiveScene().buildIndex ==1 )
        {
            dialogueIndex = 0;
            StartDialogue();
        }
    }

    void OnEscButton(InputValue value)
    {
        ContinueGame();
    }

    public void ContinueGame()
    {
        Debug.Log("OnEscape aufgerufen");
        if (!ingameUI.activeInHierarchy)
        {

            ingameUI.SetActive(true);
            Debug.Log("Menu aufgerufen");
            Time.timeScale = 0;
            player.GetComponent<CubeMovementTest>().PauseInputs(true);
        }
        else
        {
            ingameUI.SetActive(false);
            Time.timeScale = 1;
            player.GetComponent<CubeMovementTest>().PauseInputs(false);
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        player.GetComponent<CubeMovementTest>().PauseInputs(false);
        SceneManager.LoadScene(0);
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
        }
    }
}
[System.Serializable]
public struct Dialog
{
    public string[] lines;
}