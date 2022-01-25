using System.Collections.Generic;
using UnityEngine;

/// <summary> Handles the player's health </summary>
[RequireComponent(typeof(PlayerMovement))]
public class Health : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public GameObject hitEffect;
    public GameObject deathEffect;
    public SpriteRenderer sprite;
    public float blockChance;

    public float maxHp = 4;
    public float hp;

    [HideInInspector]
    public float invincibility;
    public float invincibilityTime;
    public float invincibilityBlink;
    float invincibilityBlinkDelay;

    public float screenShakeDuration;
    public float screenShakeMagnitude;
    ScreenShake shake;

    private void Start()
    {
        shake = Camera.main.GetComponentInParent<ScreenShake>();
    }

    private void Update()
    {
        // If the player is invincible, blink the sprite
        if (invincibility > 0)
        {
            invincibility -= Time.deltaTime;

            if (invincibilityBlinkDelay > 0)
                invincibilityBlinkDelay -= Time.deltaTime;
            else
            {
                if (sprite.gameObject.activeSelf == true)
                    sprite.gameObject.SetActive(false);
                else
                    sprite.gameObject.SetActive(true);

                invincibilityBlinkDelay = invincibilityBlink;
            }
        }
        else
            sprite.gameObject.SetActive(true);
    }

    /// <summary> Deal damage to the entity. </summary>
    /// <param name="amount"> The amount of damage to deal. </param>
    public void Damage(int amount)
    {
        if(playerMovement.menuManager.GameStarted)
        {
            if (invincibility <= 0)
            {
                // If the player has a higher block chance, deal no damage. Otherwise, deal damage.
                // This does allow for the player to become invincible if they get very lucky.
                if (Random.Range(0f, 1f) > blockChance)
                {
                    hp -= amount;
                    hp = Mathf.Clamp(hp, 0, maxHp);
                    playerMovement.menuManager.damageTaken += 1;

                    // NOT FAMILY FRIENDLY!
                    if (hp == 0)
                        Die();
                    else
                        Instantiate(hitEffect, transform.position, Quaternion.identity);
                }
                invincibility = invincibilityTime;

                StartCoroutine(shake.Shake(screenShakeDuration, screenShakeMagnitude * amount));
            }
        }
    }

    /// <summary> Heals the entiy by the assigned amount. </summary>
    /// <param name="Heal Amount"> The amount to heal. </param>
    public void Heal(int amount)
    {
        if (hp + amount <= maxHp)
        {
            hp += amount;
            if (hp > maxHp)
                hp = maxHp;
        }
    }
    
    /// <summary> Begins death effects. </summary>
    void Die()
    {
        StartCoroutine(shake.Shake(screenShakeDuration * 2, screenShakeMagnitude * 2));
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        playerMovement.menuManager.menuClass.SetActive(true);
        StartCoroutine(playerMovement.menuManager.EnableMenu(true));
    }
}
