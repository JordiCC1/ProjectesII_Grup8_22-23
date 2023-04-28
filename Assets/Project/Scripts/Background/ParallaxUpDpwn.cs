using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxUpDpwn : MonoBehaviour
{
    [SerializeField] private float parallaxMultiplier;
    [SerializeField] private float parallaxMultiplierUP;

    private Transform cameraTransform;
    private Vector3 previousCameraPosition;
    private float spriteWidth, startPosition, spriteHeight;

    void Start()
    {
        cameraTransform = Camera.main.transform;
        previousCameraPosition = cameraTransform.position;
        spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
        spriteHeight = GetComponent<SpriteRenderer>().bounds.size.y;
        startPosition = transform.position.x;
    }

    void LateUpdate()
    {

        float xIncrease = (cameraTransform.position.x - previousCameraPosition.x) * parallaxMultiplier;
        float yIncrease = (cameraTransform.position.y - previousCameraPosition.y) * parallaxMultiplierUP;
        float moveAmount = cameraTransform.position.x * (1 - parallaxMultiplier);
        float moveAbove = cameraTransform.position.y * (1 - parallaxMultiplierUP);

        transform.Translate(new Vector3(xIncrease, yIncrease, 0));
        previousCameraPosition = cameraTransform.position;

        if (moveAmount > startPosition + spriteWidth)
        {
            transform.Translate(new Vector3(spriteWidth, 0, 0));
            startPosition += spriteWidth;
        }
        else if (moveAmount < startPosition - spriteWidth)
        {
            transform.Translate(new Vector3(-spriteWidth, 0, 0));
            startPosition -= spriteWidth;
        }
        if (moveAbove > startPosition + spriteHeight)
        {
            transform.Translate(new Vector3(0, spriteHeight, 0));
            startPosition += spriteHeight;
        }
        else if (moveAbove < startPosition - spriteHeight)
        {
            transform.Translate(new Vector3(0, -spriteHeight, 0));
            startPosition -= spriteHeight;
        }
    }
}
