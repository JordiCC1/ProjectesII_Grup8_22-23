using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointMaster : MonoBehaviour
{
    private static CheckpointMaster instance;

    public Vector2 lastCheckPointPos;

    private void Awake()
    {
        if(instance == null) 
        { 
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void DestroyThis()
    {
        Destroy(gameObject);
    }

}
