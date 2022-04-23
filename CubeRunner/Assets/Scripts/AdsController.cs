using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsController : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private bool _testMode = true;

    private string _gameId = "4547153"; 

    public static string _video = "Interstitial_Android";
    public static string _rewardedVideo = "Rewarded_Android";

    public bool isContinue=false;

    void Start() 
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(_gameId, _testMode);
    }

    public void DeleteListener()
    {
        Advertisement.RemoveListener(this);
    }

    public static void ShowAdsVideo(string placementId)
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show(placementId);
        }
        else
        {
            //Debug.Log("Advertisement not ready!");
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnUnityAdsDidError(string message)
    {
        GameObject.FindObjectOfType<GameController>().isLocked = false;
    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        GameObject.FindObjectOfType<GameController>().isLocked = false;
        
        if (showResult == ShowResult.Finished)
        {
            if(placementId == _rewardedVideo)
            {
                if (isContinue)
                {
                    GameObject.FindObjectOfType<GameController>().ContinuePlaying();
                }
                else
                {
                    PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money") + 100);
                }

            }
        }
        else if (showResult == ShowResult.Skipped)
        {

        }
        else if (showResult == ShowResult.Failed)
        {

        }
    }
}
