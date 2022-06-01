using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StarterAssets.FirstPersonController))]
public class PlayerController : MonoBehaviour
{
    private DialogController _dialogController;
    private StarterAssets.FirstPersonController _fpc;

    private void Awake()
    {
        _fpc = GetComponent<StarterAssets.FirstPersonController>();
        _dialogController = GameObject.FindObjectOfType<DialogController>();
    }

    private void OnEnable()
    {
        _dialogController.StartDialog += OnStartedDialog;
        _dialogController.EndDialog += OnEndedDialog;
    }

    private void OnDisable()
    {
        _dialogController.StartDialog -= OnStartedDialog;
        _dialogController.EndDialog -= OnEndedDialog;
    }

    private void OnStartedDialog()
    {
        _fpc.enabled = false;
    }

    private void OnEndedDialog()
    {
        _fpc.enabled = true;
    }
}
