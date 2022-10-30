using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float Range;
    public Transform Target;
    bool Detected = false;

    Vector2 Direction;
    public GameObject Gun;
    public GameObject bullet;
    
    //Shows when the player is visible
    public GameObject Alarm;
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
        alphaM = Alarm.GetComponent<SpriteRenderer>().color;
        alphaZ.a = 0f;
        Alarm.GetComponent<SpriteRenderer>().color = alphaZ;
    }
    
    void Update()
    {
        Vector2 targetPos = Target.position;
        Direction = targetPos - (Vector2)transform.position;
        RaycastHit2D rayInfo = Physics2D.Raycast(transform.position, Direction, Range);

        if (rayInfo)
        {
            if (rayInfo.collider.gameObject.tag == "Player")
            {
                if (Detected == false)
                {
                    Detected = true;
                    Alarm.GetComponent<SpriteRenderer>().color = alphaM;
                }
            }
            else
            {
                if (Detected == true)
                {
                    Detected = false;
                    Alarm.GetComponent<SpriteRenderer>().color = alphaZ;
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
        GameObject BulletIns = Instantiate(bullet, Shootpoint.position, Quaternion.identity);
        BulletIns.GetComponent<Rigidbody2D>().AddForce(Direction * Force);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, Range);
    }
}
