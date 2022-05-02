using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class FollowTarget : MonoBehaviour
{
    [SerializeField] private Vector3 _offset;
    [SerializeField] private float _smoothSpeed = 1f;

    private GameObject _target;
    private Vector3 _newPos;

    private void LateUpdate()
    {
        if (_target == null)
        {
            return;
        }

        _newPos = transform.position;
        _newPos.z = _target.transform.position.z;
        _newPos = _target.transform.position + _offset;
        
        transform.position = Vector3.Lerp(transform.position, _newPos, _smoothSpeed);
    }

    public void RemoveTarget()
    {
        _target = null;
    }

    public void SetTarget(GameObject target)
    {
        _target = target;
    }
}
