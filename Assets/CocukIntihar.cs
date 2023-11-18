using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocukIntihar : MonoBehaviour
{
    private bool firstTime = false;

    [SerializeField] GameObject[] objectsToActivate;

    private void OnCollisionEnter2D(Collision2D collision)
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
        }
    }
}
