using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class UseObjectOnline : MonoBehaviour, IUseObject
{
    private PhotonView _view;
    private int _useFunctionIndex = 0;
    public bool IsCanUse { get; set; } = true;
    [SerializeField] private UseFunctionOnline[] _useFunctions;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
    }

    public void SetUsability(bool canUse)
    {
        IsCanUse = canUse;
    }

    [PunRPC]
    private void SuccesFuncPun()
    {
        _useFunctions[_useFunctionIndex].funcIfSuccess.Invoke();
    }

    [PunRPC]
    private void IncreaseUseFunctionIndexPun()
    {
        _useFunctionIndex++;
    }

    public void IncreaseUseFunctionIndex()
    {
        _view.RPC("IncreaseUseFunctionIndexPun", RpcTarget.Others);
        IncreaseUseFunctionIndexPun();
    }

    public void Use(string takenObjectName = "")
    {
        if (_useFunctions[_useFunctionIndex].nameOfObjectToUse == takenObjectName || _useFunctions[_useFunctionIndex].nameOfObjectToUse == "")
        {
            if (_useFunctions[_useFunctionIndex].isToSendAllPlayersSuccesFunc)
            {
                _view.RPC("SuccesFuncPun", RpcTarget.Others);
            }
            SuccesFuncPun();
        }
        else
        {
            _useFunctions[_useFunctionIndex].funcIfUnsuccess.Invoke();
        }
    }
}

[System.Serializable]
public class UseFunctionOnline : UseFunction
{
    public bool isToSendAllPlayersSuccesFunc;
}
