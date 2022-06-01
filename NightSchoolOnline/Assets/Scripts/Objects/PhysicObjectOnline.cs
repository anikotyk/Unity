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
        base.PhysicsOff();
    }

    public override void PhysicsOn()
    {
        _view.RPC("PhysicsOnRPC", RpcTarget.Others);
        base.PhysicsOn();
    }

    public override void DestroyObject()
    {
        PhotonNetwork.Destroy(_view);
    }

    [PunRPC]
    private void PhysicsOnRPC()
    {
        base.PhysicsOn();
    }

    [PunRPC]
    private void PhysicsOffRPC()
    {
        base.PhysicsOff();
    }
    
}
