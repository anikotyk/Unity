using UnityEngine;

public class RagdollPhysics : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider _mainCollider;
    [SerializeField] private Rigidbody _mainRigidbody;

    [SerializeField] private GameObject _rig;

    private Collider[] _ragdollColliders;
    private Rigidbody[] _ragdollRigidbodies;

    private void Awake()
    {
        _ragdollColliders = _rig.GetComponentsInChildren<Collider>();
        _ragdollRigidbodies = _rig.GetComponentsInChildren<Rigidbody>();
        RagdollOff();
    }

    public void TurnOnRagdoll()
    {
        RagdollOn();
    }
    
    private void RagdollOn()
    {
        _animator.enabled = false;

        foreach(Collider collider in _ragdollColliders)
        {
            collider.enabled = true;
        }

        foreach (Rigidbody rigidbody in _ragdollRigidbodies)
        {
            rigidbody.isKinematic = false;
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }

        _mainCollider.enabled = false;
        _mainRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        _mainRigidbody.isKinematic = true;
    }

    public void RagdollOff()
    {
        foreach (Collider collider in _ragdollColliders)
        {
            collider.enabled = false;
        }

        foreach (Rigidbody rigidbody in _ragdollRigidbodies)
        {
            rigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rigidbody.isKinematic = true;
        }

        _animator.enabled = true;
        _mainCollider.enabled = true;
        _mainRigidbody.isKinematic = false;
        _mainRigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
}
