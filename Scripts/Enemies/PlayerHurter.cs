using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHurter : MonoBehaviour
{
    public int damage;

    public bool destroyOnContact;

    public GameObject destroyEffect;

    public hurterType hurtType;

    public string[] destroyedBy;

    public enum hurterType
    {
        Collision, 
        Trigger
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(hurtType == hurterType.Trigger)
        {
            Health hp = collision.gameObject.GetComponent<Health>();
            if (hp)
            {
                hp.Damage(damage);
            }

            for (int i = 0; i < destroyedBy.Length; i++)
            {
                if (collision.gameObject.tag == destroyedBy[i] && destroyOnContact)
                {
                    if (destroyEffect)
                    {
                        Instantiate(destroyEffect, transform.position, Quaternion.identity);
                    }

                    Destroy(gameObject);
                }
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (hurtType == hurterType.Collision)
        {
            Health hp = collision.gameObject.GetComponent<Health>();
            if (hp)
            {
                hp.Damage(damage);
            }

            for (int i = 0; i < destroyedBy.Length; i++)
            {
                if(collision.gameObject.tag == destroyedBy[i] && destroyOnContact)
                {
                    if (destroyEffect)
                    {
                        Instantiate(destroyEffect, transform.position, Quaternion.identity);
                    }

                    Destroy(gameObject);
                }
            }
        }
    }
}
