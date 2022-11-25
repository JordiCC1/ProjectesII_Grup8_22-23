using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    public class Gun : MonoBehaviour
    {
        private Camera mainCam;
        private Vector3 mousePos;
        public GameObject bullet;
        public Transform bulletTransform;
        public bool canFire;
        private float timer;
        public float timeBetweenFire;

        void Start()
        {
            mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }

        void Update()
        {

            /*if (transform.localScale.x > 0)
            {*/
            mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 rotation = mousePos - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);
            /*}
            else if (transform.localScale.x < 0)
            {
                mousePos = mainCam.ScreenToWorldPoint(-Input.mousePosition);
                Vector3 rotation = mousePos + transform.position;
                float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, -rotZ);
            }*/


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

        public void Shoot()
        {
            if (Input.GetMouseButtonUp(0) && canFire)
            {
                Instantiate(bullet, bulletTransform.position, Quaternion.identity, transform.parent);
                canFire = false;
            }
        }
    }
}
