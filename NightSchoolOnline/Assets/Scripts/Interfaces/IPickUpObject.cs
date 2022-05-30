using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IPickUpObject
{
    string ObjectName { get; }

    void PickUp();

    void SetPickedSettings();
}
