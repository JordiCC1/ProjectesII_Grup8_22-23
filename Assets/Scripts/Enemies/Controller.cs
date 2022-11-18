using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Enemy
{
    public class Controller : MonoBehaviour
    {
        //Shooting range
        [SerializeField]private float Range;
        //Position of the player
        [SerializeField] private Transform Target;        
        //Time to wait to shoot when it sees the player
        [SerializeField] private float waitTime;
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

        private Vector3 originalScale;
        private Vector3 scaleTo;        
       

        void Start()
        {
            alphaM = Alarm1.GetComponent<SpriteRenderer>().color;
            alphaZ.a = 0f;
            Alarm1.GetComponent<SpriteRenderer>().color = alphaZ;
            Alarm2.GetComponent<SpriteRenderer>().color = alphaZ;

            originalScale = transform.localScale;
            scaleTo = originalScale * 1.35f;
        }

        void Update()
        {
            Vector2 targetPos = Target.position;
            Direction = targetPos - (Vector2)transform.position;
            RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range, LayerMask.GetMask("Player", "Terrain"));

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
            if (Detected)
            {

                Gun.transform.up = Direction;
                if (Time.time > nextTimeToFire)
                {
                    nextTimeToFire = Time.time + 1 / FireRate;
                    StartCoroutine("WaitToShoot");
                }
            }
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
            yield return new WaitForSeconds(waitTime);
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
    }
}



