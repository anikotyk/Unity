using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObject : MonoBehaviourPun, IPunOwnershipCallbacks
{
    [SerializeField] private Quaternion _pickedRotation;
    public Quaternion PickedRotation=> _pickedRotation;

    private PhotonView _view;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDestroy()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnOwnershipRequest(PhotonView targetView, Player requestingPlayer)
    {
        if (targetView != base.photonView)
        {
            return;
        }
        
        if (_view.IsMine && GameObject.FindObjectOfType<PickUpController>().PickedUpObject != this.gameObject)
        {
            base.photonView.TransferOwnership(requestingPlayer);
        }
    }

    public void OnOwnershipTransfered(PhotonView targetView, Player previousOwner)
    {
        if (targetView != base.photonView)
        {
            return;
        }

        if (_view.IsMine)
        {
            GetComponent<PhysicsEvents>().PhysicsOffForAllPlayers();
            GameObject.FindObjectOfType<PickUpController>().PickUp(gameObject);
        }
        
    }

    public void OnOwnershipTransferFailed(PhotonView targetView, Player senderOfFailedRequest)
    {
        if (targetView != base.photonView)
        {
            return;
        }

        Debug.Log("Can't transfer ownership");
    }

    public void PickUp()
    {
        base.photonView.RequestOwnership();
    }
    
    
}
