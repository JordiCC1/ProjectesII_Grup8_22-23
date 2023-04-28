using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Player
{
    public class Gun : MonoBehaviour
    {
        public GameObject bullet;
        public Transform bulletTransform;
        [SerializeField] private GunTransform gT;

        public void Shoot()
        {
            if (Input.GetMouseButtonUp(0) && gT.canFire)
            {
                Instantiate(bullet, bulletTransform.position, Quaternion.identity, transform.parent);
                gT.canFire = false;
            }
        }
    }
}
