using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviour, IPickUpObject
{
    [SerializeField] private Vector3 _pickedRotation;
    [SerializeField] private Vector3 _pickedPosition;
    
    [SerializeField] private List<Vector3> _placesToSpawn;
    [SerializeField] private string _objectName;

    public string ObjectName => _objectName;

    private void Awake()
    {
        if (_placesToSpawn.Count > 0)
        {
            transform.localPosition = _placesToSpawn[Random.Range(0, _placesToSpawn.Count)];
        }
    }
    
    public void PickUp()
    {
        GetComponent<PhysicObject>().PhysicsOff();
        PickUpController.Instance.PickUp(gameObject);
    }

    public void SetPickedSettings()
    {
        transform.localPosition = _pickedPosition;
        transform.localRotation = Quaternion.Euler(_pickedRotation);
    }
}
