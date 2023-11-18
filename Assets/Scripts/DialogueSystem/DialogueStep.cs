using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public abstract class DialogueStep : MonoBehaviour
{
    public TMP_Text textToTypeWrite;
    private float typeSpeed = 15f;

    [SerializeField] GameObject[] RevealOnAnimationComplete;

    public bool writingComplete = false;

    public abstract bool CanSkip();

    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public virtual void OnEnable()
    {
        if (TryGetComponent<TMP_Text>(out var textObject))
        {
            textObject.text = textObject.text.Replace("{PlayerName}", PlayerPrefs.GetString("PlayerName"));
        }

        if (textToTypeWrite != null)
        {
                string text = "";
                
                var textTween = DOTween.To(() =>
                    text, newText => text = newText,
                    textToTypeWrite.text,
                    textToTypeWrite.text.Length / typeSpeed).OnUpdate(
                        () =>
                        textToTypeWrite.text = text).OnComplete(
                    () =>
                    {
                        if (RevealOnAnimationComplete != null && RevealOnAnimationComplete.Length > 0)
                        {
                            for (int i = 0; i < RevealOnAnimationComplete.Length; i++)
                            {
                                RevealOnAnimationComplete[i].SetActive(true);
                            }
                        }
                        writingComplete = true;
                    });
        }
        else
        {
            writingComplete = true;
        }
    }

    private void OnDisable()
    {
        writingComplete = false;
    }

    public virtual bool DoClose()
    {
        return false;
    }
}