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
            AdsController.Instance.DeleteListener();
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
            GameController.Instance.isLocked = true;
            AdsController.Instance.ShowAdsVideo(AdsController._rewardedVideo, false);
        }
    }
    
    public void ContinueBtn()
    {
        if (Advertisement.IsReady())
        {
            GameController.Instance.isLocked = true;
            AdsController.Instance.ShowAdsVideo(AdsController._rewardedVideo, true);
        }
    }

}
