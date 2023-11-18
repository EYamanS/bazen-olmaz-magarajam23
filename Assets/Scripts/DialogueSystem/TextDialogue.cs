using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextDialogue : DialogueStep
{
    public override bool CanSkip()
    {
        if (!writingComplete) return false;
        return true;
    }
}
