using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    [SerializeField] private GameObject[] obstaclesPrefabs;
    [SerializeField] private Transform obstaclesParent;
    [SerializeField] private Vector3 startPosSpawn;
    [SerializeField] private Transform finish;
    [SerializeField] private float differenceBetweenPosSpawn;

    private float endPosSpawnX;

    private void Awake()
    {
        endPosSpawnX = obstaclesParent.InverseTransformPoint(finish.position).x;
    }

    private void Start()
    {
        GameController.Instance.LevelStarted += SpawnNewLevel;
        GameController.Instance.LevelEnded += ClearObstacles;
    }

    private void OnDestroy()
    {
        GameController.Instance.LevelStarted -= SpawnNewLevel;
        GameController.Instance.LevelEnded -= ClearObstacles;
    }

    private void SpawnNewLevel()
    {
        Vector3 positionToSpawn = startPosSpawn;
        while ((differenceBetweenPosSpawn < 0 && (endPosSpawnX - positionToSpawn.x) <= differenceBetweenPosSpawn) || (differenceBetweenPosSpawn >= 0 && (endPosSpawnX - positionToSpawn.x) >= differenceBetweenPosSpawn))
            {
            int index = Random.Range(0, obstaclesPrefabs.Length);
            GameObject newObstacle = Instantiate(obstaclesPrefabs[index]);
            Vector3 localPos = obstaclesPrefabs[index].transform.localPosition;
            newObstacle.transform.SetParent(obstaclesParent);
            localPos.x = positionToSpawn.x;
            newObstacle.transform.localPosition = localPos;
            positionToSpawn.x += differenceBetweenPosSpawn;
        }
    }

    private void ClearObstacles()
    {
        for (int i = 0; i < obstaclesParent.childCount; i++)
        {
            Destroy(obstaclesParent.GetChild(i).gameObject);
        }
    }
}
