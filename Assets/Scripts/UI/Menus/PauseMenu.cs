using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    //public GameObject pauseMenu;
    public GameObject[] menuParts;
    public GameObject[] settingsMenuParts;
    public bool isPaused;
    public string pauseButton = "Pause";
    public GameObject[] UI;

    private CheckpointMaster cm;
    private ScreenWipe sw;
    private Scene currentScene;
    void Start()
    {
        //pauseMenu.SetActive(false);
        foreach (GameObject part in menuParts)
        {
            part.SetActive(false);
        }
    }


    void Update()
    {

        if (Input.GetButtonDown(pauseButton))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }     
        
    }

    public void PauseGame()
    {
        foreach (GameObject part in menuParts)
        {
            part.SetActive(true);
        }
        foreach (GameObject part in UI)
        {
            part.SetActive(false);
        }

        Time.timeScale = 0;
        isPaused = true;
    }

    

    public void ResumeGame()
    {
        foreach (GameObject part in menuParts)
        {
            part.SetActive(false);
        }
        foreach (GameObject part in settingsMenuParts)
        {
            part.SetActive(false);
        }
        foreach (GameObject part in UI)
        {
            part.SetActive(true);
        }
        Time.timeScale = 1;
        StartCoroutine("SetPauseFalse");
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine("SetPauseFalse");
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void RestartScene()
    {
        Time.timeScale = 1;
        StartCoroutine("SetPauseFalse");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        isPaused = false;
    }

    IEnumerator SetPauseFalse()
    {
        yield return new WaitForEndOfFrame();
        isPaused = false;
    }
}
