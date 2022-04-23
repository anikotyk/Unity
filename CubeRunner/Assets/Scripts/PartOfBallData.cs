using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfBallData : MonoBehaviour
{
    private List<Vector3> partsPositions = new List<Vector3>();
    private List<Quaternion> partsRotations = new List<Quaternion>();
    
    public void GetAwakeState()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            partsPositions.Add(transform.GetChild(i).localPosition);
            partsRotations.Add(transform.GetChild(i).localRotation);
        }
    }

    public void SetAwakeState()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
            }
            if (transform.GetChild(i).TryGetComponent<MeshCollider>(out MeshCollider meshCollider))
            {
                meshCollider.enabled = false;
            }
            transform.GetChild(i).localPosition = partsPositions[i];
            transform.GetChild(i).localRotation = partsRotations[i];
        }
    }

    public void SetDieState()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).TryGetComponent<MeshCollider>(out MeshCollider meshCollider))
            {
                meshCollider.enabled = true;
            }
            if (transform.GetChild(i).TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
            }
        }
    }
}
