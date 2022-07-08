using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _footsteps;
    [SerializeField] private AudioClip _levelLose;
    [SerializeField] private AudioClip _levelWin;
    [SerializeField] private AudioClip _getPower;

    public static SoundsController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        if (PlayerPrefs.GetInt("Sound") < 1)
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
    }

    public bool IsSoundTurned()
    {
        return (PlayerPrefs.GetInt("Sound") == 1);
    }

    private void OnEnable()
    {
        GameObject.FindObjectOfType<GameController>().GameStart += TurnOnFootsteps;
    }

    private void TurnOnFootsteps()
    {
        if (IsSoundTurned())
        {
            _audioSource.clip = _footsteps;
            _audioSource.pitch = 1.5f;
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

    public void TurnOffFootsteps()
    {
        _audioSource.Stop();
        _audioSource.pitch = 1;
    }

    public void LevelLose()
    {
        if (IsSoundTurned())
        {
            _audioSource.clip = _levelLose;
            _audioSource.loop = false;
            _audioSource.Play();
        }
    }

    public void LevelWin()
    {
        if (IsSoundTurned())
        {
            _audioSource.clip = _levelWin;
            _audioSource.loop = false;
            _audioSource.Play();
        }
    }

    public void GetPower()
    {
        if (IsSoundTurned())
        {
            _audioSource.clip = _getPower;
            _audioSource.loop = false;
            _audioSource.Play();
        }
    }
}

