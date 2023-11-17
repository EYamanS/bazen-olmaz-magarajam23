using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Action OnDoorCloseAutomatically;

    [SerializeField] Animator animator;
    private bool isOpen = false;

    public void interact()
    {
        if (isOpen)
        {
            animator.Play("Close");
            isOpen = false;
        }
        else
        {
            animator.Play("Open");
            isOpen = true;
        }
    }

    public void OnTriggerExit()
    {
        if (isOpen)
        {
            animator.Play("Close");
            isOpen = false;

            OnDoorCloseAutomatically?.Invoke();
        }
    }
}
