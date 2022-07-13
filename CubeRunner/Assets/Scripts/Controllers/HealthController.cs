using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HealthController : MonoBehaviour
{
    [SerializeField] private GameObject[] _hearts;

    public int CurrentHealth { get; private set; }

    public event UnityAction LevelLose;

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
        GameController.Instance.LevelEnded += SetHealthToCurrentMax;
        GameObject.FindObjectOfType<ContinueButton>().ContinueButtonClicked += SetHealthToCurrentMax;
        GameController.Instance.NextLevelClicked += SetHealthToCurrentMax;
        GameController.Instance.LevelStarted += SetHealthToCurrentMax;
        GameController.Instance.LevelContinued += SetHealthToCurrentMax;

        SetHealthToCurrentMax();
    }

    private void OnDestroy()
    {
        PlayerController.Instance.GetDamage -= MinusHealth;
        GameController.Instance.LevelEnded -= SetHealthToCurrentMax;
        //GameObject.FindObjectOfType<ContinueButton>().ContinueButtonClicked -= SetHealthToCurrentMax;
        GameController.Instance.NextLevelClicked -= SetHealthToCurrentMax;
        GameController.Instance.LevelStarted -= SetHealthToCurrentMax;
        GameController.Instance.LevelContinued -= SetHealthToCurrentMax;
    }

    private void SetHealthToCurrentMax()
    {
        int currentMaxHealthCount = PlayerPrefs.GetInt("lives");
        CurrentHealth = currentMaxHealthCount;

        for (int i = 0; i < currentMaxHealthCount; i++)
        {
            _hearts[i].SetActive(true);
            _hearts[i].GetComponent<Animation>().Play("HeartReturn");
        }

        for(int i = currentMaxHealthCount; i < _hearts.Length; i++)
        {
            _hearts[i].SetActive(false);
        }
    }

    private void MinusHealth()
    {
        _hearts[CurrentHealth - 1].GetComponent<Animation>().Play("Heart");
        CurrentHealth -= 1;

        if (CurrentHealth <= 0)
        {
            LevelLose?.Invoke();
        }
    }
}
