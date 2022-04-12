using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RandomNameGeneratorLibrary;

public class Spawner : MonoBehaviour
{
    [SerializeField] private Transform _squadParent;
    [SerializeField] private Transform _usedSquadParent;
    [SerializeField] private Transform _bonusesParent;
    [SerializeField] private Transform _usedBonusesParent;
    
    [SerializeField] private Transform _usedHumansParent;
    [SerializeField] private Transform _usedAliensParent;

    [SerializeField] private GameObject _humanRunnerPrefab;
    [SerializeField] private GameObject _alienRunnerPrefab;

    [SerializeField] private GameObject _squadPrefab;

    private Vector3 _spawnPosCenter;
    public Vector3 SpawnPosCenter=> _spawnPosCenter;

    [SerializeField] private float _maxSpawnPos = 75;
    public float MaxSpawnPos => _maxSpawnPos;

    [SerializeField] private int _maxSquadsCount = 4;

    [SerializeField] private int _maxBonusCount = 5;
    [SerializeField] private int _minBonusCount = 2;

    [SerializeField] private GameObject[] bonusesList;

    [SerializeField] private int _maxSquadsCountAtAll = 10;

    [SerializeField] private LayerMask _runnersLayer;

    private float _timerBonus = 0;
    private float _timeBeforeBonus;

    private float _timerNewSquad = 0;
    private float _timeBeforeNewSquad;

    public PersonNameGenerator pGen;

    [SerializeField] private float _spawnPosCenterXMin;
    [SerializeField] private float _spawnPosCenterXMax;
    [SerializeField] private float _spawnPosCenterZMin;
    [SerializeField] private float _spawnPosCenterZMax;

    public bool isStopped = false;

    private int _countHuman;
    private int _countAlien;


    private void Awake()
    {
        _countHuman = 0;
        _countAlien = 0;
        pGen = new RandomNameGeneratorLibrary.PersonNameGenerator();
    }

    public void StartGame()
    {
        _countHuman = 0;
        _countAlien = 0;

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
        _timeBeforeBonus = Random.Range(50, 500) * 0.01f;
        _timeBeforeNewSquad = Random.Range(5, 15);
        
    }

    private void Update()
    {
        if (isStopped)
        {
            return;
        }

        _timerBonus += Time.deltaTime;
        if(_timerBonus>= _timeBeforeBonus)
        {
            SpawnBonus();
            _timerBonus = 0;
            _timeBeforeBonus = Random.Range(50, 500) * 0.01f;
        }

        if(_squadParent.childCount < _maxSquadsCountAtAll)
        {
            _timerNewSquad += Time.deltaTime;
            if (_timerNewSquad >= _timeBeforeNewSquad)
            {
                SpawnSquad(1, 3, _squadParent.childCount % 2 == 0);
                _timerNewSquad = 0;
                _timeBeforeNewSquad = Random.Range(5, 15);
            }
        }
        
    }

    public void EndGame()
    {
        foreach (AddRuners addRuners in _squadParent.GetComponentsInChildren<AddRuners>())
        {
            KillSquad(addRuners);
        }

        foreach (BonusCollider bonus in _bonusesParent.GetComponentsInChildren<BonusCollider>())
        {
            RemoveBonus(bonus.ToDestroy);
        }
    }

    public void ClearRuners(Transform obj, bool isHuman)
    {
        Transform parent;
        if (isHuman)
        {
            parent = _usedHumansParent;
        }
        else
        {
            parent = _usedAliensParent;
        }

        foreach (RunnerData runnerData in obj.GetComponentsInChildren<RunnerData>())
        {
            RemoveRunner(runnerData.gameObject, parent);
        }
    }

    private void SpawnSquad(int minRunners, int maxRunners, bool isHuman, int deep=0)
    {
        if (_squadParent.childCount >= _maxSquadsCountAtAll || deep>=3)
        { return; }
        
        Vector3 pos = new Vector3(Random.Range(-_maxSpawnPos * 100, _maxSpawnPos * 100) * 0.01f + _spawnPosCenter.x, 0, Random.Range(-_maxSpawnPos * 100, _maxSpawnPos * 100) * 0.01f + _spawnPosCenter.z);
        int toAdd = Random.Range(minRunners, maxRunners);
        float rad = 0.7f * Mathf.Sqrt(toAdd);

        if (!Physics.CheckSphere(pos, rad+4, _runnersLayer))
        {
            GameObject squad;
            if (_usedSquadParent.childCount > 0)
            {
                squad = _usedSquadParent.GetChild(0).gameObject;
                squad.transform.SetParent(_squadParent);
            }
            else
            {
                squad = Instantiate(_squadPrefab, _squadParent);
            }

            squad.transform.localPosition = pos;
            squad.transform.localRotation = Quaternion.Euler(Vector3.zero);

            
            squad.SetActive(true);
            
            squad.GetComponent<AddRuners>().IsHuman = isHuman;
            squad.GetComponent<AddRuners>().AddRunners(toAdd);
            if (isHuman)
            {
                _countHuman++;
            }
            else
            {
                _countAlien++;
            }

            
            squad.GetComponent<MovementNPC>().OnStartMoving();
        }
        else
        {
            SpawnSquad(minRunners, maxRunners, isHuman, deep + 1);
        }
    }

    private void SpawnBonus(int deep=0)
    {
        if (deep > 3)
        {
            return;
        }

        Vector3 pos = new Vector3(Random.Range(-_maxSpawnPos * 100, _maxSpawnPos * 100) * 0.01f + _spawnPosCenter.x, 0, Random.Range(-_maxSpawnPos * 100, _maxSpawnPos * 100) * 0.01f + _spawnPosCenter.z);

        GameObject bonusToSpawn = bonusesList[Random.Range(0, bonusesList.Length)];
        GameObject bonusToSpawnSize = bonusToSpawn.GetComponentInChildren<BonusCollider>().gameObject;
        
        if (!Physics.CheckSphere(pos, bonusToSpawnSize.GetComponent<MeshRenderer>().bounds.size.x, _runnersLayer))
        {
            GameObject bonus = null;
            for(int i=0; i< _usedBonusesParent.childCount; i++)
            {
                if(_usedBonusesParent.transform.GetChild(i).GetComponentInChildren<BonusCollider>().BonusName == bonusToSpawn.GetComponentInChildren<BonusCollider>().BonusName)
                {
                    bonus = _usedBonusesParent.transform.GetChild(i).gameObject;
                    bonus.transform.SetParent(_bonusesParent);
                    break;
                }
            }

            if(bonus==null)
            {
                bonus = Instantiate(bonusToSpawn, _bonusesParent);
            }

            bonus.transform.position = pos;
            bonus.transform.localRotation = Quaternion.Euler(Vector3.zero);
            bonus.SetActive(true);
        }
        else
        {
            SpawnBonus(deep + 1);
        }
    }

    public void KillSquad(AddRuners addRuners, bool isWithAnimation=false)
    {
        if (addRuners.IsHuman)
        {
            _countHuman -= 1;
        }
        else
        {
            _countAlien -= 1;
        }
        
        if (isWithAnimation)
        {
            if (addRuners.GetComponent<MovementNPC>())
            {
                addRuners.GetComponent<MovementNPC>().isStopped = true;
            }
            addRuners.DeadAllRunners();
            StartCoroutine(SetActiveFalseAfterAnimation(addRuners));
        }
        else
        {
            ClearRuners(addRuners.transform, addRuners.IsHuman);
            addRuners.transform.SetParent(_usedSquadParent);
            StartCoroutine(setActiveFalseAfterTime(addRuners.gameObject));
        }
        
    }
    
    private IEnumerator SetActiveFalseAfterAnimation(AddRuners addRuners)
    {
        Animator animator = addRuners.GetComponentInChildren<Runner>().animator;
        yield return null;
        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                yield return new WaitForSeconds(0.3f);
                ClearRuners(addRuners.transform, addRuners.IsHuman);
                addRuners.transform.SetParent(_usedSquadParent);
                yield return null;
                addRuners.gameObject.SetActive(false);
                
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator setActiveFalseAfterTime(GameObject obj)
    {
        yield return null;
        obj.SetActive(false);
    }

    public void RemoveBonus(GameObject bonus)
    {
        bonus.transform.SetParent(_usedBonusesParent);
        bonus.gameObject.SetActive(false);
    }
    

    public GameObject GetHuman()
    {
        if (_usedHumansParent.childCount>0)
        {
            _usedHumansParent.GetChild(0).GetComponent<RunnerData>().DisableRunner();
            return _usedHumansParent.GetChild(0).gameObject;
        }

        return Instantiate(_humanRunnerPrefab);
    }

    public GameObject GetAlien()
    {
        if (_usedAliensParent.childCount > 0)
        {
            _usedAliensParent.GetChild(0).GetComponent<RunnerData>().DisableRunner();
            return _usedAliensParent.GetChild(0).gameObject;
        }

        return Instantiate(_alienRunnerPrefab);
    }

    public void RemoveRunner(GameObject runner, Transform parent)
    {
        runner.GetComponentInChildren<Runner>().Alive();
        runner.GetComponent<RunnerData>().ColliderRunner.enabled = true;
        runner.transform.parent = parent;
        runner.SetActive(false);
    }
}
