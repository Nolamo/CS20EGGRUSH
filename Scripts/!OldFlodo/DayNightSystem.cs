using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightSystem : MonoBehaviour
{

    //very epic, better than skyrim's or minecraft's

    public SpriteRenderer spr;
    public Sprite hurtSprite;
    public Rigidbody2D rb2d;
    public Animator anim;
    public CircleCollider2D circle;

    public GameObject sun;
    public GameObject moon;
    public GameObject background;

    public Sprite dayBG;
    public Sprite nightBG;

    public bool isDay;

    //public Transform startPosition;

    // Start is called before ur mom
    void Start()
    {

        spr = GetComponent<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        circle = GetComponent<CircleCollider2D>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Attack"))
        {

            circle.enabled = false;
            anim.enabled = false;
            spr.sprite = hurtSprite;
            rb2d.gravityScale = 2;
            rb2d.constraints = RigidbodyConstraints2D.None;

            Invoke("SwitchTime", 3f);
            Destroy(gameObject, 4f);

        }

    }

    void SwitchTime()
    {

        isDay = !isDay;

        if(isDay == true)
        {

            Instantiate(sun, new Vector3(-10, 6, 0), Quaternion.identity);
            background = GameObject.Find("Background");
            background.GetComponent<SpriteRenderer>().sprite = dayBG;

        }
        else if(isDay == false)
        {

            Instantiate(moon, new Vector3(-10, 6, 0), Quaternion.identity);
            background = GameObject.Find("Background");
            background.GetComponent<SpriteRenderer>().sprite = nightBG;

        }

    }


}
