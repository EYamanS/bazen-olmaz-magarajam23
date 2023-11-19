using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombatManager : SingletonComponent<PlayerCombatManager>
{
    [SerializeField] float coverSpeed;
    [SerializeField] float coverDistanceSensivitiy = .5f;
    [SerializeField] BoxCollider2D playerMovementCollider;
    [SerializeField] LayerMask hittableLayerMask;

    [SerializeField] AudioClip shootClip;

    public Action onPlayerDeath;

    [Space(10)]
    [SerializeField] ParticleSystem bulletParticleSystem;

    [Space(15)]
    [SerializeField] float attackDelay = .3f;
    [SerializeField] float reloadDelay = 1f;
    private float remainingAttackDelay = 0;
    private bool attackReady = false;


    public bool isNearCover = false;
    public Cover nearCover;
    public Vector3 coverStartPos;
    private float prevGravityScale;

    public bool isCovering = false;

    private CharacterController _characterController;
    private Rigidbody2D _rigidbody;
    private Animator playerAnimator;
    private PlayerCombatManager _playerCombatManager;
    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        _characterController = GetComponent<CharacterController>();
        _rigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.TryGetComponent<Cover>(out var cover))
        {
            isNearCover = true;
            nearCover = cover;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (nearCover == null) return;

        if (collision.gameObject == nearCover.gameObject)
        {
            isNearCover = false;
            nearCover = null;
        }
    }

    private void HandleAttack()
    {
        if (isCovering) return;

        if (attackReady)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Attack();
                attackReady = false;
                remainingAttackDelay = attackDelay;
            }
        }
        else
        {
            remainingAttackDelay -= Time.deltaTime;
            if (remainingAttackDelay <= 0)
            {
                attackReady = true;
            }
        }
    }

    private void Attack()
    {
        bulletParticleSystem.Play();
        playerAnimator.SetTrigger("shoot");

        audioSource.clip = shootClip;
        audioSource.Play();

        RaycastHit2D hit = Physics2D.Raycast(bulletParticleSystem.transform.position, transform.right, 99, hittableLayerMask);

        if (hit.collider != null && hit.collider.TryGetComponent<ArmedEnemy>(out var armedEnemy))
        {
            if (!armedEnemy.covered)
            {
                Debug.Log("hit " + hit.collider.gameObject.name);
                StartCoroutine(EnemyDie(armedEnemy));
            }
        }
    }

    private IEnumerator EnemyDie(ArmedEnemy enemy)
    {
        enemy.GetComponent<ArmedEnemy>().enabled = false;
        enemy.GetComponent<Animator>().SetTrigger("die");
        enemy.GetComponent<Animator>().SetBool("cover",false);
        yield return new WaitForSeconds(.2f);
    }

    private void Update()
    {
        if (!isCovering)
        {
            if (isNearCover && nearCover != null)
            {
                if (Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.LeftControl))
                {
                    Transform nearestCover = nearCover.GetNearestCover(transform.position);
                    if (nearestCover != null)
                    {
                        StopAllCoroutines();
                        StartCoroutine(GoToCover(nearestCover));
                    }
                }
                else
                {
                    HandleAttack();
                }
            }
            else
            {
                HandleAttack();
            }
        }
        else
        {
            // player is covering. No attack action
            if (!Input.GetKey(KeyCode.LeftCommand) && !Input.GetKey(KeyCode.LeftControl))
            {
                StopAllCoroutines();
                StartCoroutine(GetOutFromCover());
            }
        }
    }

    private IEnumerator GoToCover(Transform cover)
    {
        coverStartPos = transform.position;

        // DISABLE PLAYER MOVEMENT!
        _characterController.enabled = false;
        prevGravityScale = _rigidbody.gravityScale;
        _rigidbody.gravityScale = 0;
        playerAnimator.SetBool("doCover", true);

        isCovering = true;
        // PLAY COVER ANIMATION
        // ..................


        // move it to the cover.
        playerMovementCollider.enabled = false;
        while ((transform.position - cover.position).magnitude > coverDistanceSensivitiy)
        {
            _rigidbody.velocity = (cover.position - transform.position).normalized * coverSpeed;
            yield return new WaitForEndOfFrame();
        }
        _rigidbody.velocity = Vector3.zero;

        
    }

    private IEnumerator GetOutFromCover()
    {
        playerAnimator.SetBool("doCover", false);

        while ((transform.position - coverStartPos).magnitude > coverDistanceSensivitiy)
        {
            _rigidbody.velocity = (coverStartPos - transform.position).normalized * coverSpeed;
            yield return new WaitForEndOfFrame();
        }
        _rigidbody.velocity = Vector3.zero;


        _characterController.enabled = true;
        _rigidbody.gravityScale = prevGravityScale;

        playerMovementCollider.enabled = true;

        isCovering = false;
    }
}
