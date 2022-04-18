using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PartOfBallData : MonoBehaviour
{
    private List<Vector3> partsPositions = new List<Vector3>();
    private List<Quaternion> partsRotations = new List<Quaternion>();

    private void Awake()
    {
        for(int i=0; i<transform.childCount; i++)
        {
            partsPositions.Add(transform.GetChild(i).localPosition);
            partsRotations.Add(transform.GetChild(i).localRotation);
        }
    }

    public void SetAwakeState()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if(transform.GetChild(i).TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.useGravity = false;
                rigidbody.isKinematic = true;
            }
            transform.GetChild(i).localPosition = partsPositions[i];
            transform.GetChild(i).localRotation = partsRotations[i];
        }
    }
}
