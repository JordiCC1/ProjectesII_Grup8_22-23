using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Base : MonoBehaviour
{
    public GameObject interactable;
    public bool canInteract = false;

    void Update()
    {
        HandleInput();
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
        if (Input.GetButtonDown("Interact"))
        {
            if (IsActive())
                interactable.SetActive(false);
            else if (canInteract)
                interactable.SetActive(true);
        }
    }
    bool IsActive()
    {
        return interactable.activeInHierarchy;
    }
}
