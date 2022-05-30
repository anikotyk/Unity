using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenStateChanger : MonoBehaviour
{
    [SerializeField] private bool _isOpen;
    [SerializeField] private float _openSpeed = 0.3f;

    [SerializeField] private Vector3 _openPosition;
    [SerializeField] private Vector3 _closedPosition;

    [SerializeField] private Vector3 _openRotation;
    [SerializeField] private Vector3 _closedRotation;

    public void ChangeOpenState()
    {
        if (_isOpen)
        {
            Close();
        }
        else
        {
            Open();
        }
    }

    public void Open()
    {
        _isOpen = true;
        LeanTween.moveLocal(gameObject, _openPosition, _openSpeed);
        LeanTween.rotateLocal(gameObject, _openRotation, _openSpeed);
    }

    public void Close()
    {
        _isOpen = false;
        LeanTween.moveLocal(gameObject, _closedPosition, _openSpeed);
        LeanTween.rotateLocal(gameObject, _closedRotation, _openSpeed);
    }
}
