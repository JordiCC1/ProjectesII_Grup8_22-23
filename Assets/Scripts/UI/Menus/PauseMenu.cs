using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    //public GameObject pauseMenu;
    public GameObject[] menuParts;
    public bool isPaused;
    public string pauseButton = "Pause";
    public GameObject[] UI;
    [SerializeField] bool gamePaused;

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
        gamePaused = true;

        //pauseMenu.SetActive(true);
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
        gamePaused = false;

        //pauseMenu.SetActive(false);
        foreach (GameObject part in menuParts)
        {
            part.SetActive(false);
        }
        foreach (GameObject part in UI)
        {
            part.SetActive(true);
        }
        Time.timeScale = 1;
        isPaused = false;
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;        
        SceneManager.LoadScene("Main Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
