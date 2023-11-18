using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class DialogWindow : SingletonComponent<DialogWindow>
{
    public List<DialogueStep> DialogueObjects = new List<DialogueStep>();
    AudioSource writingAudioSource;

    private void OnEnable()
    {
        MysteryCube.Instance.doMove = false;
        
        if (DialogueObjects.Count == 0)
        {
            gameObject.SetActive(false);
        }
        
    }

    private void OnDisable()
    {
        MysteryCube.Instance.doMove = true;
    }

    protected override void Awake()
    {
        base.Awake();
        writingAudioSource = GetComponent<AudioSource>();
        //StartCoroutine(WritingEnumerator());
    }
    private void Start()
    {
        DialogueObjects[0].gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void AdvanceDialogue(bool withEnterKey = false)
    {
        if (DialogueObjects[0].DoClose())
        {
            gameObject.SetActive(false);    
        }

        if (!withEnterKey)
        {
            if (DialogueObjects[0] is InputTakerDialogue)
                return;

            if (DialogueObjects[0].CanSkip())
            {
                SkipDialogue();
            }
        }
        else
        {
            if (DialogueObjects[0].CanSkip())
            {
                SkipDialogue();
            }
        }
    }

    private void SkipDialogue()
    {
        DialogueObjects[0].gameObject.SetActive(false);
        DialogueObjects.RemoveAt(0);
        if (DialogueObjects.Count > 0)
        {
            DialogueObjects[0].gameObject.SetActive(true);
            StartCoroutine(WritingEnumerator());
        }
        else
        {
            DialogWindow.Instance.gameObject.SetActive(false);
        }
    }

    private IEnumerator WritingEnumerator()
    {
        float volume = 0;

        writingAudioSource.Play();

        var increateTween = DOTween.To(() =>
            volume, newVolume => volume = newVolume,
            1,
            .4f).OnUpdate(
                () =>
                writingAudioSource.volume = volume).OnComplete(
            () =>
            {

            });


        while (!DialogueObjects[0].writingComplete)
        {
            yield return null;
        }


        var lowerTween = DOTween.To(() =>
            volume, newVolume => volume = newVolume,
            0,
            .4f).OnUpdate(
                () =>
                writingAudioSource.volume = volume).OnComplete(
            () =>
            {
                writingAudioSource.Pause();
            });
    }
    
}
