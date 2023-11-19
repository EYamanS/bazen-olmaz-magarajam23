using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using DG.Tweening;

public class ParkourTrigger : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera defaultCamera;
    [SerializeField] CinemachineVirtualCamera parkourCamera;

    public Transform parkourRespawnPoint;

    private bool firstTime = true;

    public void SwitchCamera(bool parkour)
    {
        if (parkour)
        {
            CharacterController.Instance.gameObject.GetComponent<Animator>().SetBool("inParkour", true);
            defaultCamera.Priority = 1;
            parkourCamera.Priority = 2;

            if (firstTime)
            {
                TeachAction();
            }
        }
        else
        {
            defaultCamera.Priority = 2;
            parkourCamera.Priority = 1;
        }

        firstTime = false;
    }

    private void TeachAction()
    {
        var robot = FindObjectOfType<MysteryCube>(true);
        var robotComponent = robot.GetComponent<MysteryCube>();

        robotComponent.moveTarget = parkourRespawnPoint.position;

        CharacterController.Instance.canMove = false;
        robot.transform.DOMove(FindObjectOfType<PlayerCombatManager>().transform.position, 1f).OnComplete(() =>
        {
            //robotComponent.SetInteractibility(true);
            CharacterController.Instance.canMove = true;
        });
    }

    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<CharacterController>(out var ctrl))
        {
            ctrl.transform.position = parkourRespawnPoint.position;
        }
    }
    */
}
