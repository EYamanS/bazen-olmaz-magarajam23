using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed, dirX, dirY, hookMultiplierX, hookMultiplierY;
    [SerializeField] bool isGrounded;
    SpriteRenderer sr;
    Animator anim;
    [SerializeField] GameObject interactableSign, _hook;
    [SerializeField] float timer = 0;
    float garagara;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
       if(collision.gameObject.TryGetComponent<IInteractable>(out var interactable))
        {
            interactableSign.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent<IInteractable>(out var interactable))
        {
            Debug.Log("Works");
            interactableSign.gameObject.SetActive(false);
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        playerMovement();
        if(Input.GetMouseButton(0))
        {
                if (timer < 1.5f)
                {
                    timer += Time.deltaTime;
                }
                else
                {
                    timer = 1.5f;
                }
        }
        if(Input.GetMouseButtonUp(0))
        {
            hookMultiplierX = timer*2;
            hookMultiplierY = timer*2;
            timer = 0;
            if (sr.flipX) { garagara = -0.5f; } else { garagara = 0.5f; }
            Vector2 transformPos = new Vector2(rb.transform.position.x + garagara, rb.transform.position.y);
            GameObject hook = Instantiate(_hook, transformPos, Quaternion.identity);
            hook.GetComponent<Rigidbody2D>().velocity = new Vector2(hookMultiplierX * (Camera.main.ScreenToWorldPoint(Input.mousePosition).x - rb.position.x), hookMultiplierY * (Camera.main.ScreenToWorldPoint(Input.mousePosition).y - rb.position.y));
        }
    }

    void playerMovement()
    {
        if(speed != 0)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            speed = dirX;
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            speed = -dirX;
        }
        if ((Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.A)))
        {
            speed = 0;
        }
        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, dirY);
            isGrounded = false;
        }
        if (speed < 0)
        {
            sr.flipX = true;
        }
        else if (speed > 0)
        {
            sr.flipX = false;
        }
        rb.velocity = new Vector2(speed, rb.velocity.y);
    }
}
