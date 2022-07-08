using Lofelt.NiceVibrations;
using UnityEngine;

public abstract class PickUpObject : MonoBehaviour
{
    [SerializeField] protected GameObject _visibleObject;
    
    [SerializeField] protected GameObject[] _particlesOnPickUp;
    [SerializeField] protected AudioSource _soundOnPickUp;

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<Collector>())
        {
            ActionOnPickUp();
            ShowPickUpEffects();
            MakeObjectInvisible();
        }
    }

    protected virtual void ActionOnPickUp()
    {
        
    }

    protected virtual void ShowPickUpEffects()
    {
        foreach (GameObject particle in _particlesOnPickUp)
        {
            particle?.gameObject.SetActive(true);
        }

        if (SoundsController.Instance.IsSoundTurned())
        {
            _soundOnPickUp?.Play();
        }

        HapticPatterns.PlayPreset(HapticPatterns.PresetType.LightImpact);
    }

    protected virtual void MakeObjectInvisible()
    {
        _visibleObject?.SetActive(false);
    }
}
