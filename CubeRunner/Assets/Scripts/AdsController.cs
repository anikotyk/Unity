using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

public class AdsController : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private bool _testMode = true;

    private string _gameId = "4547153"; 

    public static string _video = "Interstitial_Android";
    public static string _rewardedVideo = "Rewarded_Android";

    private bool isContinue=false;

    public event UnityAction ContinueButtonClicked;

    void Start() 
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(_gameId, _testMode);
    }

    public void DeleteListener()
    {
        Advertisement.RemoveListener(this);
    }

    public void ShowAdsVideo(string placementId, bool isForContinue)
    {
        isContinue = isForContinue;
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
                    ContinueButtonClicked?.Invoke();
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
