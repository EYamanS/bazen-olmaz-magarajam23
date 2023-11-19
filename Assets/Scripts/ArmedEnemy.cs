using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ArmedEnemy : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    Collider2D enemyCollider;
    Animator animator;

    Vector3 coverStartPos;

    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] Cover enemyCover;
    [SerializeField] float shootChance = 0.8f;
    [SerializeField] ParticleSystem bulletParticleSystem;

    public bool doingAction = false;
    bool covered = false;

    private void Awake()
    {
        enemyCollider = GetComponent<Collider2D>();
        _rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
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


    private void Update()
    {
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
                    ShootRoutine();
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

    private IEnumerator Shoot()
    {
        Debug.Log("ates ediyomm");

        animator.SetTrigger("shoot");
        yield return new WaitForSeconds(.2f);
        bulletParticleSystem.Play();

        RaycastHit2D hit = Physics2D.Raycast(bulletParticleSystem.transform.position, transform.forward, 20, playerLayerMask);

        if (hit.collider != null)
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
