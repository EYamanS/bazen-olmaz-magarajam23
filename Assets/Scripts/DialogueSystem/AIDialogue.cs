using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AIDialogue : MonoBehaviour
{
    [SerializeField] TMP_InputField inputToGetMessage;
    [SerializeField] TMP_Text textToChange;
    [SerializeField] OpenAITest aiToTalkTo;

    [SerializeField] GameObject[] revealOnWritingComplete;

    public bool complete = false;
    public bool receivedMessage = false;

   

    private void ProceedDialogue(string message)
    {
        aiToTalkTo.SendDialog(message);
    }

    private void OnEnable()
    {
        aiToTalkTo.onRecieveMessage += OnRecieveMessage;
        ProceedDialogue("Hey Kid! What are you doing here??");
    }


    private void OnDisable()
    {
        aiToTalkTo.onRecieveMessage -= OnRecieveMessage;

    }

    private void Update()
    {
        if (complete)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                complete = false;
                receivedMessage = false;
                inputToGetMessage.gameObject.SetActive(false);
                textToChange.text = "";

                ProceedDialogue(inputToGetMessage.text);
            }
        }
    }

    private void OnRecieveMessage(string obj)
    {
        receivedMessage = true;
        StopAllCoroutines();

        var text = "";
        var textTween = DOTween.To(() =>
            text, newText => text = newText,
            obj,
            obj.Length / 20f).OnUpdate(
                () =>
                textToChange.text = text).OnComplete(
            () =>
            {
                complete = true;

                if (obj.Contains("[CANCEL]"))
                {
                    textToChange.transform.parent.gameObject.SetActive(false);

                    StartCoroutine(SaveKidSequence());

                }
                else if (obj.Contains("[FALL]"))
                {
                    textToChange.transform.parent.gameObject.SetActive(false);

                    StartCoroutine(KidFallSequence());
                }
                else
                {
                    inputToGetMessage.gameObject.SetActive(true);

                }

                for (int i = 0; i < revealOnWritingComplete.Length; i++)
                {
                    revealOnWritingComplete[i].SetActive(true);
                }
            });
    }

    private IEnumerator SaveKidSequence()
    {
        yield return new WaitForSeconds(1.5f);

        SceneTransitor.Instance.GoToNextScene();
    }

    private IEnumerator KidFallSequence()
    {
        yield return new WaitForSeconds(1.5f);

        SceneTransitor.Instance.GoToNextScene();
    }
}
