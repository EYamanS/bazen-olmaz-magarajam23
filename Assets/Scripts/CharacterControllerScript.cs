using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed, dirX, dirY;
    [SerializeField] bool isGrounded;
    SpriteRenderer sr;
    Animator anim;
    [SerializeField] GameObject interactableSign;
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

        if (speed != 0)
        {
            anim.SetBool("isMoving", true);
        }
        else
        {
            anim.SetBool("isMoving", false);
        }

        if (speed < 0)
        {
            sr.flipX = true;
        }
        else if (speed > 0)
        {
            sr.flipX = false;
        }
    }

    void playerMovement()
    {

        if (Input.GetKey(KeyCode.D))
        {
            speed = dirX;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            speed = -dirX;
        }
        else
        {
            speed = 0;
        }

        if (Input.GetKeyDown(KeyCode.W) && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, dirY);
            isGrounded = false;
        }

        rb.velocity = new Vector2(speed, rb.velocity.y);
    }
}
