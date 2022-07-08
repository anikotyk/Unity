using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BattleController : MonoBehaviour
{
    [SerializeField] private float _delayBeforePushRagdoll;
    
    private List<PokemonBattle> _pokemons;

    private bool _isBattleEnded = false;
    public bool IsGameEnded => _isBattleEnded;

    private List<GameObject> bullets = new List<GameObject>();
    private Coroutine _collectPower;

    [SerializeField] private float _delayBeforeBattle = 1f;
    [SerializeField] private float _delayBeforeEnemiesStartShoot = 0.75f;

    [SerializeField] private float _timeBigBullets = 1f;
    
    [SerializeField] private HapticPatterns.PresetType _vibrationBulletSend;
    [SerializeField] private HapticPatterns.PresetType _vibrationBulletOnTarget;

    private LevelData _levelData;

    public event UnityAction HeroWinAction;

    public static BattleController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }


    public void ResetController()
    {
        bullets.Clear();
        _levelData = LevelController.Instance.LevelData;
        _isBattleEnded = false;
    }

    private void OnEnable()
    {
        GameObject.FindObjectOfType<GameController>().BattleStart += StartBattle;
        GameObject.FindObjectOfType<LevelController>().LevelLoaded += ResetController;
    }

    private void Start()
    {
        ResetController();
    }

    public void SetPokemons(List<PokemonBattle> pokemons)
    {
        _pokemons = pokemons;
    }
    
    private void StartBattle()
    {
        StartCoroutine(StartShootingCoroutine());
    }

    private IEnumerator StartShootingCoroutine()
    {
        yield return new WaitForSeconds(_delayBeforeBattle);

        if (_pokemons.Count <= 0)
        {
            EnemiesWin();
            yield break;
        }

        for (int i = 0; i < _pokemons.Count; i++)
        {
            _pokemons[i].SetShootingTarget(GetPokemonByIndex(i, _levelData.EnemyPokemons));
        }

        yield return new WaitForSeconds(_delayBeforeEnemiesStartShoot);

        for (int i = 0; i < _levelData.EnemyPokemons.Count; i++)
        {
            _levelData.EnemyPokemons[i].SetShootingTarget(GetPokemonByIndex(i, _pokemons));
        }
    }

    private PokemonBattle GetPokemonByIndex(int index, List<PokemonBattle> pokemonsList)
    {
        return pokemonsList[Mathf.Min(pokemonsList.Count - 1, index)];
    }

    public PokemonBattle GetAveliableTarget(bool isTargetEnemy)
    {
        List<PokemonBattle> pokemonsList;
        if(isTargetEnemy)
        {
            pokemonsList = _levelData.EnemyPokemons;
        }
        else
        {
            pokemonsList = _pokemons;
        }

        
        foreach(PokemonBattle pokemon in pokemonsList)
        {
            if (pokemon.Health.CheckIsAlive())
            {
                return pokemon;
            }
        }

        return null;
    }

    public void CheckWinner()
    {
        if (_isBattleEnded) { return; }

        bool isEnemiesAlive = false;
        bool isPokemonsAlive = false;

        foreach (PokemonBattle pokemon in _levelData.EnemyPokemons)
        {
            if (pokemon.Health.CheckIsAliveCurrent())
            {
                isEnemiesAlive = true;
            }
        }

        foreach (PokemonBattle pokemon in _pokemons)
        {
            if (pokemon.Health.CheckIsAliveCurrent())
            {
                isPokemonsAlive = true;
            }
        }

        if(!isPokemonsAlive && isEnemiesAlive)
        {
            EnemiesWin();
        }
        else if (!isEnemiesAlive)
        {
            HeroWin();
        }
    }

    public void EnemiesWin()
    {
        _isBattleEnded = true;
        StartCoroutine(SendEnemyBullets());
    }

    private IEnumerator SendEnemyBullets()
    {
        foreach (PokemonBattle pokemon in _levelData.EnemyPokemons)
        {
            HapticPatterns.PlayPreset(_vibrationBulletSend);
            GameObject bullet = pokemon.SpawnBullet();
            LeanTween.move(bullet, _levelData.HeroPoint.transform.position, _timeBigBullets).setOnComplete(() => { bullet.SetActive(false); });
        }
        yield return new WaitForSeconds(_timeBigBullets);
        GameObject.FindObjectOfType<Runner>().Die();
        GameController.Instance.LoseLevel();
    }

    private void HeroWin()
    {
        _isBattleEnded = true;

        HeroWinAction?.Invoke();
        GameObject.FindObjectOfType<EndLevelWallsController>().ShowWalls();

        StartCollectPower();
    }

    private void StartCollectPower()
    {
        _collectPower = StartCoroutine(CollectPowerCoroutine());
    }

    private IEnumerator CollectPowerCoroutine()
    {
        foreach (PokemonBattle pokemon in _pokemons)
        {
            if (pokemon.Health.CheckIsAlive())
            {
                bullets.Add(pokemon.SpawnBigBullet());
            }
        }

        while (true)
        {
            SoundsController.Instance.GetPower();
            foreach (GameObject bullet in bullets)
            {
                float value = 1 + GameObject.FindObjectOfType<PowerBar>().BarValue * 1f / 100f;
                LeanTween.scale(bullet, new Vector3(value, value, value), 0.1f);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    
    public void SendBulletsWithForce(float force)
    {
        
        StopCoroutine(_collectPower);
        foreach (GameObject bullet in bullets)
        {
            HapticPatterns.PlayPreset(_vibrationBulletSend);
            LeanTween.move(bullet, _levelData.EnemyRigidbody.transform.position, _timeBigBullets).setOnComplete(()=> { bullet.SetActive(false); });
        }
        
        StartCoroutine(ForceEnemyCoroutine(force));
    }

    private IEnumerator ForceEnemyCoroutine(float force)
    {
        yield return new WaitForSeconds(_timeBigBullets);

        GameObject.FindObjectOfType<CinemachineController>().SetCameraBattleForwardEnd();

        yield return new WaitForSeconds(_delayBeforePushRagdoll);

        HapticPatterns.PlayPreset(_vibrationBulletOnTarget);
        _levelData.RagdollPhysics.TurnOnRagdoll();
        
        ArrowsController.Instance.TurnOnArrowsColliders();
        int wallEndIndex = (int)Mathf.Ceil(_levelData.MaxWallsBrokenIndex * force * 1f / 100f);

        GameObject.FindObjectOfType<RagdollMovement>().enabled = true;
        GameObject.FindObjectOfType<RagdollMovement>().StartMove(_levelData.Walls[wallEndIndex]);
    }
}