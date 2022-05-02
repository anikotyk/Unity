using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Vector3 _bulletStartPosition;
    [SerializeField] private Transform _bulletParent;

    public static Gun Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    public void Shoot(Transform target)
    {
        GameObject bullet = Instantiate(_bulletPrefab);
        bullet.transform.SetParent(_bulletParent);
        bullet.transform.localPosition = _bulletStartPosition;
        bullet.GetComponent<Bullet>().SetTargetAndStartMove(target);
    }

    
}
