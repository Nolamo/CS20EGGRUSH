using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Duck : MonoBehaviour
{
    public float speed;
    Animator anim;
    bool stunned;
    public float raycastDistance;
    public Transform rayOrigin;
    public float slopeCastDistance;
    public Transform slopeRayOrigin;
    public Transform flippingTransforms;
    public Transform ledgeRayOrigin;


    public SpriteRenderer sprite;

    public float defaultGravity;
    public float slopeGravity;

    public LayerMask whatIsWall;

    public EnemyHealth hp;
    RaycastHit2D hit;
    RaycastHit2D slopeHit;
    RaycastHit2D LedgeHit;

    public GameObject projectile;

    public float fireTime;
    float fireDelay;
    public Transform fireDirection;
    public bool firing;


    bool detectingSlope;

    // Start is called before the first frame update
    void Start()
    {
        fireDelay = fireTime;
        anim = GetComponent<Animator>();

        hit = Physics2D.Raycast(rayOrigin.position, Vector2.right, raycastDistance, whatIsWall);
        slopeHit = Physics2D.Raycast(slopeRayOrigin.position, Vector2.right, slopeCastDistance, whatIsWall);
        LedgeHit = Physics2D.Raycast(ledgeRayOrigin.position, -Vector2.up, 0.25f, whatIsWall);
    }

    // Update is called once per frame
    void Update()
    {
        LedgeHit = Physics2D.Raycast(ledgeRayOrigin.position, -Vector2.up, 0.4f, whatIsWall);

        firing = LedgeHit.collider == null;

        if (firing)
        {
            hp.rb.constraints = RigidbodyConstraints2D.FreezeAll;

            anim.SetBool("Firing", true);

            fireDelay -= Time.deltaTime;

            if(fireDelay <= 0)
            {
                Fire();
            }
        }

        stunned = hp.stunned > 0;

        if (hp.facingLeft && !stunned && !firing)
        {
            flippingTransforms.localScale = new Vector3(-1, 1, 1);
            hp.rb.velocity = new Vector2(-speed, hp.rb.velocity.y);
            hit = Physics2D.Raycast(rayOrigin.position, -Vector2.right, raycastDistance, whatIsWall);
        }
        else if (!stunned && !firing)
        {
            flippingTransforms.localScale = new Vector3(1, 1, 1);
            hp.rb.velocity = new Vector2(speed, hp.rb.velocity.y);
            hit = Physics2D.Raycast(rayOrigin.position, Vector2.right, raycastDistance, whatIsWall);
        }

        if (hp.facingLeft && !stunned && detectingSlope && !firing)
        {
            slopeHit = Physics2D.Raycast(slopeRayOrigin.position, -Vector2.right, slopeCastDistance, whatIsWall);
        }
        else if (!stunned && detectingSlope && !firing)
        {
            slopeHit = Physics2D.Raycast(slopeRayOrigin.position, Vector2.right, slopeCastDistance, whatIsWall);
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
        } 
    }

    void Fire()
    {
        anim.Play("Fire");
        fireDelay = fireTime;

        Instantiate(projectile, fireDirection.position, fireDirection.rotation);
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

        Gizmos.DrawRay(ledgeRayOrigin.position, Vector2.down);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        detectingSlope = collision != null;
    }
}
