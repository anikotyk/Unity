using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfBallData : MonoBehaviour
{
    private List<Vector3> _partsPositions = new List<Vector3>();
    private List<Quaternion> _partsRotations = new List<Quaternion>();
    
    public void SaveAwakePosRotOfBallParts()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _partsPositions.Add(transform.GetChild(i).localPosition);
            _partsRotations.Add(transform.GetChild(i).localRotation);
        }
    }

    public void ReturnBallPartsToAwakePosRot()
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
            transform.GetChild(i).localPosition = _partsPositions[i];
            transform.GetChild(i).localRotation = _partsRotations[i];
        }
    }

    public void TurnOnDiePhysicsOfBallParts()
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
