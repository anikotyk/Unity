using Lofelt.NiceVibrations;
using UnityEngine;

public abstract class Bullet : MonoBehaviour
{
    protected Transform _target;

    [SerializeField] protected float _speed = 1f;
    [SerializeField] protected int _bulletDamage = 35;
    public int BulletDamage => _bulletDamage;
    [SerializeField] protected AudioSource _soundOnShoot;
    [SerializeField] protected GameObject _visibleObject;



    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.transform == _target)
        {
            OnReachedTarget();
        }
    }

    protected virtual void OnReachedTarget()
    {
        //Destroy(gameObject);
        _visibleObject.SetActive(false);
    }

    public virtual void MoveToTarget(Transform target)
    {
        _target = target;
        if (SoundsController.Instance.IsSoundTurned())
        {
            _soundOnShoot?.Play();
        }
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.RigidImpact);

        float timeOfMovement = Vector3.Distance(target.position, transform.position) / _speed;
        LeanTween.move(gameObject, target, timeOfMovement);
    }
}
