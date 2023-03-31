using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkSound : MonoBehaviour
{
    public AudioClip walkSound;

    void PlayWalk()
    {
        AudioManager.PlayAudio2D(this.transform, walkSound);
    }
}
