using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopNewItemData : MonoBehaviour
{
    public GameObject buttonBuy;
    public GameObject[] skillsGO;
    public int toAdd;
    public bool isFloat;
    public bool isCount;
    public float toAddFloat;
    public Text countText;
    public string namePlayerPrefs;
    public bool isPlayerPrefsCount;
    public string namePlayerPrefsCount;
    public int price;
    public int[] prices;
    public Text priceText;

    public float limitLow;
}
