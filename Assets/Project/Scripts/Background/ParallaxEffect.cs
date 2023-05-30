using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private float spriteWidth, startPosition;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        startPosition = transform.position.x;
    }

    void LateUpdate()
    {

        float xIncrease = (cameraTransform.position.x - previousCameraPosition.x) * parallaxMultiplier;

        transform.Translate(new Vector3(xIncrease, 0, 0));
        previousCameraPosition = cameraTransform.position;
    }
}
