using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmedEnemy : MonoBehaviour
{
    PlayerCombatManager _playerCombatManager;
    Rigidbody2D _rigidbody;
    BoxCollider2D enemyCollider;

    private bool noticedPlayer = false;
    private bool attackReady = false;
    private bool isCovering = false;
    private Vector3 coverStartPos;
    private float shootDelayCountdown = 0;
    private float coverLeftCountdown = 0f;
    private float decisionCountdown = 0f;

    [Header("Combat Settings")]
    [SerializeField] float decisionDelay = 1f;
    [SerializeField] float minShootDelay;
    [SerializeField] float maxShootDelay;
    [SerializeField] float shootChance = .8f;
    [SerializeField] float noticeRange = 20f;
    [SerializeField] ParticleSystem bulletParticleSystem;

    [Space(15)]
    [Header("Cover Settings")]
    [SerializeField] Cover enemyCover;
    [SerializeField] float coverDistanceSensivitiy = .2f;
    [SerializeField] float coverSpeed = 10f;

    [SerializeField] float minCoverTime = 2f;
    [SerializeField] float maxCoverTime = 4f;

    private void Awake()
    {
        _playerCombatManager = FindObjectOfType<PlayerCombatManager>();
        _rigidbody = GetComponent<Rigidbody2D>();
        enemyCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        if (shootDelayCountdown >= 0) shootDelayCountdown -= Time.deltaTime;
        else { attackReady = true; }

        if (coverLeftCountdown >= 0) coverLeftCountdown -= Time.deltaTime;

        if (decisionCountdown > 0) { decisionCountdown -= Time.deltaTime; return; }
        else
        {
            decisionCountdown = decisionDelay;
        }


        
        if (!noticedPlayer)
        {
            if ((_playerCombatManager.transform.position - transform.position).magnitude < noticeRange)
            {
                noticedPlayer = true;
            }
        }
        else
        {
            if (coverLeftCountdown > 0) return;

            float decidedActionVal = Random.Range(0f, 1f);
            if (decidedActionVal < shootChance)
            {
                // decided action is shoot
                if (attackReady)
                {
                    if (isCovering)
                    {
                        attackReady = false;
                        shootDelayCountdown = Random.Range(minShootDelay, maxShootDelay);
                        StartCoroutine(GetOutFromCover(attackAfter: true));
                    }
                    else
                    {
                        attackReady = false;
                        shootDelayCountdown = Random.Range(minShootDelay, maxShootDelay);
                        StartCoroutine(Attack());
                    }
                }
                else
                {
                    if (isCovering)
                        StartCoroutine(GetOutFromCover(attackAfter: false));
                }

            }
            else
            {
                // decided action is cover.
                if (isCovering)
                {
                    // just stay lol.
                }
                else
                {
                    isCovering = true;
                    coverLeftCountdown = Random.Range(minCoverTime, maxCoverTime);
                    StartCoroutine(GoToCover());
                }
            }
        }
    }


    private IEnumerator Attack()
    {
        yield return new WaitForSeconds(Random.Range(.2f,.4f));
        bulletParticleSystem.Play();
        RaycastHit2D hit = Physics2D.Raycast(bulletParticleSystem.transform.position, bulletParticleSystem.transform.forward, 99);

        if (hit.collider != null)
        {
            Debug.Log("enemy hit " + hit.collider.gameObject.name);
        }
    }

    private IEnumerator GoToCover()
    {
        coverStartPos = transform.position;
        Debug.Log("Going to cover");
        Transform chosenCover = enemyCover.GetNearestCover(transform.position);

        // PLAY COVER ANIMATION
        // ..................
        enemyCollider.enabled = false;

        // move it to the cover.
        while ((transform.position - chosenCover.position).magnitude > coverDistanceSensivitiy)
        {
            _rigidbody.velocity = (chosenCover.position - transform.position).normalized * coverSpeed;
            yield return new WaitForEndOfFrame();
        }

        _rigidbody.velocity = Vector3.zero;

    }

    private IEnumerator GetOutFromCover(bool attackAfter = false)
    {
        while ((transform.position - coverStartPos).magnitude > coverDistanceSensivitiy)
        {
            _rigidbody.velocity = (coverStartPos - transform.position).normalized * coverSpeed;
            yield return new WaitForEndOfFrame();
        }
        _rigidbody.velocity = Vector3.zero;

        isCovering = false;
        enemyCollider.enabled = true;


        if (attackAfter)
        {
            yield return Attack();
        }
    }
}
