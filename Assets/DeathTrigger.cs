using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour
{
    [SerializeField] ParkourTrigger parkour;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<CharacterController>(out var ctrl))
        {
            ctrl.transform.position = parkour.parkourRespawnPoint.position;
        }
    }
}
