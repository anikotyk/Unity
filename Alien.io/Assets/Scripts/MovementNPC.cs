using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNPC : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 8f;
    [SerializeField] private float _rotationSpeed = 10f;

    public bool isStopped = false;

    private float _maxPosX;
    private float _minPosX;
    private float _maxPosZ;
    private float _minPosZ;

    private float _timer = 0;
    private float _timeToChangeDirection = 0;

    private float _dirX = 0;
    private float _dirZ = 0;
    private Vector3 _direction;
    private Vector3 _dir;

    private Spawner _spawner;

    private void Awake()
    {
        _spawner = GameObject.FindObjectOfType<Spawner>();
    }

    public void OnStartMoving()
    {
        ChangeDirection();
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

        _direction = _dir * Time.deltaTime * _moveSpeed;
        transform.Translate(_direction.x, 0, _direction.z, Space.World);
        //RotatePlayer(_direction);

        _timer += Time.deltaTime;
        if (_timer >= _timeToChangeDirection)
        {
            ChangeDirection();
        }

        if (transform.position.x > _maxPosX)
        {
            transform.position = new Vector3(_maxPosX, transform.position.y, transform.position.z);
            ChangeDirection();
        }
        else if (transform.position.x < _minPosX)
        {
            transform.position = new Vector3(_minPosX, transform.position.y, transform.position.z);
            ChangeDirection();
        }

        if (transform.position.z > _maxPosZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _maxPosZ);
            ChangeDirection();
        }
        else if (transform.position.z < _minPosZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _minPosZ);
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        _dirX = Random.Range(-10, 11) * 0.1f;
        _dirZ = Random.Range(-10, 11) * 0.1f;

        _dir = new Vector3(_dirX, 0, _dirZ).normalized;
        RotatePlayer(_dir);
        _timeToChangeDirection = Random.Range(100, 500) * 0.01f;
        _timer = 0;
    }

    private void RotatePlayer(Vector3 directionRotate)
    {
        if (directionRotate != Vector3.zero)
        {
            foreach (Runner runner in transform.GetComponentsInChildren<Runner>())
            {
                runner.RotateRunner(directionRotate, _rotationSpeed);
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
