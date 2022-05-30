using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUseObject
{
    bool IsCanUse { get; set; }

    void SetUsability(bool canUse);
    void IncreaseUseFunctionIndex();
    void Use(string takenObjectName = "");
}
