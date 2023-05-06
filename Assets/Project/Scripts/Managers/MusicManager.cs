using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip menuClip;
    [SerializeField] AudioClip gameClip;
    [SerializeField] string menu;

    [SerializeField] Slider slider;

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
        slider.value = audioSource.volume;
        slider.onValueChanged.AddListener(ChangeVolume);
        audioSource.loop = true;
        audioSource.Play();
    }

    public void UpdateMusic(string nextScene)
    {
        if(nextScene==menu)
        {
            audioSource.clip = menuClip;
        }
        else
        {
            audioSource.clip = gameClip;
        }
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

    public void ChangeVolume(float volume)
    {
        audioSource.volume = volume;
    }

    public void Mute()
    {
        audioSource.mute = !audioSource.mute;
    }
}
