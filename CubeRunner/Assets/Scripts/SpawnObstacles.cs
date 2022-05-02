using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    [SerializeField] private GameObject[] _obstaclesPrefabs;
    [SerializeField] private Transform _obstaclesContainer;
    [SerializeField] private Vector3 _startPositionSpawn;
    [SerializeField] private Transform _finish;
    [SerializeField] private float _differenceBetweenSpawnPositions = -15;

    private float _endSpawnPositionX;

    private void Awake()
    {
        _endSpawnPositionX = _obstaclesContainer.InverseTransformPoint(_finish.position).x;
    }

    private void Start()
    {
        GameController.Instance.LevelStarted += SpawnNewLevel;
        GameController.Instance.LevelEnded += ClearObstaclesContainer;
    }

    private void OnDestroy()
    {
        GameController.Instance.LevelStarted -= SpawnNewLevel;
        GameController.Instance.LevelEnded -= ClearObstaclesContainer;
    }

    private void SpawnNewLevel()
    {
        Vector3 positionToSpawn = _startPositionSpawn;
        while ((_differenceBetweenSpawnPositions < 0 && (_endSpawnPositionX - positionToSpawn.x) <= _differenceBetweenSpawnPositions) || (_differenceBetweenSpawnPositions >= 0 && (_endSpawnPositionX - positionToSpawn.x) >= _differenceBetweenSpawnPositions))
            {
            int index = Random.Range(0, _obstaclesPrefabs.Length);
            GameObject newObstacle = Instantiate(_obstaclesPrefabs[index]);
            Vector3 localPos = _obstaclesPrefabs[index].transform.localPosition;
            newObstacle.transform.SetParent(_obstaclesContainer);
            localPos.x = positionToSpawn.x;
            newObstacle.transform.localPosition = localPos;
            positionToSpawn.x += _differenceBetweenSpawnPositions;
        }
    }

    private void ClearObstaclesContainer()
    {
        for (int i = 0; i < _obstaclesContainer.childCount; i++)
        {
            Destroy(_obstaclesContainer.GetChild(i).gameObject);
        }
    }
}
