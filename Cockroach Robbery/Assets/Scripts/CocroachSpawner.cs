using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocroachSpawner : MonoBehaviour
{
    [SerializeField] private GameObject cocroachPrefab;
    [SerializeField] private Transform cocroachesContainer;

    [SerializeField] private float xMinSpawn;
    [SerializeField] private float xMaxSpawn;
    [SerializeField] private float zMinSpawn;
    [SerializeField] private float zMaxSpawn;

    [SerializeField] private float ySpawn;

    [SerializeField] private int countMinSpawnAtStart;
    [SerializeField] private int countMaxSpawnAtStart;
    
    [SerializeField] private int countMaxSpawnAtWave;

    [SerializeField] private LayerMask _cocroachLayer;
    


    public static CocroachSpawner Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        CocroachesCountControl.Instance.LevelLose += ClearCocroachesContainer;
        WavesController.Instance.LevelWin += ClearCocroachesContainer;
        WavesController.Instance.WaveSpawn += SpawnNewWave;
        GameController.Instance.StartLevel += SpawnLevel;
    }

    private void OnDestroy()
    {
        CocroachesCountControl.Instance.LevelLose -= ClearCocroachesContainer;
        WavesController.Instance.LevelWin -= ClearCocroachesContainer;
        WavesController.Instance.WaveSpawn -= SpawnNewWave;
        GameController.Instance.StartLevel -= SpawnLevel;
    }

    private void SpawnLevel()
    {
        SpawnCocroaches(Random.Range(countMinSpawnAtStart, countMaxSpawnAtStart));
    }

    private void SpawnNewWave()
    {
        SpawnCocroaches(Mathf.Min(countMaxSpawnAtWave, CocroachesCountControl.Instance.LoseCocroachesCount/(WavesController.Instance.WavesCount+1)));
    }

    private void SpawnCocroaches(int cnt)
    {
        for(int i=0; i<cnt; i++)
        {
            GameObject newCocroach = Instantiate(cocroachPrefab);
            newCocroach.transform.SetParent(cocroachesContainer);
            Vector3 pos = GetRandomPositionInRoom();
            int cnterror = 0;
            while (Physics.CheckSphere(pos, newCocroach.transform.localScale.z / 2, _cocroachLayer))
            {
                pos = GetRandomPositionInRoom();
                cnterror++;
                if (cnterror >= 10)
                {
                    break;
                }
            }

            newCocroach.transform.localPosition = pos;
            newCocroach.GetComponent<CocroachGrowth>().GenerateRandomSize();
            newCocroach.GetComponent<CocroachMovement>().SetDirection();
        }
    }

    private Vector3 GetRandomPositionInRoom()
    {
        return new Vector3(Random.Range(xMinSpawn, xMaxSpawn), ySpawn, Random.Range(zMinSpawn, zMaxSpawn));
    }

    public void SpawnCocroachAtPosWithDirection(Vector3 pos, Vector3 normal)
    {
        GameObject newCocroach = Instantiate(cocroachPrefab);
        //start life
        newCocroach.transform.SetParent(cocroachesContainer);
        newCocroach.transform.localPosition = pos;
        newCocroach.GetComponent<CocroachGrowth>().SetMinSize();
        newCocroach.GetComponent<CocroachMovement>().SetDirection(normal);
    }

    private void ClearCocroachesContainer()
    {
        for(int i = 0; i < cocroachesContainer.childCount; i++)
        {
            Destroy(cocroachesContainer.GetChild(i).gameObject);
        }
    }
}
