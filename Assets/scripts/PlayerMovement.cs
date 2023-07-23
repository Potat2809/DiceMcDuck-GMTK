using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movspeed;

    //jump & check
    public float jumpforce;
    public Transform cielingcheck;
    public Transform groundcheck;
    public Transform wallcheck;
    public LayerMask groundobjects;
    public float checkradius;
    public TrailRenderer tr;

    //dash
    private bool candash = true;
    private bool isdashing;
    private float dashingpower = 24f;
    private float dashingtime = 0.2f;
    private float dashingcooldown = 1f;

    //wall jump
    private bool iswalltouching;
    private bool iswallsliding;
    public float wallslidingspeed;

    private Rigidbody2D rb;
    private bool facingright = true;
    private float movedirection;
    private bool isjumping = false;
    private bool isgrounded;

    public void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    { 
        //Input
        processinput();

        //flip
        flip();

        //Dash
        if (isdashing)
        {
            return;
        }

        dashing();

        //walljump
    }

    private void dashing()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && candash)
        {
            StartCoroutine(dash());
        }
    }

    private void FixedUpdate()
    {
        //dash
        if (isdashing)
        {
            return;
        }
        //Movement
        movement();
    }

    // Check if grounded
    private void OnCollisionEnter2D(Collision2D collision)
    {
        isgrounded = true;
        iswalltouching = true;
    }

    private void movement()
    {
        rb.velocity = new Vector2(movedirection * movspeed, rb.velocity.y);
        if (isjumping)
        {
            rb.AddForce(new Vector2(0f, jumpforce));
            isjumping = false;
        }
    }

    private void flip()
    {
        if (movedirection < 0f && facingright || movedirection > 0f && !facingright )
        {
            facingright = !facingright;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private void processinput()
    {
        movedirection = Input.GetAxisRaw("Horizontal");
        if(Input.GetKeyDown(KeyCode.Space) && isgrounded)
        {
            isjumping = true;
            isgrounded = false;
        }
    }

    private IEnumerator dash()
    {
        candash = false;
        isdashing = true;
        float originalgravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingpower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingtime);
        tr.emitting = false;
        rb.gravityScale = originalgravity;
        isdashing = false;
        yield return new WaitForSeconds(dashingcooldown);
        candash = true;
    }
}
