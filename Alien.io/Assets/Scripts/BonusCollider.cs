using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCollider : MonoBehaviour
{
    [SerializeField] private int bonusCount;
    [SerializeField] private bool isHuman;
    [SerializeField] private string bonusName;

    [SerializeField] private GameObject toDestroy;

    public GameObject ToDestroy=> toDestroy;
    public bool IsHuman=> isHuman;
    public string BonusName => bonusName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent.parent.TryGetComponent<AddRuners>(out AddRuners addRunners))
        {
            if (addRunners.IsHuman == isHuman)
            {
                addRunners.AddRunners(bonusCount);
                GameObject.FindObjectOfType<Spawner>().RemoveBonus(toDestroy);
               // Destroy(toDestroy);
            }
        }
    }
}
