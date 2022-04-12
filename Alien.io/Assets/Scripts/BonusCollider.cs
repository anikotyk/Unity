using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonusCollider : MonoBehaviour
{
    [SerializeField] private int _bonusCount;
    [SerializeField] private bool _isHuman;
    [SerializeField] private string _bonusName;

    [SerializeField] private GameObject _toDestroy;

    public GameObject ToDestroy=> _toDestroy;
    public string BonusName => _bonusName;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Runner>(out Runner runner))
        {
            AddRuners addRunners = runner.addRunnersComponent;

            if (addRunners.IsHuman == _isHuman)
            {
                addRunners.AddRunners(_bonusCount);
                if (addRunners.GetComponent<Player>())
                {
                    GameObject.FindObjectOfType<GameController>().ShowCountAddedRunners(transform.position, _bonusCount);
                }
                GameObject.FindObjectOfType<Spawner>().RemoveBonus(_toDestroy);
            }
        }
    }
}
