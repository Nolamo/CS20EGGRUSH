using UnityEngine;

/// <summary> A simple Enemy that just strolls back and forwards. Startles when it takes damage. </summary>
public class Chicken : MonoBehaviour
{
    public float speed;
    public float startledSpeed;
    Animator anim;
    bool startled;
    bool stunned;
    public float raycastDistance;
    public Transform rayOrigin;
    public float slopeCastDistance;
    public Transform slopeRayOrigin;

    public SpriteRenderer sprite;

    public float defaultGravity;
    public float slopeGravity;

    public LayerMask whatIsWall;

    public EnemyHealth health;
    RaycastHit2D hit;
    RaycastHit2D slopeHit;

    bool detectingSlope;

    void Start()
    {
        
        anim = GetComponent<Animator>();
        hit = Physics2D.Raycast(rayOrigin.position, Vector2.right, raycastDistance, whatIsWall);
        slopeHit = Physics2D.Raycast(slopeRayOrigin.position, Vector2.right, slopeCastDistance, whatIsWall);
    }

    void Update()
    {
        stunned = health.stunned > 0;

        // movement code, if stunned, stop movement temporarily.
        {
        if (health.facingLeft && !stunned)
        {
            health.rb.velocity = new Vector2(-speed, health.rb.velocity.y);
            hit = Physics2D.Raycast(rayOrigin.position, -Vector2.right, raycastDistance, whatIsWall);
        }
        else if (!stunned)
        {
            health.rb.velocity = new Vector2(speed, health.rb.velocity.y);
            hit = Physics2D.Raycast(rayOrigin.position, Vector2.right, raycastDistance, whatIsWall);
        }
        if (health.facingLeft && !stunned && detectingSlope)
            slopeHit = Physics2D.Raycast(slopeRayOrigin.position, -Vector2.right, slopeCastDistance, whatIsWall);
        else if (!stunned && detectingSlope)
            slopeHit = Physics2D.Raycast(slopeRayOrigin.position, Vector2.right, slopeCastDistance, whatIsWall); }
        
        // would be an auto-property/anon function in unity 2020
        // ex. bool startled -> health.health < health.maxHealth;
        startled = health.health < health.maxHealth;

        if(startled)
        {
            // if startled, change speed to startledSpeed and switch to startle animation.
            anim.SetBool("Startled", true);
            speed = startledSpeed;
        }

        if (stunned)
            anim.SetBool("stunned", true);

        if (slopeHit.collider == null)
            health.rb.gravityScale = slopeGravity;
        else
            health.rb.gravityScale = defaultGravity;

        // if hits a wall, turn around.
        if (hit.collider != null)
        {
            if (health.facingLeft)
            {
                health.facingLeft = false;
                sprite.flipX = false;
            }
            else
            {
                health.facingLeft = true;
                sprite.flipX = true;
            }
        } 
    }

    // unity dev gizmos
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        if (health.facingLeft)
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
}
