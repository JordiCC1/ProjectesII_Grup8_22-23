using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceDeactivate : MonoBehaviour
{
    public GameObject target;

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(target.activeInHierarchy)
            target.SetActive(false);
    }
}
