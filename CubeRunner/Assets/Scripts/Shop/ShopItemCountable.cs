using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemCountable : MonoBehaviour
{
    [SerializeField] private GameObject buttonBuy;
    [SerializeField] private int toAdd;
    [SerializeField] private Text countText;
    [SerializeField] private string namePlayerPrefs;
    [SerializeField] private int price;
    [SerializeField] private Text priceText;
    [SerializeField] private float limitLow;

    private void Awake()
    {
        ShowItem();
    }

    private void OnEnable()
    {
        buttonBuy.GetComponent<Button>().onClick.RemoveAllListeners();
        buttonBuy.GetComponent<Button>().onClick.AddListener(() => { Buy(); });
    }

    private void OnDisable()
    {
        buttonBuy.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    private void ShowItem()
    {
        countText.text = "x" + PlayerPrefs.GetInt(namePlayerPrefs);
        priceText.text = price + "";

        if (limitLow >= PlayerPrefs.GetInt(namePlayerPrefs))
        {
            buttonBuy.SetActive(false);
        }
        else
        {
            buttonBuy.SetActive(true);
        }
    }

    private void Buy()
    {
        if (price <= MoneyController.Instance.GetMoneyAmount())
        {
            PlayerPrefs.SetInt(namePlayerPrefs, PlayerPrefs.GetInt(namePlayerPrefs) + toAdd);
            ShowItem();
            MoneyController.Instance.SetMoney(MoneyController.Instance.GetMoneyAmount() - price);
        }
    }
}
