using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ShopController : MonoBehaviour
{
    public GameObject itemsContainer;

    public Text coinsText;
    
    public GameObject LoadingCanvas;

    public Sprite boughtSprite;

    public AudioSource audioController;

    public void Awake()
    {
        ShowMoney();
        ReShowAllItems();
    }

    public void ShowMoney()
    {
        coinsText.text = PlayerPrefs.GetInt("money") + "";
    }

    public void LoadScene(string scene)
    {
        StartCoroutine(LoadingSceneCoroutine(scene));
    }

    public IEnumerator LoadingSceneCoroutine(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);
        LoadingCanvas.SetActive(true);
        while (!operation.isDone)
        {
            yield return null;
        }
    }
    
    public void Buy(ShopNewItemData data)
    {
        int count;
        if (data.isPlayerPrefsCount)
        {
            count = PlayerPrefs.GetInt(data.namePlayerPrefsCount);
        }
        else
        {
            count = PlayerPrefs.GetInt(data.namePlayerPrefs);

        }

        int price;
        if(data.prices.Length > 0 && count< data.prices.Length)
        {
            price = data.prices[count-1];
        }
        else
        {
            price = data.price;
        }

        if (price <= PlayerPrefs.GetInt("money"))
        {
            audioController.Play();
            PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") - price);

            if (data.isPlayerPrefsCount)
            {
                PlayerPrefs.SetInt(data.namePlayerPrefsCount, PlayerPrefs.GetInt(data.namePlayerPrefsCount) + 1);
            }

            if (data.isFloat)
            {
                PlayerPrefs.SetFloat(data.namePlayerPrefs, PlayerPrefs.GetFloat(data.namePlayerPrefs) + data.toAddFloat);
            }
            else{
                PlayerPrefs.SetInt(data.namePlayerPrefs, PlayerPrefs.GetInt(data.namePlayerPrefs) + data.toAdd);
            }
            
            if (data.skillsGO.Length > 0)
            {
                
                data.skillsGO[count].GetComponent<Image>().sprite = boughtSprite;

                if (count >= data.prices.Length)
                {
                    data.buttonBuy.SetActive(false);
                }
                else
                {
                    data.priceText.text = data.prices[count] + "";
                    data.buttonBuy.SetActive(true);
                }
            }
            else
            {
                if (data.isCount)
                {
                    data.countText.text = "x" + PlayerPrefs.GetInt(data.namePlayerPrefs);
                }

                if (data.isFloat)
                {
                    if (data.limitLow >= PlayerPrefs.GetFloat(data.namePlayerPrefs))
                    {
                        data.buttonBuy.SetActive(false);
                    }
                    else
                    {
                        data.buttonBuy.SetActive(true);
                    }
                }
                else
                {
                    if (data.limitLow >= PlayerPrefs.GetInt(data.namePlayerPrefs))
                    {
                        data.buttonBuy.SetActive(false);
                    }
                    else
                    {
                        data.buttonBuy.SetActive(true);
                    }
                }
            }

            

            ShowMoney();
        }
        
    }

    public void ReShowAllItems()
    {
        for(int i=0; i<itemsContainer.transform.childCount; i++)
        {
            ShopNewItemData data = itemsContainer.transform.GetChild(i).GetComponent<ShopNewItemData>();
            if (data.skillsGO.Length > 0)
            {
                int count;
                if (data.isPlayerPrefsCount)
                {
                    count = PlayerPrefs.GetInt(data.namePlayerPrefsCount);
                }
                else
                {
                    count = PlayerPrefs.GetInt(data.namePlayerPrefs);

                }
                
                for(int j=0; j < count; j++)
                {
                    data.skillsGO[j].GetComponent<Image>().sprite = boughtSprite;
                }
                if(count-1 >= data.prices.Length)
                {
                    data.buttonBuy.SetActive(false);
                }
                else
                {
                    data.priceText.text = data.prices[count-1] + "";
                    data.buttonBuy.SetActive(true);
                }
            }
            else
            {
                if (data.isCount)
                {
                    data.countText.text = "x" + PlayerPrefs.GetInt(data.namePlayerPrefs);
                }
                
                data.priceText.text = data.price + "";

                if (data.isFloat)
                {
                    if (data.limitLow >= PlayerPrefs.GetFloat(data.namePlayerPrefs))
                    {
                        data.buttonBuy.SetActive(false);
                    }
                    else
                    {
                        data.buttonBuy.SetActive(true);
                    }
                }
                else
                {
                    if (data.limitLow >= PlayerPrefs.GetInt(data.namePlayerPrefs))
                    {
                        data.buttonBuy.SetActive(false);
                    }
                    else
                    {
                        data.buttonBuy.SetActive(true);
                    }
                }
            }
        }
    }

}
