using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CocroachMovement : MonoBehaviour
{
    private Rigidbody rb;
    
    private float angleMin;
    private float angleMax;
    
    private float angle;

    private int speed;

    private float timer;
    private float timeToChangeDirection;

    private bool isMoving;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        //ChangeDirection();
        isMoving = true;
    }

    public void EndMovement()
    {
        isMoving = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public void StartMovement()
    {
        rb.isKinematic = false;
        rb.useGravity = true;
        isMoving = true;
    }

    private void Update()
    {
        if (!isMoving) { return; }
        rb.velocity = transform.forward * speed;
        timer += Time.deltaTime;
        if(timer >= timeToChangeDirection)
        {
            ChangeDirection();
        }
    }

    private void ResetTimer()
    {
        timer = 0;
        timeToChangeDirection = Random.Range(1f, 4f);
    }

    private void ChangeSpeedRandomly()
    {
        speed = Random.Range(7, 12);
    }

    private void ChangeDirection()
    {
        angleMin = 0;
        angleMax = 360;

        angle = Random.Range(angleMin, angleMax);

        transform.localRotation = Quaternion.Euler(0, angle, 0);

        ResetTimer();
        ChangeSpeedRandomly();
    }
    
    public void SetDirection(Vector3 normal)
    {
        ChangeDirection(normal);
    }

    public void SetDirection()
    {
        ChangeDirection();
    }

    private void ChangeDirection(Vector3 normal)
    {
        angle = Vector3.Angle(normal, Vector3.forward);
        if (normal.x != 0)
        {
            angle *= Mathf.Sign(normal.x);
        }
        angleMin = angle-80;
        angleMax = angle+80;
        angle = Random.Range(angleMin, angleMax);
        
        transform.localRotation = Quaternion.Euler(0, angle, 0);

        ResetTimer();
        ChangeSpeedRandomly();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Floor")
        {
            ChangeDirection(collision.contacts[0].normal);
        }
    }
    
}
