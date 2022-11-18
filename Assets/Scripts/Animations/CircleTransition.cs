using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleTransition : MonoBehaviour
{
    private Canvas _canvas;
    private Image translucentScreen;

    private void Awake()
    {
        _canvas = GetComponent<Canvas>();
        translucentScreen = GetComponentInChildren<Image>();
    }

    private void Start()
    {
        DrawTranslucentScreen();
    }
    // Update is called once per frame
    void Update()
    {
        
    }

    private void DrawTranslucentScreen()
    {
        var canvasRect = _canvas.GetComponent<RectTransform>().rect;
        var canvasWidth = canvasRect.width;
        var canvasHeight = canvasRect.height;

        translucentScreen.rectTransform.sizeDelta = new Vector2(canvasWidth, canvasHeight);
    }
}
