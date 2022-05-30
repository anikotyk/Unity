using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpObjectOnline : MonoBehaviourPun, IPunOwnershipCallbacks, IPickUpObject
{
    [SerializeField] private Vector3 _pickedRotation;
    [SerializeField] private Vector3 _pickedPosition;

    [SerializeField] private List<Vector3> _placesToSpawn;
    [SerializeField] private string _objectName;

    public string ObjectName => _objectName;

    private PhotonView _view;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        PhotonNetwork.AddCallbackTarget(this);

        if (_view.IsMine && _placesToSpawn.Count > 0)
        {
            transform.localPosition = _placesToSpawn[Random.Range(0, _placesToSpawn.Count)];
        }
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

        if (_view.IsMine && PickUpController.Instance.PickedUpObject != this.gameObject)
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
            GetComponent<PhysicObject>().PhysicsOff();
            PickUpController.Instance.PickUp(gameObject);
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

    public void SetPickedSettings()
    {
        transform.localPosition = _pickedPosition;
        transform.localRotation = Quaternion.Euler(_pickedRotation);
    }
}
