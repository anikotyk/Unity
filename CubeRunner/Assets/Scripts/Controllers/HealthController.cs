using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] private GameObject[] hearts;

    public int CurrentHealth { get; private set; }

    public event UnityAction LevelLoose;

    public static HealthController Instance { get; private set; }

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
        PlayerController.Instance.GetDamage += MinusHealth;
        GameController.Instance.LevelEnded += SetHealthToMax;
        AdsController.Instance.ContinueButtonClicked += SetHealthToMax;
        GameController.Instance.NextLevelClicked += SetHealthToMax;
        GameController.Instance.LevelStarted += SetHealthToMax;
        GameController.Instance.LevelContinued += SetHealthToMax;

        SetHealthToMax();
    }

    private void OnDestroy()
    {
        PlayerController.Instance.GetDamage -= MinusHealth;
        GameController.Instance.LevelEnded -= SetHealthToMax;
        AdsController.Instance.ContinueButtonClicked -= SetHealthToMax;
        GameController.Instance.NextLevelClicked -= SetHealthToMax;
        GameController.Instance.LevelStarted -= SetHealthToMax;
        GameController.Instance.LevelContinued -= SetHealthToMax;
    }

    private void SetHealthToMax()
    {
        CurrentHealth = PlayerPrefs.GetInt("lives");

        for (int i = 0; i < CurrentHealth; i++)
        {
            hearts[i].SetActive(true);
            hearts[i].GetComponent<Animation>().Play("HeartReturn");
        }

        for(int i = CurrentHealth; i < hearts.Length; i++)
        {
            hearts[i].SetActive(false);
        }
    }

    private void MinusHealth()
    {
        hearts[CurrentHealth - 1].GetComponent<Animation>().Play("Heart");
        CurrentHealth -= 1;

        if (CurrentHealth <= 0)
        {
            LevelLoose?.Invoke();
        }
    }
}
