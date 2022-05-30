using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class UseObject : MonoBehaviour, IUseObject
{
    private int _useFunctionIndex = 0;
    public bool IsCanUse { get; set; } = true;
    [SerializeField] private UseFunction[] _useFunctions;
    
    public void SetUsability(bool canUse)
    {
        IsCanUse = canUse;
    }

    public void Use(string takenObjectName="")
    {
        if (_useFunctions[_useFunctionIndex].nameOfObjectToUse == takenObjectName || _useFunctions[_useFunctionIndex].nameOfObjectToUse=="")
        {
            _useFunctions[_useFunctionIndex].funcIfSuccess.Invoke();
        }
        else
        {
            _useFunctions[_useFunctionIndex].funcIfUnsuccess.Invoke();
        }
    }
    
    public void IncreaseUseFunctionIndex()
    {
        _useFunctionIndex++;
    }
}

[System.Serializable]
public class UseFunction
{
    public string nameOfObjectToUse;
    public UnityEvent funcIfSuccess;
    public UnityEvent funcIfUnsuccess;
}
