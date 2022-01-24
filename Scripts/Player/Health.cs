using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMovement))]
public class Health : MonoBehaviour
{
    public PlayerMovement playerMovement;

    /*
    [SerializeField] Image[] hearts;
    [SerializeField] Sprite fullHeart;
    [SerializeField] Sprite halfHeart;
    [SerializeField] Sprite emptyHeart;
    [SerializeField] Sprite fullHalfContainer;
    [SerializeField] Sprite emptyHalfContainer;
    */

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

    public List<Sprite> healthSprites = new List<Sprite>();

    private void Start()
    {
        shake = Camera.main.GetComponentInParent<ScreenShake>();
    }

    private void Update()
    {
        if(playerMovement.menuManager.GameStarted)
        {
            /*
            for (int i = 0; i < hearts.Length; ++i)
            {
                if (i <= Mathf.RoundToInt(maxHp / 2)) // Last is for half hearts.
                {
                    hearts[i].gameObject.SetActive(true);
                    hearts[i].sprite = fullHeart;
                    if (Mathf.RoundToInt(maxHp / 2) != (maxHp / 2)) // Enables half hearts
                    {
                        Debug.Log("Test");
                        hearts[i + 1].gameObject.SetActive(true);
                        if (maxHp / 2 == hp / 2) // If MaxHP and HP are the same, make it a full half container
                        {
                            hearts[i + 1].sprite = fullHalfContainer;
                        }
                        else // Else, make it an empty half container
                        {
                            hearts[i + 1].sprite = emptyHalfContainer;
                        }
                    }
                }
                else
                {
                    hearts[i].gameObject.SetActive(false);
                }

                if (Mathf.RoundToInt((maxHp - hp) / 2) != hp) // Half Full Sprite
                {
                    hearts[i].sprite = halfHeart;
                }

                //if(Mathf.RoundToInt((maxHp - hp) / 2) == hp) // Make that sprite EMPTY
                //{
                //    hearts[i].sprite = emptyHeart;
                //}

                if(i >= hearts.Length) // Repeat
                {
                    i = 0;
                }
            }
            */
        }

        if (invincibility > 0)
        {
            invincibility -= Time.deltaTime;

            if (invincibilityBlinkDelay > 0)
            {
                invincibilityBlinkDelay -= Time.deltaTime;
            }
            else
            {
                if (sprite.gameObject.activeSelf == true)
                {
                    sprite.gameObject.SetActive(false);
                }
                else
                {
                    sprite.gameObject.SetActive(true);
                }
                invincibilityBlinkDelay = invincibilityBlink;
            }
        }
        else
        {
            sprite.gameObject.SetActive(true);
        }
    }

    public void Damage(int amount)
    {
        if(playerMovement.menuManager.GameStarted)
        {
            if (invincibility <= 0)
            {
                if (Random.Range(0f, 1f) > blockChance)
                {

                    hp -= amount;
                    hp = Mathf.Clamp(hp, 0, maxHp);
                    playerMovement.menuManager.damageTaken += 1;

                    if (hp == 0)
                    {
                        Die();
                    }
                    else
                    {
                        Instantiate(hitEffect, transform.position, Quaternion.identity);

                        if (hp < 5)
                        {
                            sprite.sprite = healthSprites[Mathf.RoundToInt(hp) - 1];
                        }
                    }
                }
                invincibility = invincibilityTime;

                StartCoroutine(shake.Shake(screenShakeDuration, screenShakeMagnitude * amount));
            }
        }
    }

    public void Heal(int amount)
    {
        if (hp + amount <= maxHp)
        {
            hp += amount;
            if (hp > maxHp)
            {
                hp = maxHp;
            }
        }
        if (hp < 5)
        {
            sprite.sprite = healthSprites[Mathf.RoundToInt(hp)];
        }
    }
    
    void Die()
    {
        StartCoroutine(shake.Shake(screenShakeDuration * 2, screenShakeMagnitude * 2));
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        playerMovement.menuManager.menuClass.SetActive(true);
        StartCoroutine(playerMovement.menuManager.EnableMenu(true));
        //Destroy(gameObject);
    }
}
