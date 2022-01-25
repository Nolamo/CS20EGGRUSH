using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary> Attached to the player and controls the player's movement and attacks. It also serves to communicate with several other Player scripts. </summary>
[RequireComponent(typeof(MenuManager))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Systems & Classes")]
    public MenuManager menuManager; // Used to check for if the game has started/ended (when the player dies)

    [Header("Input Systems")]
    [Tooltip("Custom Input Manager system, should be on the same GameObject.")]
    public bool InputSetupFinished = false; // Used to check if the input manager has finished setting up

    [Header("Player Info")]
    public float speed;
    public float airControl;
    public float jumpForce;
    private float moveInput;
    float _moveInputRaw;
    float decellerationDelay;
    public float decellerationTime = 0.5f;
    public Health hp;

    public Rigidbody2D rb2d;

    [Header("Ground Check")]
    [SerializeField] private bool isGrounded;
    public Transform groundCheck;
    public float checkRadius;
    public LayerMask whatIsGround;

    [Header("Attack Info")]
    public GameObject attackCollider;
    public bool attacking = false;
    public float attackSpeed;
    public float stopAttackThreshold = -2f;
    //pauses the game for a small amount of time to make it feel stronger
    public float pauseTime = 0.017f;
    float pauseDelay;

    public int damage;
    [SerializeField] private int trueDamage;

    [Header("Particle Effects")]
    public GameObject attackEffect;
    public List<GameObject> killEffect = new List<GameObject>();
    public List<GameObject> missEffect = new List<GameObject>();
    public ParticleSystem trailParticle;
    public float trailEmission;
    float particleDelay;

    [Header("Food Info")]
    public GameObject cooked;
    public float cookChance;

    [Header("Upgrades")]
    public LayerMask whatIsUpgrade;
    bool dayUpgradeActive;
    public int dayDoubleJumps;
    public int nightDoubleJumps;
    int _jumpsLeft;
    int trueDoubleJumps;
    public float knockbackStrength;
    public float stunTime;
    public int dayDamageUpgrade;
    public int nightDamageUpgrade;

    public Text titleText;
    public Text descText;

    public PowerUp PowerUp;

    [Header("Sprite Info")]
    public SpriteRenderer eye;
    public GameObject attackEffects;
    public Sprite eyeOpen;
    public Sprite eyeClosed;

    [Header("Day Night Cycle")]
    public DayNight dayNight;

    [Header("Screenshake")]
    public float screenShakeMagnitude;
    public float screenShakeDuration;
    [HideInInspector]
    public ScreenShake shake;

    // get stuff
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
        shake = Camera.main.GetComponentInParent<ScreenShake>();
    }

    private void FixedUpdate()
    {
        // only run if the game has started
        if(menuManager.GameStarted)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - 0.5f), Vector2.up, 15, whatIsUpgrade);
            Debug.DrawRay(transform.position, Vector3.up, Color.red, 0.5f);

            // if the player hits a powerup, get it.
            if (hit)
                PowerUp = hit.collider.gameObject.GetComponent<PowerUp>();
            else
                PowerUp = null;

            if (PowerUp)
            {
                titleText.text = PowerUp.itemTitle;
                descText.text = PowerUp.itemDesc;
            }
            else
            {
                titleText.text = "";
                descText.text = "";
            }

            decellerationDelay = Mathf.Clamp(decellerationDelay, 0, decellerationTime);

            isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkRadius, whatIsGround);

            moveInput = Input.GetAxis("Horizontal");
            _moveInputRaw = Input.GetAxisRaw("Horizontal");
            if (Mathf.Abs(moveInput) > 0.1f)
            {
                rb2d.velocity = new Vector2(moveInput * speed, rb2d.velocity.y);
                decellerationDelay = 0;
            }

            else if (Mathf.Abs(moveInput) < 0.1f)
            {
                decellerationDelay += Time.deltaTime;
                Vector2.Lerp(rb2d.velocity, new Vector2(0, rb2d.velocity.y), decellerationDelay / decellerationTime);

            }
        }
    }

    void ActivateNight()
    {
        dayUpgradeActive = false;
    }

    void ActivateDay()
    {
        dayUpgradeActive = true;
    }

    private void Update()
    {
        if(menuManager.GameStarted)
        {
            if (isGrounded)
            {
                _jumpsLeft = trueDoubleJumps;
            }

            if (dayUpgradeActive)
            {
                trueDoubleJumps = 0 + dayDoubleJumps;
                trueDamage = damage + dayDamageUpgrade;
            }
            else
            {
                trueDoubleJumps = 0 + nightDoubleJumps;
                trueDamage = damage + nightDamageUpgrade;
            }

            if (dayNight.isNotDay)
            {
                ActivateNight();
            }
            else if (!dayNight.isNotDay)
            {
                ActivateDay();
            }

            if (attacking)
            {
                if (particleDelay > 0)
                {
                    particleDelay -= Time.deltaTime;
                }
                else
                {
                    particleDelay = 1 / trailEmission;
                    trailParticle.Emit(1);
                }

                attackEffects.SetActive(true);
                eye.sprite = eyeOpen;
            }
            else
            {
                attackEffects.SetActive(false);
                eye.sprite = eyeClosed;
            }
            
            // check if jump is pressed and if the player is grounded, jump
            if (Input.GetButtonDown("Jump") || Input.GetButtonDown("Jump2"))
            {
                if (isGrounded)
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);

                // if the player can double jump, double jump
                else if (!isGrounded && _jumpsLeft > 0)
                {
                    _jumpsLeft -= 1;
                    rb2d.velocity = new Vector2(rb2d.velocity.x, jumpForce);
                }

            }

            // if the player is not grounded and attack button(s) is pressed, attack
            if (Input.GetButtonDown("Fire1") && isGrounded == false || Input.GetButtonDown("Fire2") && isGrounded == false || Input.GetButtonDown("Fire3") && isGrounded == false || Input.GetButtonDown("Fire4") && isGrounded == false)
            {
                pauseDelay = pauseTime;

                rb2d.velocity = new Vector2(rb2d.velocity.x, -attackSpeed);
                attackCollider.SetActive(true);
                attacking = true;
            }
            if (attacking && rb2d.velocity.y > stopAttackThreshold)
            {
                attackCollider.SetActive(false);
                attacking = false;
            }

            // delay before resuming the game
            if (pauseDelay > 0)
            {
                pauseDelay -= Time.unscaledDeltaTime;
                Time.timeScale = 0;
            }
            else
                Time.timeScale = 1;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "Enemy" && attacking == true)
        {
            rb2d.velocity = Vector2.up * jumpForce * 0.75f;
            StartCoroutine(shake.Shake(screenShakeDuration, screenShakeMagnitude));


            foreach(ContactPoint2D contact in collision.contacts)
            {
                

                Instantiate(attackEffect, contact.point, Quaternion.identity);
                EnemyHealth enemy = collision.gameObject.GetComponent<EnemyHealth>();

                if(enemy.health - trueDamage <= 0)
                {
                    if(Random.Range(0f, 1f) < cookChance)
                    {
                        Instantiate(cooked, collision.gameObject.transform.position, Quaternion.identity);
                    }

                    if(killEffect.ToArray().Length > 0)
                    {
                        foreach (GameObject k in killEffect)
                        {
                            Instantiate(k, collision.gameObject.transform.position, Quaternion.identity);
                        }
                    }
                }

                Vector2 knockback = collision.gameObject.transform.position - transform.position;


                enemy.Damage(trueDamage, stunTime, knockback.normalized * knockbackStrength);
            }
        }
        else if(collision.gameObject.tag == "Ground" && attacking == true)
        {
            hp.Damage(1);
            menuManager.selfHarmTaken += 1;
            StartCoroutine(shake.Shake(screenShakeDuration, screenShakeMagnitude));
        }
        else if(collision.gameObject.tag == "Upgrade" && attacking == true)
        {
            rb2d.velocity = Vector2.up * jumpForce * 0.75f;

            PowerUp p = collision.gameObject.GetComponent<PowerUp>();
            p.TakePowerUp(this);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(groundCheck.position, checkRadius);
    }

    /// <summary> When this function is called, the Player's stats are reset. </summary>
    /// <param name="resetPos"> Should position be reset? Used for the credits. (default: false) </param>
    public void ResetPlayer(bool resetPos)
    {
        if(resetPos)
        {
            rb2d.velocity = new Vector2(0, 0);
            this.transform.position = new Vector3(0, 3, 0);
        }
        isGrounded = true;
        hp.hp = 5;
        speed = 10;
        jumpForce = 12;
        dayDamageUpgrade = 0;
        nightDamageUpgrade = 0;
        rb2d.gravityScale = 2;
        dayDoubleJumps = 0;
        nightDoubleJumps = 0;
        hp.blockChance = 0;
        cookChance = 0;
        hp.maxHp = 5;
        dayNight.nightSpeed = 3;
        dayNight.daySpeed = 3;
        hp.invincibility = 2;
    }
}