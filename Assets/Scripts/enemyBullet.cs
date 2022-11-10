using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBullet : MonoBehaviour
{
    public float lifeTime;
    public GameObject particles;


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
}
