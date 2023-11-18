using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputTakerDialogue : DialogueStep
{
    [SerializeField] private TMP_InputField _inputField;
    [SerializeField] private string saveKeyName;

    public override bool CanSkip()
    {
        if (!writingComplete) return false;

        var canSkip = _inputField.text.Length > 0;
        if (canSkip) PlayerPrefs.SetString(saveKeyName,_inputField.text);
        return canSkip;
    }
}