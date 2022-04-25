using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemImprover : MonoBehaviour
{
    [SerializeField] private GameObject buttonBuy;
    [SerializeField] private string namePlayerPrefsIndex;
    [SerializeField] private string namePlayerPrefsValue;
    [SerializeField] private Text priceText;
    [SerializeField] private Sprite boughtSprite;
    [SerializeField] private GameObject[] improvementCells;
    [SerializeField] private int[] prices;
    [SerializeField] private int[] valuesToSet;

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
        int count = PlayerPrefs.GetInt(namePlayerPrefsIndex);
        for (int j = 0; j < Mathf.Min(count, improvementCells.Length); j++)
        {
            improvementCells[j].GetComponent<Image>().sprite = boughtSprite;
        }
        if (count >= prices.Length)
        {
            buttonBuy.SetActive(false);
        }
        else
        {
            priceText.text = prices[count] + "";
            buttonBuy.SetActive(true);
        }
    }

    private void Buy()
    {
        int count = PlayerPrefs.GetInt(namePlayerPrefsIndex);
        int price;
        if (count < prices.Length)
        {
            price = prices[count];
        }
        else
        {
            ShowItem();
            return;
        }
        
        if (price <= MoneyController.Instance.GetMoneyAmount())
        {
            PlayerPrefs.SetInt(namePlayerPrefsValue, valuesToSet[count]);
            PlayerPrefs.SetInt(namePlayerPrefsIndex, PlayerPrefs.GetInt(namePlayerPrefsIndex) + 1);

            ShowItem();
            MoneyController.Instance.SetMoney(MoneyController.Instance.GetMoneyAmount() - price);
        }

    }
}