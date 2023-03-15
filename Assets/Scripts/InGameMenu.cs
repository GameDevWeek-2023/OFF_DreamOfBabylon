using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] GameObject ingameUI;
    // Start is called before the first frame update
    void Start()
    {
        
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
        }
        else
        {
            ingameUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void BackToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
