using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSource2;

    [SerializeField] private AudioClip win;
    [SerializeField] private AudioClip gameover;
    [SerializeField] private AudioClip getcoin;
    [SerializeField] private AudioClip breakball;
    [SerializeField] private AudioClip breakingSound;

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
        GameController.Instance.AddedMoney += MoneySound;
        GameController.Instance.LevelLooseScreen += GameOverSound;
        HealthController.Instance.LevelLoose += OnLevelLoose;
    }

    private void OnDestroy()
    {
        GameController.Instance.LevelEnded -= OnLevelEnded;
        GameController.Instance.AddedMoney -= MoneySound;
        GameController.Instance.LevelLooseScreen -= GameOverSound;
        HealthController.Instance.LevelLoose -= OnLevelLoose;
    }

    private void OnLevelEnded()
    {
        audioSource.clip = win;
        audioSource.Play();
    }

    private void OnLevelLoose()
    {
        audioSource.clip = breakball;
        audioSource.Play();
    }

    private void MoneySound()
    {
        audioSource2.clip = getcoin;
        audioSource2.Play();
    }

    private void GameOverSound()
    {
        audioSource.clip = gameover;
        audioSource.Play();
    }

    public void BreakingSound()
    {
        audioSource.clip = breakingSound;
        audioSource.Play();
    }
}
