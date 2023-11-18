using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneRoot : MonoBehaviour
{
    [SerializeField] Transform characterSpawnPoint;
    [HideInInspector] public Transform player;

    public virtual void Awake()
    {
        player = FindObjectOfType<PlayerCombatManager>().transform;
    }

    private void OnEnable()
    {
        OnSceneActive();
    }

    private void OnDisable()
    {
        OnScenePassive();
    }

    public virtual void OnSceneActive()
    {
        if (characterSpawnPoint == null) return;
        transform.position = player.position; //+ (player.position - characterSpawnPoint.position);
    }

    public virtual void OnScenePassive()
    {

    }
}
