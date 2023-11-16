using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    Rigidbody2D rb;
    [SerializeField] float speed, dirX, dirY;
    [SerializeField] bool isGrounded, isMoving;
    SpriteRenderer sr;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
        }
    }
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        isMoving = false;
    }

    void Update()
    {
        playerMovement();
    }

    void playerMovement()
    {
        if(speed != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
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
