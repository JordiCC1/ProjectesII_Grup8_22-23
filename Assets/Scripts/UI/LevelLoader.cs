using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;

    [HideInInspector] public bool load = false;
    // Update is called once per frame
    void Update()
    {
        if(load)
            transition.SetTrigger("Start");
    }

  
}
