using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turkey : MonoBehaviour
{
    public float jumpTimer;
    float jumpDelay;
    public float jumpForce;
    public GameObject jumpEffect;

    public float speed;
    public float startledSpeed;
    Animator anim;
    public bool startled;
    bool stunned;
    public float raycastDistance;
    public Transform rayOrigin;
    public float slopeCastDistance;
    public Transform slopeRayOrigin;

    public SpriteRenderer sprite;

    public float defaultGravity;
    public float slopeGravity;

    public LayerMask whatIsWall;

    public EnemyHealth hp;
    RaycastHit2D hit;
    RaycastHit2D slopeHit;

    bool detectingSlope;

    // Start is called before the first frame update
    void Start()
    {
        jumpDelay = jumpTimer;

        anim = GetComponent<Animator>();
        hit = Physics2D.Raycast(rayOrigin.position, Vector2.right, raycastDistance, whatIsWall);
        slopeHit = Physics2D.Raycast(slopeRayOrigin.position, Vector2.right, slopeCastDistance, whatIsWall);
    }

    // Update is called once per frame
    void Update()
    {

        stunned = hp.stunned > 0;

        if (hp.facingLeft && !stunned)
        {
            hp.rb.velocity = new Vector2(-speed, hp.rb.velocity.y);
            hit = Physics2D.Raycast(rayOrigin.position, -Vector2.right, raycastDistance, whatIsWall);
        }
        else if (!stunned)
        {
            hp.rb.velocity = new Vector2(speed, hp.rb.velocity.y);
            hit = Physics2D.Raycast(rayOrigin.position, Vector2.right, raycastDistance, whatIsWall);
        }

        if (hp.facingLeft && !stunned && detectingSlope && !startled)
        {
            slopeHit = Physics2D.Raycast(slopeRayOrigin.position, -Vector2.right, slopeCastDistance, whatIsWall);
        }
        else if (!stunned && detectingSlope && !startled)
        {
            slopeHit = Physics2D.Raycast(slopeRayOrigin.position, Vector2.right, slopeCastDistance, whatIsWall);
        }

        startled = hp.health < hp.maxHealth;

        if(startled)
        {
            anim.SetBool("Startled", true);

            speed = startledSpeed;

            if (!stunned)
            {
                jumpDelay -= Time.deltaTime;
                if(jumpDelay <= 0)
                {
                    Jump(jumpForce);
                }
            }
        }

        if (stunned)
        {
            anim.SetBool("stunned", true);
        }

        if (slopeHit.collider == null)
        {
            hp.rb.gravityScale = slopeGravity;
        }
        else
        {
            hp.rb.gravityScale = defaultGravity;
        }

        if (hit.collider != null)
        {
            if (hp.facingLeft)
            {
                hp.facingLeft = false;
                sprite.flipX = false;
            }
            else
            {
                hp.facingLeft = true;
                sprite.flipX = true;
            }
            if (startled)
            {
                Jump(jumpForce);
            }
        } 
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (hp.facingLeft)
        {
            Gizmos.DrawRay(rayOrigin.position, -Vector2.right);
            Gizmos.DrawRay(slopeRayOrigin.position, -Vector2.right);
        }
        else
        {
            Gizmos.DrawRay(rayOrigin.position, Vector2.right);
            Gizmos.DrawRay(slopeRayOrigin.position, Vector2.right);
        }

        

    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        detectingSlope = collision != null;
    }

    void Jump(float jumpForce)
    {
        jumpDelay = jumpTimer;
        hp.rb.AddForce(Vector2.up * jumpForce);
        Instantiate(jumpEffect, transform.position, Quaternion.identity);
    }
}
