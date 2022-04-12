using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Vector3 _direction;
    [SerializeField] private Vector3 _offset;

    private float _smoothSpeed = 2f;
    private Vector3 _newPos;

    public float coefHowNeadCam=15f;

    void LateUpdate()
    {
        if (_target == null) return;

        _newPos = _target.transform.position + _direction + _offset;
        transform.position = Vector3.Lerp(transform.position, _newPos, _smoothSpeed);
    }


    public void SetCameraOffset(float coef)
    {
        _direction = _direction.normalized * coef* coefHowNeadCam;
    }

    public void SetOffsetX(float x)
    {
        _offset = new Vector3(x, _offset.y, _offset.z);
    }


}
