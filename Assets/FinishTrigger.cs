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
    private int sceneIndex = 0;

    public void FinishSection()
    {
        if (interacted) return;
        else
        {
            interacted = true;
        }

        sceneIndex++;
        float alpha = 0;
        
        var textTween = DOTween.To(() =>
            alpha, newAlpha => alpha = newAlpha,
            1,
            .5f).OnUpdate(
                () =>
                cutsceneImage.color = new Color(cutsceneImage.color.r, cutsceneImage.color.g, cutsceneImage.color.b, alpha)).OnComplete(
            () =>
            {
                SceneManager.LoadScene(sceneIndex);
            });
    }
}
