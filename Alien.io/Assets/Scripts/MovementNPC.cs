using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementNPC : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;

    private float maxPosX;
    private float minPosX;
    private float maxPosZ;
    private float minPosZ;

    private float timer = 0;
    private float timeToChangeDirection = 0;

    private float dirX = 0;
    private float dirZ = 0;

    public bool isStopped = false;

    private void Start()
    {
        ChangeDirection();
        Spawner spawner = GameObject.FindObjectOfType<Spawner>();
        maxPosX = spawner._spawnPosCenter.x + spawner._maxSpawnPos;
        minPosX = spawner._spawnPosCenter.x - spawner._maxSpawnPos;
        maxPosZ = spawner._spawnPosCenter.z + spawner._maxSpawnPos;
        minPosZ = spawner._spawnPosCenter.z - spawner._maxSpawnPos;
    }

    private void Update()
    {
        if (isStopped)
        {
            return;
        }

        float xValue = dirX * Time.deltaTime * moveSpeed;
        float zValue = dirZ * Time.deltaTime * moveSpeed;
        transform.Translate(xValue, 0, zValue, Space.World);
        RotatePlayer(new Vector3(xValue, 0, zValue));

        timer += Time.deltaTime;
        if (timer >= timeToChangeDirection)
        {
            ChangeDirection();
        }

        if (transform.position.x > maxPosX)
        {
            transform.position = new Vector3(maxPosX, transform.position.y, transform.position.z);
            ChangeDirection();
        }
        else if (transform.position.x < minPosX)
        {
            transform.position = new Vector3(minPosX, transform.position.y, transform.position.z);
            ChangeDirection();
        }

        if (transform.position.z > maxPosZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, maxPosZ);
            ChangeDirection();
        }
        else if (transform.position.z < minPosZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, minPosZ);
            ChangeDirection();
        }
    }

    private void ChangeDirection()
    {
        dirX = Random.Range(-100, 100) * 0.01f;
        dirZ = Random.Range(-100, 100) * 0.01f;
        timeToChangeDirection = Random.Range(100, 500) * 0.01f;
        timer = 0;
    }

    void RotatePlayer(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            foreach (Runner runner in transform.GetComponentsInChildren<Runner>())
            {
                runner.RotateRunner(direction, rotationSpeed);
            }
        }
    }
}
