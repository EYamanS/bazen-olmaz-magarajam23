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
        if (transform.parent.name.Contains("cocuk"))
        {
            ProceedDialogue("Hey Kid! What are you doing here??");
        }
        else
        {
            ProceedDialogue("Hey! GET AWAY FROM HER! WHAT DO YOU WANT?");
        }
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

                    StartCoroutine(SaveKidSequence());

                }
                else if (obj.Contains("[FALL]"))
                {

                    StartCoroutine(KidFallSequence());
                }
                else if (obj.Contains("[NOTKILL]"))
                {
                    StartCoroutine(WifeAliveSequence());
                }
                else if (obj.Contains("[KILL]"))
                {
                    StartCoroutine(WifeDieSequence());
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
        PlayerPrefs.SetString("kid", "alive");
        yield return new WaitForSeconds(1.5f);
        SceneTransitor.Instance.GoToNextScene();
    }

    private IEnumerator KidFallSequence()
    {
        PlayerPrefs.SetString("kid", "dead");
        yield return new WaitForSeconds(1.5f);
        SceneTransitor.Instance.GoToNextScene();
    }

    private IEnumerator WifeDieSequence()
    {
        PlayerPrefs.SetString("wife", "dead");

        yield return new WaitForSeconds(1.5f);
        SceneTransitor.Instance.GoToNextScene();

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
    private IEnumerator WifeAliveSequence()
    {
        PlayerPrefs.SetString("wife", "alive");

        yield return new WaitForSeconds(1.5f);
        SceneTransitor.Instance.GoToNextScene();

        SceneManager.LoadScene(0, LoadSceneMode.Single);
    }
}
