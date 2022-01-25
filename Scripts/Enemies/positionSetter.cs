using UnityEngine;

// Moves an object to specified place. Likely used on duck but I forget.
public class positionSetter : MonoBehaviour
{
    public Transform parent;
    public Vector3 offset;

    // Update is called once per frame
    void Update()
    {
        transform.position = parent.position + offset;
    }
}
