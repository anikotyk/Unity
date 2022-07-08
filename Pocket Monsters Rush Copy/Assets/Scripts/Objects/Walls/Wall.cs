using Lofelt.NiceVibrations;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Wall : MonoBehaviour
{
    [SerializeField] private string _textCoef = "X 2.0";
    [SerializeField] private GameObject _textObj;
    [SerializeField] private ParticleSystem[] _particles;
    [SerializeField] private Transform _wallPositionPoint;
    [SerializeField] private AudioSource _soundPunch;
    [SerializeField] private HapticPatterns.PresetType _vibrationPunch = HapticPatterns.PresetType.MediumImpact;
    public Transform WallPositionPoint => _wallPositionPoint;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainEnemy")
        {
            WallsCoeffAnim.Instance.SetCoef(_textCoef);
            if (SoundsController.Instance.IsSoundTurned())
            {
                _soundPunch.Play();
            }
            HapticPatterns.PlayPreset(_vibrationPunch);
            GetComponent<Collider>().enabled = false;
            _textObj.SetActive(false);
        }
    }

    public void PlayParticles()
    {
        foreach(ParticleSystem particle in _particles)
        {
            particle.gameObject.SetActive(true);
        }
    }

    public void HideCoefText()
    {
        _textObj.SetActive(false);
    }
}
