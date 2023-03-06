using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable_Base : MonoBehaviour
{
    public GameObject interactable;
    public GameObject interactPrompt;
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
            interactPrompt.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            canInteract = false;
            interactPrompt.SetActive(false);
        }
    }
    private void HandleInput()
    {
        if (Input.GetButtonDown("Interact"))
        {
            if (IsActive())
            {
                interactable.SetActive(false);
                interactPrompt.SetActive(true);
            }
            else if (canInteract)
            {
                interactable.SetActive(true);
                interactPrompt.SetActive(false);
            }
        }
    }
    bool IsActive()
    {
        return interactable.activeInHierarchy;
    }
}
