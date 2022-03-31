using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCollider : MonoBehaviour
{
    [SerializeField] private int bonusCount;
    [SerializeField] private bool isHuman;

    [SerializeField] private GameObject toDestroy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.parent.TryGetComponent<AddRuners>(out AddRuners addRunners))
        {
            if (addRunners.IsHuman == isHuman)
            {
                addRunners.AddRunners(bonusCount);
                Destroy(toDestroy);
            }
        }
    }
}
