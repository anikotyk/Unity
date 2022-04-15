using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObstacle : MonoBehaviour
{
    public float _speed = 20f;
    private float endposX = 8;
    
    public void OnEnable()
    {
        _speed = GameObject.FindObjectOfType<GameController>().speedMoveObstacles;
    }

    private void OnDisable()
    {
        StopMove();
    }

    public float CalculateTime()
    {
        return (GetDistance() / _speed);
    }

    public float GetDistance()
    {
        return (endposX - transform.localPosition.x);
    }

    public void StartMove()
    {
        LeanTween.moveLocalX(gameObject, endposX, CalculateTime());
    }

    public void StopMove()
    {
        LeanTween.pause(gameObject);
    }
}
