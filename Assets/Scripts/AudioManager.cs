using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pBulletWallSFX;
    [SerializeField] AudioClip pBulletEnemySFX;

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

    public void pBulletWallCollisionSFX()
    {
        audioSource.PlayOneShot(pBulletWallSFX);
    } 
    public void pBulletEnemyCollisionSFX()
    {
        audioSource.PlayOneShot(pBulletEnemySFX);
    }
}
