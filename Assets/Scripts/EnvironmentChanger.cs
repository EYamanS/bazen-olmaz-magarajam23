using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Cinemachine.PostFX;

public class EnvironmentChanger : MonoBehaviour
{
    [SerializeField] CinemachineVolumeSettings[] fxToClose;
    [SerializeField] GameObject[] objectsToActivate;
    [SerializeField] GameObject[] objectsToDeActivate;

    private void OnEnable()
    {
        for (int i = 0; i < fxToClose.Length; i++)
        {
            fxToClose[i].enabled = false;
        }

        for (int i = 0; i < objectsToDeActivate.Length; i++)
        {
            objectsToDeActivate[i].SetActive(false);
        }
        for (int i = 0; i < objectsToActivate.Length; i++)
        {
            objectsToActivate[i].SetActive(true);
        }

    }


    private void OnDisable()
    {
        for (int i = 0; i < fxToClose.Length; i++)
        {
            fxToClose[i].enabled = true;
        }

        for (int i = 0; i < objectsToActivate.Length; i++)
        {
            objectsToActivate[i].SetActive(false);
        }

        for (int i = 0; i < objectsToDeActivate.Length; i++)
        {
            objectsToDeActivate[i].SetActive(true);
        }
    }
}
