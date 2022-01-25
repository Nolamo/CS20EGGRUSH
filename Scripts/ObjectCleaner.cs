using System.Collections;
using UnityEngine;

/// <summary> Deletes the object after a certain amount of time. Used for cleaning up the hierachy from dead instantiated objects. </summary>
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
