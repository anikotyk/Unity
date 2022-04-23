using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.SceneManagement;

public class Buttons : MonoBehaviour
{
    public void OpenScene(string scene)
    {
        if (GameObject.FindObjectOfType<AdsController>())
        {
            GameObject.FindObjectOfType<AdsController>().DeleteListener();
        }
        
        SceneManager.LoadScene(scene);
    }
    
    public void Buy(ShopNewItemData data)
    {
        GameObject.FindObjectOfType<ShopController>().Buy(data);
    }
    
    public void AddCoins()
    {
        if(Advertisement.IsReady())
        {
            GameObject.FindObjectOfType<GameController>().isLocked = true;
            GameObject.FindObjectOfType<AdsController>().ShowAdsVideo(AdsController._rewardedVideo, false);
        }
    }
    
    public void ContinueBtn()
    {
        if (Advertisement.IsReady())
        {
            GameObject.FindObjectOfType<GameController>().isLocked = true;
            GameObject.FindObjectOfType<AdsController>().ShowAdsVideo(AdsController._rewardedVideo, true);
        }
    }

}
