using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CocukScene : SceneRoot
{
    [SerializeField] CinemachineVirtualCamera vCamera;
    [SerializeField] CinemachineVirtualCamera vCamera2;

    public override void OnSceneActive()
    {
        base.OnSceneActive();
        vCamera.Follow = player;
        vCamera2.Follow = player;
    }
}
