using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Base : MonoBehaviour
{
    public GameObject interactable;
    public bool canInteract = false;

    void Update()
    {
        if (canInteract)
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
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (IsActive())
                interactable.SetActive(false);
            else
                interactable.SetActive(true);
        }
    }
    bool IsActive()
    {
        return interactable.activeInHierarchy;
    }
}
