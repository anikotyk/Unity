using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioSource _audioSource2;

    [SerializeField] private AudioClip _winSound;
    [SerializeField] private AudioClip _gameOverSound;
    [SerializeField] private AudioClip _getCoinSound;
    [SerializeField] private AudioClip _breakBallSound;
    [SerializeField] private AudioClip _breakingWindowSound;

    public static AudioController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    private void Start()
    {
        GameController.Instance.LevelEnded += OnLevelEnded;
        GameController.Instance.AddedMoney += PlayGetCoinSound;
        GameController.Instance.LevelLoseScreen += PlayGameOverSound;
        HealthController.Instance.LevelLose += OnLevelLose;
    }

    private void OnDestroy()
    {
        GameController.Instance.LevelEnded -= OnLevelEnded;
        GameController.Instance.AddedMoney -= PlayGetCoinSound;
        GameController.Instance.LevelLoseScreen -= PlayGameOverSound;
        HealthController.Instance.LevelLose -= OnLevelLose;
    }

    private void OnLevelEnded()
    {
        _audioSource.clip = _winSound;
        _audioSource.Play();
    }

    private void OnLevelLose()
    {
        _audioSource.clip = _breakBallSound;
        _audioSource.Play();
    }

    private void PlayGetCoinSound()
    {
        _audioSource2.clip = _getCoinSound;
        _audioSource2.Play();
    }

    private void PlayGameOverSound()
    {
        _audioSource.clip = _gameOverSound;
        _audioSource.Play();
    }

    public void PlayBreakingWindowSound()
    {
        _audioSource.clip = _breakingWindowSound;
        _audioSource.Play();
    }
}
