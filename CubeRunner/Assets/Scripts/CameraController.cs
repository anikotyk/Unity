using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    private float smoothSpeed=1f;

    void LateUpdate()
    {
        Vector3 newPos = transform.position;
        newPos.z = player.transform.position.z;
        //transform.position = newPos;

        Vector3 smoothFollow = Vector3.Lerp(transform.position, newPos, smoothSpeed);
        transform.position = smoothFollow;
    }
}
