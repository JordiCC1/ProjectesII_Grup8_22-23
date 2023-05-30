using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [SerializeField] AudioSource audioSource;
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
    }

    public void PlayAudio2D(Transform root, AudioClip clip, bool loop = false)
    {
        if (audioSource != null)
        {
            if (loop)
            {
                audioSource.loop = true;
                audioSource.clip= clip;
                audioSource.Play();
            }
            else
                audioSource.PlayOneShot(clip);
            StartCoroutine("ResetPitch");
        }
        else
            Debug.Log("AudioSource = null");
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

   public void Mute() {
        audioSource.mute = !audioSource.mute;
    }
}
