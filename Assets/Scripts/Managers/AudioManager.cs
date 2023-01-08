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
    [SerializeField] AudioClip enemyShoot;
    [SerializeField] AudioClip enterBT;
    [SerializeField] AudioClip exitBT;

    [Header("Interpolation")]
    Interpolator lerp;
    public AnimationCurve curve;

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

        lerp = new Interpolator(0.5f);
    }

    private void Start()
    {
        audioSource.Play();
    }

    public void ChangePitch(float pitch)
    {
        float prevPitch = audioSource.pitch;

        lerp.Update(Time.deltaTime);

        if (lerp.IsMaxPrecise)
            lerp.ToMin();

        else if (lerp.IsMinPrecise)
            lerp.ToMax();

        audioSource.pitch = Mathf.Lerp(prevPitch, pitch, curve.Evaluate(lerp.Value));
    }

    public void ChangeVolume(float sliderValue)
    {
        audioSource.volume = sliderValue;
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
    public void EnemyShootSFX()
    {        
        audioSource.PlayOneShot(enemyShoot,0.2f);        
    } 
    public void EnterBTSFX()
    {        
        audioSource.PlayOneShot(enterBT,0.2f);
        //Temporal
        audioSource.pitch = 1f;
        
    }
    public void ExitBTSFX()
    {        
        audioSource.PlayOneShot(exitBT,0.2f);
        //Temporal
        audioSource.pitch = 0.38f;
    }
}
