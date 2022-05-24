using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PhysicsEvents : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _collider;
    private PhotonView _view;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _view = GetComponent<PhotonView>();
    }

    public void PhysicsOffForAllPlayers()
    {
        _view.RPC("PhysicsOff", RpcTarget.Others);
        if (_view.IsMine)
        {
            PhysicsOff();
        }
    }

    public void PhysicsOnForAllPlayers()
    {
        _view.RPC("PhysicsOn", RpcTarget.Others);
        if (_view.IsMine)
        {
            PhysicsOn();
        }
    }
    
    [PunRPC]
    private void PhysicsOff()
    {
        _collider.enabled = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    [PunRPC]
    private void PhysicsOn()
    {
        _collider.enabled = true;
        _rb.constraints = RigidbodyConstraints.None;
    }
}
