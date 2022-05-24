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
        if (_movement.move == Vector2.zero)
        {
            EndMovement();
            if(Mathf.Abs(_movement.look.x) < 0.1f)
            {
                EndTurning();
            }
            else
            {
                if (_movement.look.x < 0)
                {
                    _animator.SetBool("isLeft", true);
                }
                else
                {
                    _animator.SetBool("isLeft", false);
                }

                StartTurning();
            }
        }
        else
        {
            StartMovement();
        }
    }

    private void StartMovement()
    {
        _animator.SetBool("isWalking", true);
    }

    private void EndMovement()
    {
        _animator.SetBool("isWalking", false);
    }

    private void StartTurning()
    {
        _animator.SetBool("isTurning", true);
    }

    private void EndTurning()
    {
        _animator.SetBool("isTurning", false);
    }
}
