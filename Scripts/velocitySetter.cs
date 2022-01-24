using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class velocitySetter : MonoBehaviour
{
    // Start is called before the first frame update

    public Vector2 velocity;
    [SerializeField] Duck parent;
    bool duckCheck = false;

    void Start()
    {
        StartCoroutine(WaitForDuckCheck());
    }

    IEnumerator WaitForDuckCheck()
    {
        yield return new WaitUntil(() => duckCheck);
        Rigidbody2D rb = this.GetComponent<Rigidbody2D>();
        rb.velocity = transform.rotation * new Vector3(velocity.x, velocity.y, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (this.name == ("DuckEgg(Clone)") && !parent || this.name == "DuckEggLow(Clone)" && !parent || this.name == "DuckEggArtilery(Clone)" && !parent)
        {
            parent = collision.GetComponentInParent<Duck>();
            if (parent.GetComponent<SpriteRenderer>().flipX)
            {
                velocity.x = -velocity.x;
            }
        }
        duckCheck = true;
    }
}
