using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocukIntihar : MonoBehaviour
{
    private bool firstTime = true;

    [SerializeField] GameObject[] objectsToActivate;
    [SerializeField] GameObject[] objectsToDeactivate;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!firstTime) return;

        if (collision.gameObject.TryGetComponent<CharacterController>(out var controller))
        {
            firstTime = true;
            controller.canMove = false;

            for (int i = 0; i < objectsToActivate.Length; i++)
            {
                objectsToActivate[i].SetActive(true);
            }
            for (int i = 0; i < objectsToDeactivate.Length; i++)
            {
                objectsToDeactivate[i].SetActive(false);
            }
        }
    }
}
