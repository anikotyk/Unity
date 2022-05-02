using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 _distanceToMoveRight;
    [SerializeField] private Vector3 _distanceToMoveLeft;

    [SerializeField] private Vector3 _position;
    [SerializeField] private float _smoothSpeed = 1f;

    void LateUpdate()
    {
        if (_position == null) return;
        
        transform.position = Vector3.Lerp(transform.position, _position, _smoothSpeed);
    }

    public void MoveRight()
    {
        _position += _distanceToMoveRight;
    }

    public void MoveLeft()
    {
        _position += _distanceToMoveLeft;
    }
}
