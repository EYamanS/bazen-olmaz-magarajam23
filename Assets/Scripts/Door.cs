using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    public Action OnDoorClose;
    public Action OnDoorOpen;

    [SerializeField] AudioSource doorAudioSource;
    [SerializeField] AudioClip doorOpenSound;
    [SerializeField] AudioClip doorCloseSound;

    public bool locked;

    public void Lock() => locked = true;
    public void UnLock() => locked = false;

    [SerializeField] Animator animator;
    private bool isOpen = false;

    public void interact()
    {
        if (locked) return;

        if (isOpen)
        {
            animator.Play("Close");
            doorAudioSource.clip = doorCloseSound;
            doorAudioSource.Play();
            isOpen = false;
            OnDoorClose?.Invoke();
        }
        else
        {
            animator.Play("Open");
            doorAudioSource.clip = doorOpenSound;
            doorAudioSource.Play();
            isOpen = true;
            OnDoorOpen?.Invoke();
        }
    }

    public void OnTriggerExit()
    {
        if (isOpen)
        {
            animator.Play("Close");
            isOpen = false;
            doorAudioSource.clip = doorCloseSound;
            doorAudioSource.Play();
            OnDoorClose?.Invoke();
        }
    }
}
