using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomNameGeneratorLibrary;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform _squadParent;
    [SerializeField] private Transform _bonusesParent;

    [SerializeField] private GameObject _squadPrefab;

    public Vector3 _spawnPosCenter;
    public float _maxSpawnPos = 75;

    [SerializeField] private int _maxSquadsCount = 4;
    //[SerializeField] private int _minSquadsCount = 2;

    [SerializeField] private int _maxBonusCount = 5;
    [SerializeField] private int _minBonusCount = 2;

    [SerializeField] private GameObject[] bonusesList;

    [SerializeField] private int _maxSquadsCountAtAll = 10;
    [SerializeField] private int _maxBonusesCountAtAll = 20;

    [SerializeField] private LayerMask runnersLayer;
    [SerializeField] private float bonusRad = 2;

    private float timerBonus = 0;
    private float timeBeforeBonus;

    private float timerNewSquad = 0;
    private float timeBeforeNewSquad;

    public PersonNameGenerator pGen;

    [SerializeField] private float _spawnPosCenterXMin;
    [SerializeField] private float _spawnPosCenterXMax;
    [SerializeField] private float _spawnPosCenterZMin;
    [SerializeField] private float _spawnPosCenterZMax;

    public bool isStopped = false;


    private void Awake()
    {
        pGen = new RandomNameGeneratorLibrary.PersonNameGenerator();
    }

    public void StartGame()
    {
        _spawnPosCenter = new Vector3(Random.Range(_spawnPosCenterXMin, _spawnPosCenterXMax), 0, Random.Range(_spawnPosCenterZMin, _spawnPosCenterZMax));

        for (int i = 0; i < _maxSquadsCount / 2; i++)
        {
            SpawnSquad(1, 20, true);
        }

        for (int i = 0; i < _maxSquadsCount / 2; i++)
        {
            SpawnSquad(1, 20, false);
        }

        GameObject.FindObjectOfType<GameController>().GetTopPlayers();

        int countBonuses = Random.Range(_minBonusCount, _maxBonusCount);
        for (int i = 0; i < countBonuses; i++)
        {
            SpawnBonus();
        }
        timeBeforeBonus = Random.Range(50, 500) * 0.01f;
        timeBeforeNewSquad = Random.Range(15, 60);

        

    }

    private void Update()
    {
        if (isStopped)
        {
            return;
        }

        timerBonus += Time.deltaTime;
        if(timerBonus>= timeBeforeBonus)
        {
            SpawnBonus();
            timerBonus = 0;
            timeBeforeBonus = Random.Range(50, 500) * 0.01f;
        }

        if(_squadParent.childCount< _maxSquadsCountAtAll)
        {
            timerNewSquad += Time.deltaTime;
            if (timerNewSquad >= timeBeforeNewSquad)
            {
                SpawnSquad(1, 3, _squadParent.childCount % 2 == 0);
                timerNewSquad = 0;
                timeBeforeNewSquad = Random.Range(15, 60);
            }
        }
        
    }

    public void EndGame()
    {
        ClearChildren(_squadParent);
        ClearChildren(_bonusesParent);
    }

    public void ClearChildren(Transform obj)
    {
        for(int i=0; i< obj.childCount; i++)
        {
            Destroy(obj.GetChild(i).gameObject);
        }
    }

    public void SpawnSquad(int minRunners, int maxRunners, bool isHuman, int deep=0)
    {
        //Debug.Log(deep);
        if (_squadParent.childCount >= _maxSquadsCountAtAll || deep>=3)
        { return; }
        
        Vector3 pos = new Vector3(Random.Range(-_maxSpawnPos * 100, _maxSpawnPos * 100) * 0.01f + _spawnPosCenter.x, 0, Random.Range(-_maxSpawnPos * 100, _maxSpawnPos * 100) * 0.01f + _spawnPosCenter.z);
        int toAdd = Random.Range(minRunners, maxRunners);
        float rad = 0.7f * Mathf.Sqrt(toAdd);

        if (!Physics.CheckSphere(pos, rad+4, runnersLayer))
        {
            GameObject squad = Instantiate(_squadPrefab, _squadParent);
            squad.transform.localPosition = pos;
            squad.transform.localRotation = Quaternion.Euler(Vector3.zero);

            squad.GetComponent<AddRuners>().IsHuman = isHuman;
            squad.GetComponent<AddRuners>().AddRunners(toAdd);
        }
        else
        {
            SpawnSquad(minRunners, maxRunners, isHuman, deep + 1);
        }

        
    }

    private void SpawnBonus(int deep=0)
    {
        //if(_bonusesParent.transform.childCount >= _maxBonusesCountAtAll) { return; }
        
        if (deep > 3)
        {
            return;
        }

        Vector3 pos = new Vector3(Random.Range(-_maxSpawnPos * 100, _maxSpawnPos * 100) * 0.01f + _spawnPosCenter.x, 0, Random.Range(-_maxSpawnPos * 100, _maxSpawnPos * 100) * 0.01f + _spawnPosCenter.z);

        GameObject bonusToSpawn = bonusesList[Random.Range(0, bonusesList.Length)];
        GameObject bonusToSpawnSize = bonusToSpawn;
        if (!bonusToSpawnSize.GetComponent<BonusCollider>())
        {
            bonusToSpawnSize = bonusToSpawn.GetComponentInChildren<BonusCollider>().gameObject;
        }
        //Debug.Log(pos);
        if (!Physics.CheckSphere(pos, bonusToSpawnSize.GetComponent<MeshRenderer>().bounds.size.x, runnersLayer))
        {
            GameObject bonus = Instantiate(bonusToSpawn, _bonusesParent);
            bonus.transform.position = pos;
            bonus.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
        else
        {
            SpawnBonus(deep + 1);
        }
    }
}
