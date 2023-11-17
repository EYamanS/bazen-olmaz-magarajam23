using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Cinemachine;
using DG.Tweening;

public class CharacterController : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    private float speed;

    [ReadOnly]
    public bool isGrounded;
    SpriteRenderer _characterRenderer;
    Animator _playerAnimator;

    [SerializeField] LineRenderer parabolaRenderer;

    GameObject hook;

    [SerializeField] float movementSpeed = 15f;
    [SerializeField] float jumpSpeed = 50f;

    // Throwing References
    [SerializeField] GameObject _mysteryObjectPrefab;
    [SerializeField] private float maxThrowTimer = 3f;
    [SerializeField] private float minThrowTimer = 1f;
    [SerializeField] private float objectReturnSpeed = .2f;
    [SerializeField] private float throwSpeed = 5f;
    [SerializeField] private float objectPickupRange = .05f;
    [Space(10)]
    [SerializeField] private float yeetSpeed = 5f;

    private float throwTimer = 0;

    [Space(15)]
    [ReadOnly]
    public bool hasMysteryObject = true;
    [ReadOnly]
    public bool holdingMysteryObject = true;
    [ReadOnly]
    public bool canYeet = false;
    [ReadOnly]
    public bool canThrow = true;


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.TryGetComponent<IInteractable>(out var interactable))
       {
            // notify you can interact with it or not
       }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<IInteractable>(out var interactable))
        {
            Debug.Log("Works");
            // if player leaves this interactable, disable the head's up popup.
        }
    }
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _characterRenderer = GetComponent<SpriteRenderer>();
        _playerAnimator = GetComponent<Animator>();
    }

    private void HandleThrowing()
    {
        if (hasMysteryObject)
        {
            if (holdingMysteryObject)
            {
                if (Input.GetMouseButton(0))
                {
                    RenderParabola();

                    if (throwTimer > maxThrowTimer)
                    {
                        throwTimer = maxThrowTimer;
                    }
                    else
                    {
                        throwTimer += Time.deltaTime;
                    }
                    parabolaRenderer.enabled = true;
                }

                if (Input.GetMouseButtonUp(0))
                {
                    if (throwTimer > minThrowTimer)
                    {
                        if (!canThrow)
                        {
                            canThrow = true;
                            //return;
                        }

                        holdingMysteryObject = false;
                        canYeet = true;
                        canThrow = false;
                        isGrounded = false;
                        Throw();
                        throwTimer = 0;
                        parabolaRenderer.enabled = false;
                    }
                    else
                    {
                        throwTimer = 0;
                        canThrow = true;
                    }
                }
            }
            else
            {
                if(Input.GetMouseButtonDown(0) && canYeet)
                {
                    canYeet = false;
                    _rigidbody.velocity = (hook.transform.position - transform.position) * yeetSpeed;

                    hook.GetComponent<Rigidbody2D>().isKinematic = true;
                    hook.GetComponent<Collider2D>().enabled = false;

                    StartCoroutine(HookGoToPlayer());
                }
            }
        }
    }

    private void RenderParabola()
    {
        float pointSeperationTime = .2f;

        Vector3 aimDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(_rigidbody.position.x, _rigidbody.position.y, 0)).normalized;
        Vector3 finalVelocity = aimDirection * (throwSpeed * throwTimer / maxThrowTimer);

        for (int i = 0; i < parabolaRenderer.positionCount; i++)
        {
            float finalX = transform.position.x + finalVelocity.x * (pointSeperationTime * i);
            float finalY = transform.position.y + (finalVelocity.y * (pointSeperationTime * i)) - (.5f * -Physics2D.gravity.y * 0.3f * (pointSeperationTime * i) * (pointSeperationTime * i));
            parabolaRenderer.SetPosition(i, new Vector3(finalX, finalY, 0));
        }
    }

    private void Throw()
    {
        hook = Instantiate(_mysteryObjectPrefab, transform.position, Quaternion.identity);
        Rigidbody2D hookrb =  hook.GetComponent<Rigidbody2D>();
        Vector3 aimDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - new Vector3(_rigidbody.position.x, _rigidbody.position.y, 0)).normalized;
        Vector3 finalVelocity = aimDirection * (throwSpeed * throwTimer / maxThrowTimer);
        hookrb.velocity = finalVelocity;
    }

    private IEnumerator HookGoToPlayer()
    {
        var hookRb = hook.GetComponent<Rigidbody2D>();
        do
        {
            hookRb.velocity = -(hook.transform.position - transform.position).normalized * objectReturnSpeed;
            yield return new WaitForEndOfFrame();
        }
        while ((hook.transform.position - transform.position).magnitude > .1f);

        holdingMysteryObject = true;
        Destroy(hook.gameObject);
    }




    void Update()
    {
        HandlePlayerMovement();
        HandleThrowing();
        HandleAnimations();
    }

    void HandleAnimations()
    {
        if (speed != 0)
        {
            _playerAnimator.SetBool("isMoving", true);
        }
        else
        {
            _playerAnimator.SetBool("isMoving", false);
        }
        if (speed < 0)
        {
            _characterRenderer.flipX = true;
        }
        else if (speed > 0)
        {
            _characterRenderer.flipX = false;
        }
    }

    void HandlePlayerMovement()
    {
        if (isGrounded)
        {
            if (Input.GetKey(KeyCode.D))
            {
                speed = 1;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                speed = -1;
            }
            else
            {
                speed = 0;
            }

            if ((Input.GetKeyDown(KeyCode.W)))
            {
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpSpeed);
            }
             
            _rigidbody.velocity = new Vector2(speed * movementSpeed, _rigidbody.velocity.y);
            Debug.Log(speed * movementSpeed);

        }
        else
        {
            // no control
            speed = 0;

            if (Input.GetKey(KeyCode.D))
            {
                speed = 1;
            }
            else if (Input.GetKey(KeyCode.A))
            {
                speed = -1;
            }



            if (speed != 0)
            {
                _rigidbody.velocity = new Vector2(speed * movementSpeed, _rigidbody.velocity.y);
            }
        }

    }
}
