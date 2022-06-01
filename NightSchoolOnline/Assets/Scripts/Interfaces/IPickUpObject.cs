using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public interface IPickUpObject
{
    event UnityAction PickUpAction;
    event UnityAction ThrowAction;
    event UnityAction DestroyAction;

    string ObjectName { get; }

    void PickUp();
    void Throw();
    void DestroyObject();
}
