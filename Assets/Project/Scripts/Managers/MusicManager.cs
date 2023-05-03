using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    [SerializeField] AudioSource audioSource;
    [SerializeField] Slider slider;
    [SerializeField] string MenuScene;
    string manuScene;

    [Header("Music Clips")]
    [SerializeField] AudioClip menuMusic;
    [SerializeField] AudioClip gameMusic;

    [Header("Interpolation")]
    Interpolator lerp;
    public AnimationCurve curve;

    bool sceneChanged = false;

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

    private void Update()
    {

        if (SceneManager.GetActiveScene().name == MenuScene)
        {
            audioSource.clip = menuMusic;
            sceneChanged = false;
        }
        else
        {
            audioSource.clip = gameMusic;
            sceneChanged = true;
        }

        if (sceneChanged)
        {
            audioSource.Play();
            sceneChanged = false;

        }
    }

    private void LateUpdate()
    {

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
