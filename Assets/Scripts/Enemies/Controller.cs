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
        private Vector2 BulletDirection;
        //Where it shoots from
        [SerializeField] private GameObject Gun;
        //What it shoots
        [SerializeField] private GameObject bullet;

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

        [HideInInspector] public Vector3 swapPosition;


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
            Direction = (targetPos - (Vector2)transform.position).normalized;
            BulletDirection = targetPos - (Vector2)transform.position;
            RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range, LayerMask.GetMask("Player", "Terrain"));

            if (swapped)
            {
                if (check == true)
                {
                    StopCoroutine("KnockedTime");
                    check = false;
                }
                StartCoroutine("KnockedTime");                
            }

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
                    if (Detected && !swapped)
                    {
                        Gun.transform.up = Direction;
                        if (Time.time > nextTimeToFire)
                        {
                            nextTimeToFire = Time.time + 1 / FireRate;
                            StartCoroutine("WaitToShoot");
                        }
                    }
                }                
            }
            else if (Detected)
            {                
                Detected = false;
                Alarm1.GetComponent<SpriteRenderer>().color = alphaZ;
                Alarm2.GetComponent<SpriteRenderer>().color = alphaZ;
            }
            Blink();
        }

        #region Shoot
        void Shoot()
        {
            if (!Detected)
                return;
            CinemachineShake.Instance.ShakeCamera(5f, .1f);
            AudioManager.instance.EnemyShootSFX();
            GameObject BulletIns = Instantiate(bullet, Shootpoint.position, Quaternion.identity);
            BulletIns.GetComponent<Rigidbody2D>().AddForce(BulletDirection * Force);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireSphere(transform.position, Range);
        }

        IEnumerator WaitToShoot()
        {
            transform.DOScale(scaleTo, 0.5f);
            yield return new WaitForSeconds(waitTime); 
            transform.DOScale(originalScale, 0.5f);
            if (!swapped)            
                Shoot();              
            
        }
        #endregion

        #region collisions
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
            else if (collision.gameObject.tag == "Bullet" && nonPlayerkillable)
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

        private void OnDrawGizmos()
        {
            Gizmos.DrawLine(transform.position, transform.position + (Vector3)Direction * Range);
        }
    }
}