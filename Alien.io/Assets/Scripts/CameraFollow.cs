using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject _target;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private Vector3 _offset;

    private float smoothSpeed = 1f;
    private Vector3 newPos;

    void LateUpdate()
    {
        if (_target == null) return;

        newPos = _target.transform.position + _direction + _offset;
        transform.position = Vector3.Lerp(transform.position, newPos, smoothSpeed);
    }


    public void SetCameraOffset(float coef)
    {
        _direction = _direction.normalized * coef;
    }

    public void SetOffsetX(float x)
    {
        _offset = new Vector3(x, _offset.y, _offset.z);
    }
}
