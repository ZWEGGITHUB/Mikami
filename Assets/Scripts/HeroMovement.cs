using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class HeroMovement : MonoBehaviour
{
    [Header("Rigidbody")] 
    private Rigidbody2D rigidbodyPlayer;

    [Header("Movement")] 
    [SerializeField] private float moveSpeed = 8f;
    private bool isFacingRight = true;
    private float horizontal;
    private float acceleration = 20f;
    private float deceleration = 12f;

    [Header("Jump")] 
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private float jumpingPower = 8f;
    private bool doubleJump;

    [Header("Dash")] 
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;
    [SerializeField] private TrailRenderer trailRenderer;

    private void Start()
    {
        rigidbodyPlayer = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        
        horizontal = Input.GetAxisRaw("Horizontal");

        if (IsGrounded() && !Input.GetButton("Jump"))
        {
            doubleJump = false;
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            if (IsGrounded() || doubleJump)
            {
                rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x, jumpingPower);

                doubleJump = !doubleJump;
            }
        }

        /* Pour faire en sorte que quand tu appuies plus ou moins longtemps sur le bouton de saut le joueur saute plus haut ou moins haut
         
        if (Input.GetButtonUp("Jump") && rigidbodyPlayer.velocity.y > 0f)
        {
            rigidbodyPlayer.velocity = new Vector2(rigidbodyPlayer.velocity.x, rigidbodyPlayer.velocity.y * 0.5f);
        }
        */

        if (Input.GetKeyDown(KeyCode.E) && canDash)
        {
            StartCoroutine(Dash());
        }
        
        Flip();
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        
        if (horizontal != 0)
        {
            rigidbodyPlayer.velocity = new Vector2(horizontal * (moveSpeed + acceleration * Time.fixedDeltaTime), rigidbodyPlayer.velocity.y);   
        }
        else
        {
            rigidbodyPlayer.velocity = new Vector2(horizontal * (moveSpeed - deceleration * Time.fixedDeltaTime), rigidbodyPlayer.velocity.y);   
        }
    }

    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, .2f, groundLayer);
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rigidbodyPlayer.gravityScale;
        rigidbodyPlayer.gravityScale = 0f;
        rigidbodyPlayer.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        trailRenderer.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        rigidbodyPlayer.gravityScale = originalGravity;
        trailRenderer.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
}
