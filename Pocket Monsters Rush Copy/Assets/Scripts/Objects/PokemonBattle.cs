using System.Collections;
using UnityEngine;

public class PokemonBattle : MonoBehaviour
{
    [SerializeField] private bool _isEnemy;

    [SerializeField] private BattlePokemonBullet _bulletPrefab;

    [SerializeField] private Transform _shootPoint;
    [SerializeField] private float _shootDelay;

    [SerializeField] private Animator _animator;

    [SerializeField] private GameObject _particleDamage;
    [SerializeField] private GameObject _particleDead;

    [SerializeField] private GameObject _bigBulletPrefab;
    [SerializeField] private Transform _bigBulletPosition;

    [SerializeField] private GameObject _visibleObject;

    [SerializeField] private BattlePokemonHealth _health;
    public BattlePokemonHealth Health => _health;

    private PokemonBattle _target;
    private Coroutine _shootingCoroutine;
    private bool isDead=false;

    private void Awake()
    {
        _health.HealthBar.Deactivate();
    }

    public void SetShootingTarget(PokemonBattle target)
    {
        if (target != null)
        {
            _target = target;
            _health.HealthBar.Activate();
            _shootingCoroutine = StartCoroutine(ShootingCoroutine());
        }
    }

    private IEnumerator ShootingCoroutine()
    {
        while (true)
        {
            if (_target.Health.CheckIsAlive())
            {
                Shoot(_target);

                yield return new WaitForSeconds(_shootDelay);
            }
            else
            {
                SetShootingTarget(BattleController.Instance.GetAveliableTarget(!_isEnemy));

                _health.HealthBar.Deactivate();

                yield break;
            }
        }
    }
    
    private void Shoot(PokemonBattle target)
    {
        BattlePokemonBullet bullet = Instantiate(_bulletPrefab);
        bullet.transform.SetParent(LevelController.Instance.LevelData.BulletsParent);
        bullet.transform.position = _shootPoint.position;
        bullet.MoveToTarget(target.transform);

        target.Health.GetDamage(bullet.BulletDamage);

        _animator.Play("Atack");
    }

    public void GetDamage(int damageAmount)
    {
        if (isDead) { return; }

        _health.GetCurrentDamage(damageAmount);

        _particleDamage.SetActive(true);
        
        if (!_health.CheckIsAliveCurrent())
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        _particleDead.SetActive(true);
        _health.HealthBar.Deactivate();
        _animator.enabled = false;
        _visibleObject.SetActive(false);

        StopCoroutine(_shootingCoroutine);

        BattleController.Instance.CheckWinner();
    }

    public GameObject SpawnBigBullet()
    {
        GameObject bullet = Instantiate(_bigBulletPrefab);
        bullet.transform.SetParent(LevelController.Instance.LevelData.BulletsParent);
        bullet.transform.position = _bigBulletPosition.position;

        return bullet;
    }

    public GameObject SpawnBullet()
    {
        BattlePokemonBullet bullet = Instantiate(_bulletPrefab);
        bullet.transform.SetParent(LevelController.Instance.LevelData.BulletsParent);
        bullet.transform.position = _shootPoint.position;

        return bullet.gameObject;
    }
}
