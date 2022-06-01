using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PickUpObjectOnline : MonoBehaviourPun, IPunOwnershipCallbacks, IPickUpObject
{
    [SerializeField] private Vector3 _pickedRotation;
    [SerializeField] private Vector3 _pickedPosition;

    [SerializeField] private List<Vector3> _placesToSpawn;
    [SerializeField] private string _objectName;

    public string ObjectName => _objectName;
    public event UnityAction PickUpAction;
    public event UnityAction ThrowAction;
    public event UnityAction DestroyAction;

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
            PickUp();
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
        if (_view.IsMine)
        {
            PickUpAction?.Invoke();
            PickUpController.Instance.PickUp(gameObject);
            SetPickedSettings();
        }
        else
        {
            base.photonView.RequestOwnership();
        }
    }

    public void Throw()
    {
        ThrowAction?.Invoke();
    }

    public void DestroyObject()
    {
        DestroyAction?.Invoke();
    }

    private void SetPickedSettings()
    {
        transform.localPosition = _pickedPosition;
        transform.localRotation = Quaternion.Euler(_pickedRotation);
    }
}
