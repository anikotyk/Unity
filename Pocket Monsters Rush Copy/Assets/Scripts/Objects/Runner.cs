using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class Runner : MonoBehaviour
{
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _shootingDelay;
    [SerializeField] private PokeballEggBullet _pockeballPrefab;

    [SerializeField] private Transform[] _pockemonPoints;

    [SerializeField] private Transform _pockemonsParent;

    [SerializeField] private FollowPath _followPath;
    [SerializeField] private RunnerMovement _movement;

    [SerializeField] private float _timeSetPositionHeroBattle=1f;
    [SerializeField] private float _timeSetPositionPokemonsBattle = 1.5f;

    [SerializeField] private float _delayBeforePurposeStartBattle = 2f;

    [SerializeField] private GameObject _particleAddPokemon;


    private List<PokemonBattle> _pokemons = new List<PokemonBattle>();

    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _dieParticle;
    [SerializeField] private GameObject _visibleObject;
    

    private Coroutine _shootingCoroutine;

    public event UnityAction PreBattleStarted;
    
    private void OnEnable()
    {
        GameObject.FindObjectOfType<GameController>().GameStart += OnGameStart;
        GameObject.FindObjectOfType<LevelController>().LevelLoaded += ResetRunner;
    }
    
    private void OnGameStart()
    {
        _movement.enabled = true;
        _animator.SetBool("IsRunning", true);
    }

    public void OnFinishReached()
    {
        StartPreBattle();
    }

    public void Die()
    {
        _followPath.StopMove();
        _movement.enabled = false;
        _dieParticle.SetActive(true);
        _visibleObject.SetActive(false);
    }
    
    public void StartPreBattle()
    {
        PreBattleStarted?.Invoke();
        
        _movement.enabled = false;
        foreach (Transform pokemon in _pockemonPoints)
        {
            pokemon.GetComponent<PokemonMovement>().enabled = false;
        }

        BattleController.Instance.SetPokemons(_pokemons);

        StartCoroutine(MoveOnBattlePositions());
    }

    private IEnumerator MoveOnBattlePositions()
    {
        Vector3 targetPos = new Vector3(LevelController.Instance.LevelData.HeroPoint.position.x, transform.position.y, LevelController.Instance.LevelData.HeroPoint.position.z);
        LeanTween.move(gameObject, targetPos, _timeSetPositionHeroBattle);

        for (int i = 0; i < _pokemons.Count; i++)
        {
            targetPos = new Vector3(LevelController.Instance.LevelData.PokemonPoints[i].position.x, _pokemons[i].transform.position.y, LevelController.Instance.LevelData.PokemonPoints[i].position.z);
            LeanTween.move(_pokemons[i].gameObject, targetPos, _timeSetPositionPokemonsBattle);
        }

        yield return new WaitForSeconds(_timeSetPositionHeroBattle);
        SoundsController.Instance.TurnOffFootsteps();
        _animator.SetBool("IsRunning", false);
        yield return new WaitForSeconds(_timeSetPositionPokemonsBattle - _timeSetPositionHeroBattle);

        foreach (Transform pokemon in _pockemonPoints)
        {
            Pokemon pokemonControll = pokemon.GetComponentInChildren<Pokemon>();
            if (pokemonControll)
            {
                pokemonControll.StartBattle();
            }
        }

        GameObject.FindObjectOfType<CinemachineController>().SetCameraBattleHero();
        yield return new WaitForSeconds(_delayBeforePurposeStartBattle);
        if (_pokemons.Count <= 0)
        {
            BattleController.Instance.EnemiesWin();
        }
        else
        {
            GameObject.FindObjectOfType<GameController>().ShowStartBattleButton();
        }
        
    }

    public void StartShooting(Pokemon shootingTarget)
    {
        _shootingCoroutine = StartCoroutine(ShootingCoroutine(shootingTarget));
    }
    
    private IEnumerator ShootingCoroutine(Pokemon shootingTarget)
    {
        while (true)
        {
            Shoot(shootingTarget);
            yield return new WaitForSeconds(_shootingDelay);
        }
    }

    public void EndShooting()
    {
        StopCoroutine(_shootingCoroutine);
    }

    private void Shoot(Pokemon shootingTarget)
    {
        if (PockeballEggsController.Instance.GetCount() > 0 && shootingTarget.Health.CheckIsAlive())
        {
            PockeballEggsController.Instance.DecreaseCount(1);

            PokeballEggBullet pockeball = Instantiate(_pockeballPrefab);
            pockeball.transform.SetParent(LevelController.Instance.LevelData.BulletsParent);
            
            pockeball.transform.position = _shootPoint.position;
            pockeball.MoveToTarget(shootingTarget.transform);

            shootingTarget.Health.GetDamage(pockeball.BulletDamage);
        }
    }

    public void AddPockemon(Pokemon pokemon)
    {
        _pokemons.Add(pokemon.GetComponent<PokemonBattle>());

        pokemon.SetOnPlace(_pockemonPoints[_pokemons.Count-1]);
        _animator.Play("Jumping");
        _particleAddPokemon.SetActive(true);
    }

    private void ResetRunner()
    {
        _visibleObject.SetActive(true);
        transform.localPosition = new Vector3(0, transform.localPosition.y, 0);
        transform.localRotation = Quaternion.Euler(Vector3.zero);

        foreach(Transform pokemon in _pockemonPoints)
        {
            pokemon.GetComponent<PokemonMovement>().enabled = true;
            if (pokemon.childCount > 0)
            {
                Destroy(pokemon.GetChild(0).gameObject);
            }
        }
        _pokemons.Clear();


        _animator.SetBool("IsRunning", false);
        _movement.enabled = false;
    }
}
