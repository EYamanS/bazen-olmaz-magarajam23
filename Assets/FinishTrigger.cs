using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
public class FinishTrigger : MonoBehaviour
{
    [SerializeField] Image cutsceneImage;
    private bool interacted = false;

    public void FinishSection()
    {
        if (interacted) return;
        else
        {
            interacted = true;
        }

        SceneTransitor.Instance.GoToNextScene();
    }
}
