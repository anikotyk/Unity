using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicObject : MonoBehaviour
{
    protected Rigidbody _rb;
    protected Collider _collider;

    protected virtual void Awake() {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
    }

    public virtual void PhysicsOff()
    {
        _collider.enabled = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    public virtual void PhysicsOn()
    {
        _collider.enabled = true;
        _rb.constraints = RigidbodyConstraints.None;
    }

    public virtual void DestroyObject()
    {
        Destroy(gameObject);
    }
}
