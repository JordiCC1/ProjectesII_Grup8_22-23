using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Enemy
{
    public class Controller : MonoBehaviour
    {
        //Shooting range
        [SerializeField] private float Range;
        //Position of the player
        [SerializeField] private GameObject Target;
        //Time to wait to shoot when it sees the player
        [SerializeField] private float waitTime;
        [SerializeField] private float knockedTime;
        //Where it shoots from
        [SerializeField] private Transform Shootpoint;
        //Force of the bullet
        [SerializeField] private float Force;
        //Rate of fire
        [SerializeField] private float FireRate;
        //Particles at dying
        [SerializeField] private GameObject particles;
        //Vector to the player
        private Vector2 Direction;
        //Where it shoots from
        [SerializeField] private GameObject Gun;
        //What it shoots
        [SerializeField] private GameObject bullet;

        //Stun Animation
        private SpriteRenderer sr;
        private float minimum = 0.3f;
        private float maximum = 1.0f;
        private float cyclesPerSecond = 2.0f;
        private float alpha;
        private bool increasing = true;
        private Color srColor;
        private Color maxAlpha;

        //Wait time to shoot the player
        private float nextTimeToFire = 0;

        //Shows when the player is visible        
        /// Esto al tener enemigos hechos lo eliminaremos porque es el interrogante        
        [SerializeField] private GameObject Alarm1;
        [SerializeField] private GameObject Alarm2;
        //Alpha Zero
        private Color alphaZ;
        //Max alpha
        private Color alphaM;
        /// Hasta aqui se podra borrar        

        private bool Detected = false;
        public bool swapped = false;

        private Vector3 originalScale;
        private Vector3 scaleTo;


        void Start()
        {
            Target = GameObject.FindGameObjectWithTag("Player");

            alphaM = Alarm1.GetComponent<SpriteRenderer>().color;
            alphaZ.a = 0f;
            Alarm1.GetComponent<SpriteRenderer>().color = alphaZ;
            Alarm2.GetComponent<SpriteRenderer>().color = alphaZ;

            originalScale = transform.localScale;
            scaleTo = originalScale * 1.35f;
            sr = gameObject.GetComponent<SpriteRenderer>();
            maxAlpha = sr.color;
            srColor = sr.color;
            alpha = maximum;
        }

        void Update()
        {

            Vector2 targetPos = Target.transform.position;
            Direction = targetPos - (Vector2)transform.position;
            RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range, LayerMask.GetMask("Player", "Terrain"));

            StartCoroutine("KnockedTime");

            if (rayInfo)
            {
                if (rayInfo.collider.gameObject.tag == "Player")
                {
                    if (Detected == false)
                    {
                        Alarm1.GetComponent<SpriteRenderer>().color = alphaM;
                        Alarm2.GetComponent<SpriteRenderer>().color = alphaM;
                        Detected = true;
                    }
                }
                else
                {
                    if (Detected == true)
                    {
                        Detected = false;
                        Alarm1.GetComponent<SpriteRenderer>().color = alphaZ;
                        Alarm2.GetComponent<SpriteRenderer>().color = alphaZ;
                    }
                }
            }
            if (Detected && !swapped)
            {

                Gun.transform.up = Direction;
                if (Time.time > nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1 / FireRate;
                    StartCoroutine("WaitToShoot");
                }
            }
            Blink();
        }

        #region Shoot
        void Shoot()
        {
            CinemachineShake.Instance.ShakeCamera(5f, .1f);
            AudioManager.instance.EnemyShootSFX();
            GameObject BulletIns = Instantiate(bullet, Shootpoint.position, Quaternion.identity);
            BulletIns.GetComponent<Rigidbody2D>().AddForce(Direction * Force);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, Range);
        }

        IEnumerator WaitToShoot()
        {
            transform.DOScale(scaleTo, 0.5f);
            yield return new WaitForSeconds(waitTime);  ////fer un yield waituntil swapped o algo similar
            transform.DOScale(originalScale, 0.5f);
            Shoot();
        }
        #endregion

        #region collisions
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player" || collision.gameObject.tag == "Bullet")
            {
                AudioManager.instance.EnemyDeathSFX();
                GameObject ParticleIns = Instantiate(particles, transform.position, Quaternion.identity);
                ParticleIns.GetComponent<ParticleSystem>().Play();
                CinemachineShake.Instance.ShakeCamera(5f, .1f);
                Destroy(gameObject);
            }
        }
        #endregion

        #region Swap
        public void OnSwap()
        {
            Debug.Log("knocked");
            swapped = true;
        }

        IEnumerator KnockedTime()
        {
            yield return new WaitUntil(() => swapped);
            yield return new WaitForSeconds(knockedTime);
            if (swapped)
            {
                swapped = false;
                Debug.Log("finish knocked");
                sr.color = maxAlpha;
            }

        }

        void Blink()
        {
            if (swapped)
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
    }
}