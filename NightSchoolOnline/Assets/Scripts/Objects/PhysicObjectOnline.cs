using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicObjectOnline : PhysicObject
{
    private PhotonView _view;

    protected override void Awake()
    {
        base.Awake();

        _view = GetComponent<PhotonView>();
    }

    public override void PhysicsOff()
    {
        _view.RPC("PhysicsOffRPC", RpcTarget.Others);
        PhysicsOffRPC();
    }

    public override void PhysicsOn()
    {
        _view.RPC("PhysicsOnRPC", RpcTarget.Others);
        PhysicsOnRPC();
    }

    public override void DestroyObject()
    {
        PhotonNetwork.Destroy(_view);
    }

    [PunRPC]
    private void PhysicsOnRPC()
    {
        _collider.enabled = true;
        _rb.constraints = RigidbodyConstraints.None;
    }

    [PunRPC]
    private void PhysicsOffRPC()
    {
        _collider.enabled = false;
        _rb.constraints = RigidbodyConstraints.FreezeAll;
    }
    
}
