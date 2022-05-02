using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    [SerializeField] private Text _moneyText;
    [SerializeField] private AudioSource _moneyAudioSource;
    
    public static MoneyController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        ShowMoneyCountText();
    }
    
    public void AddMoneyAmount(int amount)
    {
        PlayerPrefs.SetInt("money", GetMoneyAmount() + amount);
        if (GetMoneyAmount() < 0)
        {
            PlayerPrefs.SetInt("money", 0);
        }

        ShowMoneyCountText();
        PlayMoneySound();
    }

    public int GetMoneyAmount()
    {
        return PlayerPrefs.GetInt("money");
    }

    private void ShowMoneyCountText()
    {
        if (_moneyText != null)
        {
            _moneyText.text = GetMoneyAmount()+"";
        }
    }

    private void PlayMoneySound()
    {
        if (_moneyAudioSource != null)
        {
            _moneyAudioSource.Play();
        }
    }
}
