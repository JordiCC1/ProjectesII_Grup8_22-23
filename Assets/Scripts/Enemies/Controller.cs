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
            AudioManager.instance.EnemyShootSFX();
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
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player" && !nonPlayerkillable)
            {
                AudioManager.instance.EnemyDeathSFX();
                GameObject ParticleIns = Instantiate(particles, transform.position, Quaternion.identity);
                ParticleIns.GetComponent<ParticleSystem>().Play();
                CinemachineShake.Instance.ShakeCamera(5f, .1f);
                Destroy(gameObject);
            }
            else if (collision.gameObject.tag == "Bullet")
            {
                AudioManager.instance.EnemyDeathSFX();
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
                        //Alarm1.GetComponent<SpriteRenderer>().color = alphaM;
                        //Alarm2.GetComponent<SpriteRenderer>().color = alphaM;
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
                //Alarm1.GetComponent<SpriteRenderer>().color = alphaZ;
                //Alarm2.GetComponent<SpriteRenderer>().color = alphaZ;
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
            yield return new WaitForSeconds(0.1f);
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