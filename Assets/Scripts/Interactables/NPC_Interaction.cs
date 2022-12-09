using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Interaction : MonoBehaviour
{
    public GameObject textBox;

    public bool canInteract = false;

    void Update()
    {
        if(canInteract)
        {
            HandleInput();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
        }
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if(IsActive())
                textBox.SetActive(false);
            else
                textBox.SetActive(true);
        }
    }

    bool IsActive()
    {
        return textBox.activeInHierarchy;
    }
}