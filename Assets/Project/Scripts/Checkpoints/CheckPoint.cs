using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{

    private CheckpointMaster cm;

    private void Start()
    {
        cm = GameObject.FindGameObjectWithTag("CM").GetComponent<CheckpointMaster>();
    }
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if(coll.CompareTag("Player"))
        {
            cm.lastCheckPointPos = transform.position;
        }
    }
}
