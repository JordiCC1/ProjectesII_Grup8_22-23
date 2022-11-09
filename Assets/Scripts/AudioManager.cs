using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip pBulletWallSFX;
    [SerializeField] AudioClip pBulletEnemySFX;
    [SerializeField] AudioClip landingSFX;
    [SerializeField] AudioClip enemyDeathSFX;
    [SerializeField] AudioClip playerDeathSFX;


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

    public void PBulletWallCollisionSFX()
    {
        audioSource.PlayOneShot(pBulletWallSFX);
    } 
    public void PBulletEnemyCollisionSFX()
    {
        audioSource.PlayOneShot(pBulletEnemySFX);
    }

    public void LandingSFX()
    {
        audioSource.PlayOneShot(landingSFX);
    } 

    public void EnemyDeathSFX()
    {
        audioSource.PlayOneShot(enemyDeathSFX);
    } 
    
    public void PlayerDeathSFX()
    {
        audioSource.PlayOneShot(playerDeathSFX);
    }
}
