using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

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

    public static AudioSource PlayAudio2D(Transform root, AudioClip clip, bool DestroyOnFinish = true)
    {
        AudioSource source = Instantiate(new GameObject("AudioObject"), root).AddComponent<AudioSource>();
        source.clip = clip;
        source.Play();
        if (DestroyOnFinish)
            Destroy(source.gameObject, clip.length + 1f);
        return source;
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

    public void ChangeVolume()
    {
        audioSource.volume = slider.value;
    }

    public float GetVolume()
    {
        return audioSource.volume;
    }

}
