using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hook : MonoBehaviour
{
    [SerializeField] Rigidbody2D _rb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _rb.isKinematic = true;
        _rb.velocity = Vector2.zero;
    }
}
