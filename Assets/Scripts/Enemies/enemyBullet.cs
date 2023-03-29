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
