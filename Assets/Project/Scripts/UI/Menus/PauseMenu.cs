using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PauseMenu : MonoBehaviour
{
    public static PauseMenu instance;

    //public GameObject pauseMenu;
    public GameObject[] menuParts;
    public GameObject[] settingsMenuParts;
    public bool isPaused;
    public string pauseButton = "Pause";
    public GameObject[] UI;

    private CheckpointMaster cm;
    private ScreenWipe sw;
    private Scene currentScene;

    [SerializeField] string mainMenu;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

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

        if (Input.GetButtonDown(pauseButton) && !(SceneManager.GetActiveScene().name == mainMenu))
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
        if (SceneManager.GetActiveScene().name == mainMenu)
            ResumeGame();

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
        MusicManager.instance.UpdateMusic("Main Menu");
        MusicManager.instance.ChangePitch(1.4f);  
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

    public void Mute()
    {
        SFXManager.instance.Mute();
    }
}
