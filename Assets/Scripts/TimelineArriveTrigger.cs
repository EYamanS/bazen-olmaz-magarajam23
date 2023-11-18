using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using DG.Tweening;

public class TimelineArriveTrigger : MonoBehaviour
{
    [SerializeField] Door doorToSubscribe;
    PlayableDirector playableDirector;
    private bool triggered = false;

    private void Awake()
    {
        playableDirector = GetComponent<PlayableDirector>();
    }

    private void OnEnable()
    {
        doorToSubscribe.OnDoorClose += OnDoorCloseAuto;

    }

    private void OnDisable()
    {
        doorToSubscribe.OnDoorClose -= OnDoorCloseAuto;
    }

    private void OnDoorCloseAuto()
    {
        if (!triggered)
        {
            triggered = true;
            StartCoroutine(StrangerSequence());
        }
    }

    private IEnumerator StrangerSequence()
    {
        yield return new WaitForSeconds(8);
        playableDirector.Play();
    }

    public void OnStrangerKnock()
    {
        transform.DOShakePosition(.1f, .1f, 20, 40).OnComplete(() =>
        {
            transform.DOShakePosition(.1f, .1f, 20, 40);
        });
    }
}
