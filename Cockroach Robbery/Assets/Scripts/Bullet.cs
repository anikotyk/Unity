using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform _target;
    [SerializeField] private float speed;

    

    private void Update()
    {
        if (_target == null) { return; }

        transform.position = Vector3.MoveTowards(transform.position, _target.position, speed*Time.deltaTime);
    }

    public void SetTargetAndStartMove(Transform target)
    {
        _target = target;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.TryGetComponent<CocroachGrowth>(out CocroachGrowth cocroach))
        {
            if (cocroach.transform == _target)
            {
                cocroach.Die();
                Destroy(this.gameObject);
            }
        }
    }
}
