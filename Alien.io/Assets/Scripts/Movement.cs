using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10f;
    [SerializeField] private float _rotationSpeed = 10f;

    public bool isStopped = false;

    private float _maxPosX;
    private float _minPosX;
    private float _maxPosZ;
    private float _minPosZ;
    private Vector3 _direction;
    private Vector3 _dir;

    private Vector2 _startPos;
    private Vector2 _directionSwipe;

    private Spawner _spawner;

    private void Awake()
    {
        _spawner = GameObject.FindObjectOfType<Spawner>();
        _directionSwipe = new Vector3(0, 0.5f);
        _dir = new Vector3(_directionSwipe.x, 0, _directionSwipe.y).normalized;
    }

    public void OnStartMoving()
    {
        _maxPosX = _spawner.SpawnPosCenter.x + _spawner.MaxSpawnPos;
        _minPosX = _spawner.SpawnPosCenter.x - _spawner.MaxSpawnPos;
        _maxPosZ = _spawner.SpawnPosCenter.z + _spawner.MaxSpawnPos;
        _minPosZ = _spawner.SpawnPosCenter.z - _spawner.MaxSpawnPos;
        isStopped = false;
    }

    private void Update()
    {
        if (isStopped)
        {
            return;
        }
        
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    _startPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    _directionSwipe = touch.position - _startPos;
                    _dir = new Vector3(_directionSwipe.x, 0, _directionSwipe.y).normalized;
                    RotatePlayer(_dir);
                    break;

                case TouchPhase.Ended:
                    break;
            }
        }
        
        _direction = _dir * Time.deltaTime * _moveSpeed;
        transform.Translate(_direction.x, 0, _direction.z, Space.World);
        
        if (transform.position.x > _maxPosX)
        {
            transform.position = new Vector3(_maxPosX, transform.position.y, transform.position.z);
        }
        else if (transform.position.x < _minPosX)
        {
            transform.position = new Vector3(_minPosX, transform.position.y, transform.position.z);
        }

        if (transform.position.z > _maxPosZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _maxPosZ);
        }
        else if (transform.position.z < _minPosZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _minPosZ);
        }
    }
    
    private void RotatePlayer(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            foreach(Runner runner in transform.GetComponentsInChildren<Runner>())
            {
                runner.RotateRunner(direction, _rotationSpeed);
            }
        }
    }

    public void RotateAllPlayers()
    {
        if (_dir != Vector3.zero)
        {
            foreach (Runner runner in transform.GetComponentsInChildren<Runner>())
            {
                runner.RotateRunner(_dir, _rotationSpeed);
            }
        }
    }
}
