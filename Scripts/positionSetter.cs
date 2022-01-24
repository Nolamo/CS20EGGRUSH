using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
