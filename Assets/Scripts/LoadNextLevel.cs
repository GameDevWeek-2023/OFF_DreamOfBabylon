using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadNextLevel : MonoBehaviour
{

    [SerializeField] AudioSource succsess;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        succsess.Play();
        if (collision.gameObject.CompareTag("Player") && SceneManager.GetActiveScene().buildIndex<SceneManager.sceneCountInBuildSettings-1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
