using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlesManager : MonoBehaviour
{

    public static ParticlesManager instance;

    [SerializeField] GameObject enemyBulletparticles;
    [SerializeField] GameObject enemyParticles;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    public void enemyBulletPS(Vector3 pos)
    {
        GameObject ParticleIns = Instantiate(enemyBulletparticles, pos, Quaternion.identity);
        ParticleIns.GetComponent<ParticleSystem>().Play();
    }
}
