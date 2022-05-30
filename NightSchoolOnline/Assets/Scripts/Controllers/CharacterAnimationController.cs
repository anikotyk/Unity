using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private PhotonView _view;
    private Animator _animator;

    private StarterAssets.StarterAssetsInputs _movement;

    private void Awake()
    {
        _view = GetComponent<PhotonView>();
        if (!_view.IsMine)
        {
            Destroy(this);
        }
        _movement = GameObject.FindObjectOfType<StarterAssets.StarterAssetsInputs>();
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (_movement.move.magnitude < 0.1f)
        {
            _animator.SetFloat("SpeedForward", 0f);
            _animator.SetFloat("SpeedRight", 0f);
        }
        else
        {
           
            _animator.SetFloat("SpeedForward", _movement.move.normalized.x * 1.5f);
            _animator.SetFloat("SpeedRight", _movement.move.normalized.y * 1.5f);
        }
    }
}
