using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Vector3 offset;
    [SerializeField] private float smoothSpeed = 1f;

    private GameObject target;
    private Vector3 newPos;
    private Vector3 smoothFollow;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        newPos = transform.position;
        newPos.z = target.transform.position.z;
        newPos = target.transform.position + offset;

        smoothFollow = Vector3.Lerp(transform.position, newPos, smoothSpeed);
        transform.position = smoothFollow;
    }

    public void RemoveTarget()
    {
        target = null;
    }

    public void SetTarget(GameObject newTarget)
    {
        target = newTarget;
    }
}
