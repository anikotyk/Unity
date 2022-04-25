using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CocroachMovement : MonoBehaviour
{
    private Rigidbody rb;

    [SerializeField] private int speed = 10;
    private Vector3 direction;
    private float angleMin;
    private float angleMax;
    
    private float angle;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ChangeDirection();
    }

    private void Update()
    {
        rb.velocity = transform.forward * speed;
    }
    private void ChangeDirection()
    {
        angleMin = 0;
        angleMax = 360;

        angle = Random.Range(angleMin, angleMax);
        Debug.Log(angleMin + " " + angleMax + " " + angle);

        transform.localRotation = Quaternion.Euler(0, angle, 0);
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
        Debug.Log(normal + " " + Vector3.forward);
        Debug.Log(angle);
        angle = Random.Range(angleMin, angleMax);

        Debug.Log(angleMin+" "+ angleMax+" "+angle);
        
        transform.localRotation = Quaternion.Euler(0, angle, 0);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag != "Floor")
        {
            var point = collision.contacts[0].point;

            var dir = collision.contacts[0].normal;
            ChangeDirection(collision.contacts[0].normal);
            point -= dir;
            RaycastHit hitInfo;
            if (collision.collider.Raycast(new Ray(point, dir), out hitInfo, 2))
            {
                var normal = hitInfo.normal;
                
                
            }
            
        }
    }
    
}
