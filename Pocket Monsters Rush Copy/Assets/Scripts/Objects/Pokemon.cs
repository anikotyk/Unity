using Lofelt.NiceVibrations;
using System.Collections;
using UnityEngine;

public class Pokemon : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    [SerializeField] private float _timeMoveJoin = 1f;

    [SerializeField] private GateHealth _health;
    public GateHealth Health => _health;

    [SerializeField] private GameObject[] _particles;

    [SerializeField] private AudioSource _soundOnJoin;
    [SerializeField] private HapticPatterns.PresetType _vibrationsOnJoin;

    private bool _isJoined = false;

   

    public void GetDamage(int damage)
    {
        _health.GetCurrentDamage(damage);
        if (!_health.CheckIsAliveCurrent())
        {
            JoinRunner();
        }

        PlayParticles();
    }

    private void PlayParticles()
    {
        foreach (GameObject obj in _particles)
        {
            obj.SetActive(true);
        }
    }

    private void JoinRunner()
    {
        if (_isJoined) { return; }
        _isJoined = true;
        if (SoundsController.Instance.IsSoundTurned())
        {
            _soundOnJoin.Play();
        }
        HapticPatterns.PlayPreset(_vibrationsOnJoin);
        GameObject.FindObjectOfType<Runner>().AddPockemon(this);
    }

    public void SetOnPlace(Transform parent)
    {
        StartCoroutine(SetOnPlaceCoroutine(parent));
    }
    
    public void StartBattle()
    {
        _animator.SetBool("IsRunning", false);
    }

    private IEnumerator SetOnPlaceCoroutine(Transform parent)
    {
        Vector3 pos = transform.position;
        transform.SetParent(parent);
        transform.position = pos;

        _animator.SetBool("IsRunning", true);
        Vector3 targetPosition = new Vector3(0, transform.position.y, 0);

        transform.localRotation = Quaternion.Euler(Vector3.zero);

        LeanTween.moveLocal(gameObject, targetPosition, _timeMoveJoin);
        yield return new WaitForSeconds(_timeMoveJoin);
    }
}
