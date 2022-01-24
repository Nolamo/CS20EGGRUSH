using System.Collections;
using UnityEngine;
public class ObjectCleaner : MonoBehaviour
{
    [Tooltip("Time until the gameobject is deleted")]
    public float timer = 1f;

    void Start()
    {
        StartCoroutine(CleanEffect());
    }

    IEnumerator CleanEffect()
    {
        yield return new WaitForSeconds(timer);
        Destroy(gameObject);
    }
}
