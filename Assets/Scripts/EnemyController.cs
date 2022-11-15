using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class EnemyController : MonoBehaviour
{
    

    public float Range;
    public Transform Target;
    bool Detected = false;

    //Time to wait to shoot the enemies
    public float waitTime;

    public GameObject particles;

    Vector2 Direction;
    public GameObject Gun;
    public GameObject bullet;

    //Shows when the player is visible
    public GameObject Alarm1;
    public GameObject Alarm2;
    //Alpha Zero
    Color alphaZ;
    //Max alpha
    Color alphaM;

    public float FireRate;
    float nextTimeToFire = 0;
    public Transform Shootpoint;
    public float Force;

    void Start()
    {
        alphaM = Alarm1.GetComponent<SpriteRenderer>().color;
        alphaZ.a = 0f;
        Alarm1.GetComponent<SpriteRenderer>().color = alphaZ;
        Alarm2.GetComponent<SpriteRenderer>().color = alphaZ;

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
                    StartCoroutine("PlayerDetected");
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
                Shoot();
            }
        }
    }
    void Shoot()
    {
        AudioManager.instance.EnemyShootSFX();
        GameObject BulletIns = Instantiate(bullet, Shootpoint.position, Quaternion.identity);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(Direction * Force);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }

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

    IEnumerator PlayerDetected()
    {
        yield return new WaitForSeconds(waitTime);
        Detected = true;
    }
}



