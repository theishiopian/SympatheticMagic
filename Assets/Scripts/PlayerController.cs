using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5;
    public float jumpSpeed = 5;
    public LayerMask groundMask;


    private Rigidbody2D body;

    private void Start()
    {
        body = GetComponent<Rigidbody2D>();
    }

    bool jumped = false;

    private void FixedUpdate()
    {
        float walk = Input.GetAxis("Horizontal");
        bool jump = Input.GetAxis("Jump") > 0;

        RaycastHit2D grounded = Physics2D.Raycast(transform.position, Vector2.down, 0.6f, groundMask);
        if (jump && grounded && !jumped)
        {
            body.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);
            jumped = true;
        }
        if (grounded && jumped)
        {
            jumped = false;
        }

        body.velocity = new Vector2(walk * moveSpeed, body.velocity.y);
    }
}
