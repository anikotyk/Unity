using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Unit : MonoBehaviour
{
    //[SerializeField] Leader leader = null;
    [SerializeField] float sqrMaxForce = 55, sqrMaxVelocity = 12;

    private Vector3 currentForce = Vector3.zero;
    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void Flock(Vector3 _goalPos)
    {
        currentForce = Seek(_goalPos).normalized;
        Move(currentForce * 50);
    }

    // Used to find the vector towards the leader
    Vector3 Seek(Vector3 _target)
    {
        return _target - transform.position;
    }

    void Move(Vector3 _force)
    {
        Vector3 force = new Vector3(_force.x, 0, _force.z);

        if (force.sqrMagnitude > sqrMaxForce)
        {
            force = force.normalized;
            force *= sqrMaxForce;
        }
        rb.AddForce(force);

        if (rb.velocity.sqrMagnitude > sqrMaxVelocity)
        {
            rb.velocity = rb.velocity.normalized;
            rb.velocity *= sqrMaxVelocity;
        }

        // Used to align the units with their leader
        //transform.forward = Vector3.Lerp(transform.forward, leader.transform.forward, 5 * Time.deltaTime);
        //transform.forward = Vector3.Lerp(transform.forward, transform.parent.forward, 5 * Time.deltaTime);
    }
}
