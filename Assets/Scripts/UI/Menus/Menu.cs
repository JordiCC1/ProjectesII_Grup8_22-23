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
