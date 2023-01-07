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

        //Stun Animation
        private SpriteRenderer sr;
        private float minimum = 0.3f;
        private float maximum = 1.0f;
        private float cyclesPerSecond = 2.0f;
        private float alpha;
        private bool increasing = true;
        private Color srColor;
        private Color maxAlpha;

        private bool check = false;

        //Wait time to shoot the player
        private float nextTimeToFire = 0;

        //Shows when the player is visible        
        /// Esto al tener enemigos hechos lo eliminaremos porque es el interrogante        
        //[SerializeField] private GameObject Alarm1;
        //[SerializeField] private GameObject Alarm2;
        //Alpha Zero
        private Color alphaZ;
        //Max alpha
        private Color alphaM;
        /// Hasta aqui se podra borrar        

        public bool isDetected { get; private set; } = false;
        public bool isSwapped { get; private set; } = false;
        public bool isReloaded { get; private set; } = false;

        private Vector3 originalScale;
        private Vector3 scaleTo;

        [HideInInspector] public Vector3 swapPosition;


        void Start()
        {
            target = GameObject.FindGameObjectWithTag("Player");

            //alphaM = Alarm1.GetComponent<SpriteRenderer>().color;
            //alphaZ.a = 0f;
            //Alarm1.GetComponent<SpriteRenderer>().color = alphaZ;
            //Alarm2.GetComponent<SpriteRenderer>().color = alphaZ;

            originalScale = transform.localScale;
            scaleTo = originalScale * 1.35f;
            sr = gameObject.GetComponent<SpriteRenderer>();
            maxAlpha = sr.color;
            srColor = sr.color;
            alpha = maximum;
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

            Blink();
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
            Debug.Log("knocked");
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
                Debug.Log("finish knocked");
                sr.color = maxAlpha;
            }
        }

        void Blink()
        {
            if (isSwapped)
            {
                float t = Time.deltaTime;
                if (alpha >= maximum) increasing = false;
                if (alpha <= minimum) increasing = true;
                alpha = increasing ? alpha += t * cyclesPerSecond * 2 : alpha -= t * cyclesPerSecond;
                srColor.a = alpha;
                sr.color = srColor;
            }
        }
        #endregion


        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)direction * range);
        }
    }
}