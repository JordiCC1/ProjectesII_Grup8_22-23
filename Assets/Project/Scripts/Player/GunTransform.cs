using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunTransform : MonoBehaviour
{
    private float timer;
    public float timeBetweenFire;

    public bool canFire;

    private Camera mainCam;

    private Vector3 mousePos;
    [SerializeField] PauseMenu pauseMenu;
    public Transform playerTransform;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (!pauseMenu.isPaused)
        {
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            if (!canFire)
            {
                timer += Time.deltaTime;
                if (timer > timeBetweenFire)
                {
                    canFire = true;
                    timer = 0;
                }
            }
        }
    }
}
