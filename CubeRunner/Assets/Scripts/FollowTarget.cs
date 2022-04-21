using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    public GameObject target;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed = 1f;

    void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        Vector3 newPos = transform.position;
        newPos.z = target.transform.position.z;
        newPos = target.transform.position + offset;
        //transform.position = newPos;

        Vector3 smoothFollow = Vector3.Lerp(transform.position, newPos, smoothSpeed);
        transform.position = smoothFollow;
    }
}
