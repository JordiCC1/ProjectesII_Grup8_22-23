using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenWipe : MonoBehaviour
{

    public static ScreenWipe instance;

    [SerializeField][Range(0.1f, 3f)] private float wipeSpeed = 1f;
    private Image image;

    private enum WipeMode { NotBlocked, WipingToNotBlocked, Blocked, WipingToBlocked }

    private WipeMode wipeMode = WipeMode.NotBlocked;

    private float wipeProgress;

    public bool isDone { get; private set; }    

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
        image = GetComponentInChildren<Image>();
        isDone = true;
    }

    public void ToggleWipe(bool blockScreen)
    {
        isDone = false;
        if (blockScreen)
        {
            wipeMode = WipeMode.WipingToBlocked;
        }
        else
        {
            wipeMode = WipeMode.WipingToNotBlocked;
        }
    }

    private void Update()
    {       

        switch (wipeMode)
        {
            case WipeMode.WipingToBlocked:
                WipeToBlocked();
                break;
            case WipeMode.WipingToNotBlocked:
                WipeToNotBlocked();
                break;
            case WipeMode.Blocked:
                StartCoroutine("WaitAndWipe");
                break;
        }
        
    }

    private void WipeToBlocked()
    {
        wipeProgress += Time.deltaTime * (1f / wipeSpeed);
        image.fillAmount = wipeProgress;

        if (wipeProgress >= 1f)
        {
            //isDone = true;
            wipeMode = WipeMode.Blocked;
        }
    }

    private void WipeToNotBlocked()
    {
        wipeProgress -= Time.deltaTime * (1f / wipeSpeed);
        image.fillAmount = wipeProgress;

        if (wipeProgress <= 0f)
        {
            isDone = true;
            wipeMode = WipeMode.NotBlocked;
        }
    }

    private IEnumerator WaitAndWipe() 
    {
        yield return new WaitForSeconds(1f);
        wipeMode = WipeMode.WipingToNotBlocked;
    }

    [ContextMenu("Block")]
    private void Block()
    {
        ToggleWipe(true);
    }

    [ContextMenu("Clear")]
    private void Clear()
    {
        ToggleWipe(false);
    }

    public void DestroyThis()
    {
        Destroy(gameObject);
    }
}