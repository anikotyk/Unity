using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private bool _testMode = true;

    private string _gameId = "4702133";

    public static string interstitialVideo = "Interstitial_Android";
    public static string rewardedVideo = "Rewarded_Android";
    
    private void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(_gameId, _testMode);
    }

    public static void ShowAdsVideo(string placementId) 
    {
        if (Advertisement.IsReady())
        {
            Advertisement.Show(placementId);
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        
    }

    public void OnUnityAdsDidError(string message)
    {

    }

    public void OnUnityAdsDidStart(string placementId)
    {

    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            if (placementId == rewardedVideo)
            {
                GameObject.FindObjectOfType<GameController>().ContinueRuningExtraTime();
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
