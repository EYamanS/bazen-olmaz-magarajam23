using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;

public class TimelineArriveTrigger : MonoBehaviour
{
    [SerializeField] Door doorToSubscribe;
    PlayableDirector playableDirector;

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        doorToSubscribe.OnDoorCloseAutomatically += OnDoorCloseAuto;

    }

    private void OnDisable()
    {
        doorToSubscribe.OnDoorCloseAutomatically -= OnDoorCloseAuto;
    }

    private void OnDoorCloseAuto()
    {
        playableDirector.Play();
    }

    public void OnStrangerKnock()
    {
        transform.DOShakePosition(.1f, .5f, 10, 40).OnComplete(() =>
        {
            transform.DOShakePosition(.1f, .5f, 10, 40);
        });
    }
}
