using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemImprover : MonoBehaviour
{
    [SerializeField] private GameObject _buttonBuy;
    [SerializeField] private string _namePlayerPrefsIndex;
    [SerializeField] private string _namePlayerPrefsValue;
    [SerializeField] private Text _priceText;
    [SerializeField] private Sprite _boughtSprite;
    [SerializeField] private GameObject[] _improvementCells;
    [SerializeField] private int[] _prices;
    [SerializeField] private int[] _valuesToSet;

    [SerializeField] private GameObject _warningPanel;

    private void Start()
    {
        ShowItem();
    }

    private void OnEnable()
    {
        _buttonBuy.GetComponent<Button>().onClick.RemoveAllListeners();
        _buttonBuy.GetComponent<Button>().onClick.AddListener(() => { Buy(); });
    }

    private void OnDisable()
    {
        _buttonBuy.GetComponent<Button>().onClick.RemoveAllListeners();
    }

    public void ShowItem()
    {
        int count = PlayerPrefs.GetInt(_namePlayerPrefsIndex);
        for (int j = 0; j < Mathf.Min(count, _improvementCells.Length); j++)
        {
            _improvementCells[j].GetComponent<Image>().sprite = _boughtSprite;
        }
        if (count >= _prices.Length)
        {
            _buttonBuy.SetActive(false);
        }
        else
        {
            _priceText.text = _prices[count] + "";
            _buttonBuy.SetActive(true);
            //_buttonBuy.GetComponent<Button>().interactable = (_prices[count] <= MoneyController.Instance.GetMoneyAmount());
        }
    }

    private void Buy()
    {
        int count = PlayerPrefs.GetInt(_namePlayerPrefsIndex);
        int price;
        if (count < _prices.Length)
        {
            price = _prices[count];
        }
        else
        {
            ShowItem();
            return;
        }
        
        if (price <= MoneyController.Instance.GetMoneyAmount())
        {
            PlayerPrefs.SetInt(_namePlayerPrefsValue, _valuesToSet[count]);
            PlayerPrefs.SetInt(_namePlayerPrefsIndex, PlayerPrefs.GetInt(_namePlayerPrefsIndex) + 1);
            
            MoneyController.Instance.AddMoneyAmount(-price);
            ReShowAllItems();
        }
        else
        {
            _warningPanel.SetActive(true);
        }

    }

    private void ReShowAllItems()
    {
        foreach (ShopItemCountable item in GameObject.FindObjectsOfType<ShopItemCountable>())
        {
            item.ShowItem();
        }
        foreach (ShopItemImprover item in GameObject.FindObjectsOfType<ShopItemImprover>())
        {
            item.ShowItem();
        }
    }
}