using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemCountable : MonoBehaviour
{
    [SerializeField] private GameObject _buttonBuy;
    [SerializeField] private int _amountToAdd;
    [SerializeField] private Text _countText;
    [SerializeField] private string _namePlayerPrefs;
    [SerializeField] private int _price;
    [SerializeField] private Text _priceText;
    [SerializeField] private float _limitLow=-1;

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
        _countText.text = "x" + PlayerPrefs.GetInt(_namePlayerPrefs);
        _priceText.text = _price + "";

        if (_limitLow >= PlayerPrefs.GetInt(_namePlayerPrefs))
        {
            _buttonBuy.SetActive(false);
        }
        else
        {
            _buttonBuy.SetActive(true);
            //_buttonBuy.GetComponent<Button>().interactable = (_price <= MoneyController.Instance.GetMoneyAmount());
        }
    }

    private void Buy()
    {
        if (_price <= MoneyController.Instance.GetMoneyAmount())
        {
            PlayerPrefs.SetInt(_namePlayerPrefs, PlayerPrefs.GetInt(_namePlayerPrefs) + _amountToAdd);
            MoneyController.Instance.AddMoneyAmount(-_price);
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
