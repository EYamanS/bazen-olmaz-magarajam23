using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArmedEnemy : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    Collider2D enemyCollider;
    Animator animator;
    AudioSource _shootAudioSource;

    Vector3 coverStartPos;

    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] Cover enemyCover;
    [SerializeField] float shootChance = 0.8f;
    [SerializeField] ParticleSystem bulletParticleSystem;
    [SerializeField] float noticeDistance = 22f;

    [SerializeField] AudioClip shootClip;

    public Collider2D blockCollider;

    public bool doingAction = false;
    public bool covered = false;
    bool noticedPlayer = false;

    private void Awake()
    {
        enemyCollider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _shootAudioSource = GetComponent<AudioSource>();
    }


    public bool DoCover()
    {
        float seed = Random.Range(0f, 1f);

        if (seed < shootChance)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnDisable()
    {
        blockCollider.enabled = false;
    }

    private void OnEnable()
    {
        blockCollider.enabled = true;
    }


    private void Update()
    {
        if (!noticedPlayer)
        {
            if ((PlayerCombatManager.Instance.transform.position - transform.position).magnitude < noticeDistance)
            {
                noticedPlayer = true;
                return;
            }
        }

        if (!doingAction)
        {
            doingAction = true;
            bool doCover = DoCover();

           if (doCover)
           {
                if (!covered)
                    CoverRoutine();
                else
                {
                    StartCoroutine(StayPutRoutine());

                }
            }
           else
           {
                if (covered)
                    ShootRoutine();
                else
                {
                    StartCoroutine(Shoot());
                }
           }
        }
    }

    private IEnumerator StayPutRoutine()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        doingAction = false;
    }

    private IEnumerator Shoot()
    {
        Debug.Log("ates ediyomm");
        _shootAudioSource.Play();
        animator.SetTrigger("shoot");
        yield return new WaitForSeconds(.2f);
        bulletParticleSystem.Play();

        RaycastHit2D hit = Physics2D.Raycast(new Vector2(bulletParticleSystem.transform.position.x, PlayerCombatManager.Instance.transform.position.y), Vector2.left, 20, playerLayerMask);

        if (hit.collider != null && !PlayerCombatManager.Instance.isCovering)
        {
            PlayerCombatManager.Instance.onPlayerDeath?.Invoke();
        }

        yield return new WaitForSeconds(.8f);
        doingAction = false;
    }

    private void CoverRoutine()
    {
        Debug.Log("covera giriyom");
        animator.SetBool("cover", true);
        coverStartPos = transform.position;
        _rigidbody.transform.DOMove(enemyCover.GetNearestCover(transform.position).position, .5f).OnComplete(() =>
        {
            doingAction = false;
            covered = true;
        });
    }

    private void ShootRoutine()
    {
        Debug.Log("coverdan cikiyom");
        animator.SetBool("cover", false);
        _rigidbody.transform.DOMove(coverStartPos, .5f).OnComplete(() =>
        {
            doingAction = false;
            covered = false;
        });
    }
}
