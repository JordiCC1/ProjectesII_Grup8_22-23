using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float lifeTime;
    public ParticleSystem ps;
    void Start()
    {
        //ps = GetComponent<ParticleSystem>();
        StartCoroutine(WaitThenDie());
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        ps.Play();
        Destroy(gameObject);
    }
    IEnumerator WaitThenDie()
    {
        yield return new WaitForSeconds(lifeTime);
        Destroy(gameObject);
    }
}
