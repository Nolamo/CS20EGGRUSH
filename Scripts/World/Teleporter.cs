using UnityEngine;

// used on the chicken coops.
public class Teleporter : MonoBehaviour
{
    [SerializeField] Transform exit;

    // if the player enters the collider, they will be teleported to the associated exit. 
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
            collision.gameObject.transform.position = new Vector2(exit.transform.position.x, collision.gameObject.transform.position.y);
    }
}
