using UnityEngine;

/// <summary> Handles Enemy health. </summary>
// rb stuff is also here because lol
public class EnemyHealth : MonoBehaviour
{
    public int maxHealth;
    public int health;

    public Rigidbody2D rb;
    public GameObject hitEffect;
    public GameObject deadEffect;

    public bool facingLeft;

    public float stunned;

    private void Start()
    {
        health = maxHealth;
    }

    // deal damage, knockback, stun. 
    public void Damage(int amount, float stunTime, Vector2 knockback)
    {
        health -= amount;
        stunned += stunTime;
        rb.AddForce(knockback);

        if(health <= 0) Die();
        else Instantiate(hitEffect, transform.position, Quaternion.identity);
    }

    // death become me
    void Die()
    {
        Instantiate(deadEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (stunned > 0)
            stunned -= Time.deltaTime;
    }
}
