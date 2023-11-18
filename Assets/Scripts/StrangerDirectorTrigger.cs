using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class StrangerDirectorTrigger : MonoBehaviour
{
    [SerializeField] Door doorToTriggerFrom;
    [SerializeField] PlayableDirector directorToPlay;
    [SerializeField] AudioSource knockAudioSource;

    private bool triggered = false;

    private void OnEnable()
    {
        doorToTriggerFrom.OnDoorOpen += OnTriggerDoorOpen;
    }

    private void OnDisable()
    {
        doorToTriggerFrom.OnDoorOpen -= OnTriggerDoorOpen;
    }

    private void OnTriggerDoorOpen()
    {
        if (!triggered)
        {
            triggered = true;
            directorToPlay.Play();
        }
    }

    public void PlayKnockSound()
    {
        knockAudioSource.Play();
    }
}
