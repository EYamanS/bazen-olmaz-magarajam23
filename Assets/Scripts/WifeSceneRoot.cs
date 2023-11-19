using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class WifeSceneRoot : SceneRoot
{
    [SerializeField] CinemachineVirtualCamera vCamera;
    [SerializeField] ArmedEnemy[] enemies;
    [SerializeField] Transform playerSpawnPos;
    

    public override void OnSceneActive()
    {
        base.OnSceneActive();
        vCamera.Follow = player;
        PlayerCombatManager.Instance.enabled = true;
        PlayerCombatManager.Instance.onPlayerDeath += ProcessPlayerDeath;
        CharacterController.Instance.canMove = true;
        CharacterController.Instance.jumpSpeed = 16;
    }

    private void ProcessPlayerDeath()
    {
        PlayerCombatManager.Instance.transform.position = playerSpawnPos.position;
        for (int i = 0; i < enemies.Length; i++)
        {
            enemies[i].gameObject.SetActive(true);
            enemies[i].blockCollider.gameObject.SetActive(true);
        }
    }
}
