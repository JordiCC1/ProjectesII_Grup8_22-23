using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    public AudioClip walkSound;

    void PlayWalk()
    {
        SFXManager.instance.PlayAudio2D(this.transform, walkSound);
    }
}
