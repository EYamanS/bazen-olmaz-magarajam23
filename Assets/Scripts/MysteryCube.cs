using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryCube : SingletonComponent<MysteryCube>, IInteractable
{
    private Vector3 moveTarget;
    private bool hasTarget = true;
    public bool doMove = false;

    private PlayerCombatManager targetPlayer;
    private Rigidbody2D _rb;

    [SerializeField] float moveSpeed = 4f;
    [SerializeField] float maxSwayDist = 5f;

    protected override void Awake()
    {
        base.Awake();
        targetPlayer = FindObjectOfType<PlayerCombatManager>();
        _rb = GetComponent<Rigidbody2D>();
        moveTarget = targetPlayer.transform.position;
    }

    public void interact()
    {
        if (!gameObject.activeSelf) return;

        if (!DialogWindow.Instance.gameObject.activeSelf)
        {
            DialogWindow.Instance.gameObject.SetActive(true);
        }
        else
        {
            DialogWindow.Instance.AdvanceDialogue();
        }
    }

    public void DisableMovement() => doMove = false;
    public void EnableMovement() => doMove = true;

    private void Update()
    {
        if (!doMove)
        {
            _rb.velocity = Vector2.zero;
            return;
        }

        if (hasTarget)
        {
            _rb.velocity = (moveTarget - transform.position).normalized * moveSpeed;

            if ((moveTarget - transform.position).magnitude < .05f)
            {
                hasTarget = false;
            }
        }
        else
        {
            FindTarget();
        }
    }

    private void FindTarget()
    {
        moveTarget = targetPlayer.transform.position + new Vector3(Random.Range(-maxSwayDist, maxSwayDist), Random.Range(0, maxSwayDist), 0);
        hasTarget = true;
    }

    public void OnTriggerExit()
    {
    }
}
