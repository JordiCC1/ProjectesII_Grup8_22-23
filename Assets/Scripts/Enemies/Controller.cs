using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Enemy
{
    public class Controller : MonoBehaviour
    {
        [Header("Gun")]
        [SerializeField] private GameObject gun;
        //Where it shoots from
        [SerializeField] private Transform shootPoint;
        [SerializeField] private float range = 17;
        [SerializeField] private float waitTime = 0.4f;
        [SerializeField] private float knockedTime = 2;
        [SerializeField] private float bulletForce = 100;
        [SerializeField] private float fireRate = 1;

        [Header("Target")]
        [SerializeField] private GameObject bullet;
        public GameObject target { get; private set; }
        //Vector to the player
        private Vector2 direction;
        private Vector2 bulletDirection;

        //Particles at dying
        [SerializeField] private GameObject particles;

        //Make non Player killable enemy
        [SerializeField] private bool nonPlayerkillable = false;

        private bool check = false;

        //Wait time to shoot the player
        private float nextTimeToFire = 0;     

        public bool isDetected { get; private set; } = false;
        public bool isSwapped { get; private set; } = false;
        public bool isReloaded { get; private set; } = false;

        private Vector3 originalScale;
        private Vector3 scaleTo;

        [HideInInspector] public Vector3 swapPosition;

        [Header("Audio")]
        public AudioClip enemyShoot;
        public AudioClip enemyDeath;

        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player");

            originalScale = transform.localScale;
            scaleTo = originalScale * 1.35f;
        }

        void Update()
        {
            Vector2 targetPos = target.transform.position;
            direction = (targetPos - (Vector2)transform.position).normalized;
            bulletDirection = targetPos - (Vector2)transform.position;
            RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, direction, range, LayerMask.GetMask("Player", "Terrain"));


            if (isSwapped)
            {
                if (check == true)
                {
                    StopCoroutine("KnockedTime");
                    check = false;
                }
                StartCoroutine("KnockedTime");
            }

            BulletCollision(rayInfo);
        }

        #region Shoot
        void Shoot()
        {
            isReloaded = true;
            CinemachineShake.Instance.ShakeCamera(5f, .1f);
            AudioManager.instance.PlayAudio2D(this.transform, enemyShoot);
            GameObject BulletIns = Instantiate(bullet, shootPoint.position, Quaternion.identity);
            BulletIns.GetComponent<Rigidbody2D>().AddForce(bulletDirection * bulletForce);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, range);
        }

        IEnumerator WaitToShoot()
        {
            transform.DOScale(scaleTo, 0.5f);
            isReloaded = false;
            yield return new WaitForSeconds(waitTime);
            transform.DOScale(originalScale, 0.5f);
            if (!isSwapped && isDetected)
                Shoot();

        }
        #endregion

        #region Collisions        

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag == "Player" && !nonPlayerkillable)
            {
                AudioManager.instance.PlayAudio2D(this.transform, enemyDeath);
                GameObject ParticleIns = Instantiate(particles, transform.position, Quaternion.identity);
                ParticleIns.GetComponent<ParticleSystem>().Play();
                CinemachineShake.Instance.ShakeCamera(5f, .1f);
                Destroy(gameObject);
            }
            else if (col.gameObject.tag == "Bullet")
            {
                AudioManager.instance.PlayAudio2D(this.transform, enemyDeath);
                GameObject ParticleIns = Instantiate(particles, transform.position, Quaternion.identity);
                ParticleIns.GetComponent<ParticleSystem>().Play();
                CinemachineShake.Instance.ShakeCamera(5f, .1f);
                Destroy(gameObject);
            }
            else if (col.gameObject.tag == "Trap")
            {
                AudioManager.instance.PlayAudio2D(this.transform, enemyDeath);
                GameObject ParticleIns = Instantiate(particles, transform.position, Quaternion.identity);
                ParticleIns.GetComponent<ParticleSystem>().Play();
                CinemachineShake.Instance.ShakeCamera(5f, .1f);
                Destroy(gameObject);
            }
        }

        void BulletCollision(RaycastHit2D rayInfo)
        {
            if (rayInfo)
            {
                if (rayInfo.collider.gameObject.tag == "Player")
                {

                    if (isDetected == false)
                    {
                        isDetected = true;
                    }
                    if (isDetected && !isSwapped)
                    {
                        gun.transform.up = direction;
                        if (Time.time > nextTimeToFire)
                        {
                            nextTimeToFire = Time.time + 1 / fireRate;
                            StartCoroutine("WaitToShoot");
                        }
                    }
                }
            }
            else if (isDetected)
            {
                isDetected = false;
            }
        }

        #endregion

        #region Swap
        public void OnSwap()
        {
            isSwapped = true;
            check = true;
        }
        public void SwapAnimation(Vector3 target)
        {
            Tween t;
            gameObject.GetComponent<Collider2D>().enabled = false;
            t = DOTween.To(() => gameObject.transform.position, x => gameObject.transform.position = x, target, 0.2f).SetEase(Ease.InOutQuad);
            StartCoroutine("ReturnCollider");

        }

        IEnumerator ReturnCollider()
        {
            yield return new WaitForSeconds(0.2f);
            gameObject.GetComponent<Collider2D>().enabled = true;
        }

        IEnumerator KnockedTime()
        {
            yield return new WaitForSeconds(knockedTime);

            if (isSwapped)
            {
                isSwapped = false;
            }
        }
        #endregion


        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * range);
        }
    }
}