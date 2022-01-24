using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Follow : MonoBehaviour
{

    public Transform player;
    private Vector3 velocity = Vector3.zero;
    public float smoothSpeed = 0.125f;
    public Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //Vector3 desiredPosition = player.position + offset;
        //Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        //transform.position = smoothedPosition;


    }

    void Update()
    {

        Vector3 targetPos = new Vector3(player.position.x, player.position.y, -10);
        transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, 0.2f);

    }
}
