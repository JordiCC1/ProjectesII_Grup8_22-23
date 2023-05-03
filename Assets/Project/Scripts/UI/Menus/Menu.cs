using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    private void Update()
    {
        if(FindObjectOfType<CheckpointMaster>() != null)
        {
            CheckpointMaster checkpointMaster = FindObjectOfType<CheckpointMaster>();
            checkpointMaster.DestroyThis();
        }
    }
    public void ChangeScene(string name)
    {
        SceneManager.LoadScene(name);
        MusicManager.instance.UpdateMusic(name);
        MusicManager.instance.ChangePitch(1.0f);
    }
    public void Exit()
    { 
        Application.Quit();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }    
}
