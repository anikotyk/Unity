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

    private GameController gameController;
    private PlayerController playerController;
    
    private void Awake()
    {
        gameController = GameObject.FindObjectOfType<GameController>();
        playerController = GameObject.FindObjectOfType<PlayerController>();
    }

    private void OnEnable()
    {
        gameController.LevelEnded += OnLevelEnded;
        gameController.AddedMoney += MoneySound;
        gameController.LevelLooseScreen += GameOverSound;
        playerController.LevelLoose += OnLevelLoose;
    }

    private void OnDisable()
    {
        gameController.LevelEnded -= OnLevelEnded;
        gameController.AddedMoney -= MoneySound;
        gameController.LevelLooseScreen -= GameOverSound;
        playerController.LevelLoose -= OnLevelLoose;
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
