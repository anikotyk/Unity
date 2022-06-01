using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PhysicObject : MonoBehaviour
{
    protected Rigidbody _rb;
    protected Collider _collider;
    protected IPickUpObject _pickUpObject;

    protected virtual void Awake() {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _pickUpObject = GetComponent<IPickUpObject>();
    }
    
    protected virtual void OnEnable()
    {
        _pickUpObject.PickUpAction += PhysicsOff;
        _pickUpObject.ThrowAction += PhysicsOn;
        _pickUpObject.DestroyAction += DestroyObject;
    }

    protected virtual void OnDisable()
    {
        _pickUpObject.PickUpAction -= PhysicsOff;
        _pickUpObject.ThrowAction -= PhysicsOn;
        _pickUpObject.DestroyAction -= DestroyObject;
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
