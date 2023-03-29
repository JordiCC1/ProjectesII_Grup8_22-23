using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public float lifeTime;
    public GameObject particles;
    public Rigidbody2D rb;
    Vector2 force;

    void Start()
    {        
        StartCoroutine(WaitThenDie());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject ParticleIns = Instantiate(particles, transform.position, Quaternion.identity);
        ParticleIns.GetComponent<ParticleSystem>().Play();
        Destroy(gameObject);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Shield"))
        {
            rb.AddForce(-force * 2);
            SetForce(-force);
            //Destroy(gameObject);

            //objCollided = Instantiate(bullet, aux.transform.position, Quaternion.identity);
            //objCollided.GetComponent<Rigidbody2D>().AddForce(-aux.GetComponent<enemyBullet>().GetForce());
            //objCollided.GetComponent<enemyBullet>().SetForce(-aux.GetComponent<enemyBullet>().GetForce());
        }
    }
    IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }

    public void SetForce(Vector2 f)
    {
        force = f;
    }

    public Vector2 GetForce()
    {
        return force;
    }
}
