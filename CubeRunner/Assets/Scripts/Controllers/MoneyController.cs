using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoneyController : MonoBehaviour
{
    [SerializeField] private Text moneyText;
    [SerializeField] private AudioSource moneySound;
    
    public static MoneyController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        SetMoneyText();
    }

    public void SetMoney(int amount)
    {
        PlayerPrefs.SetInt("money", amount);

        SetMoneyText();
        PlayMoneySound();
    }

    public int GetMoneyAmount()
    {
        return PlayerPrefs.GetInt("money");
    }

    private void SetMoneyText()
    {
        if (moneyText != null)
        {
            moneyText.text = GetMoneyAmount()+"";
        }
    }

    private void PlayMoneySound()
    {
        if (moneySound != null)
        {
            moneySound.Play();
        }
    }
}
