using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseDisappear : MonoBehaviour
{
    public PauseMenu pM;

    // Update is called once per frame
    void Update()
    {
        if(pM.isPaused)
            gameObject.SetActive(false);
    }
}
