using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Interaction : MonoBehaviour
{
    public GameObject textBox;

    void Update()
    {
       // if(Input.GetKeyDown(KeyCode.F) && IsActive())
       //     textBox.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && Input.GetKeyDown(KeyCode.F))
        {
            Interact();
        }
    }

    void Interact()
    {
        if (!IsActive())
            textBox.SetActive(true);
    }

    bool IsActive()
    {
        return textBox.activeInHierarchy;
    }
}