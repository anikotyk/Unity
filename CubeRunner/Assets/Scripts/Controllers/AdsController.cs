using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;

public class AdsController : MonoBehaviour, IUnityAdsListener
{
    [SerializeField] private bool _testMode = true;
    [SerializeField] private int _countOfPlayedGamesBeforeShowAds = 3;
    [SerializeField] private int _moneyAmountAfterAds = 100;

    private const string _gameId = "4547153";

    private const string _video = "Interstitial_Android";
    private const string _rewardedVideo = "Rewarded_Android";

    private bool _isContinue = false;
    private int _countOfPlayedGames;
    
    public static AdsController Instance { get; private set; }
    public event UnityAction ContinueButtonClicked;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        _countOfPlayedGames = 0;
    }

    private void Start()
    {
        Advertisement.AddListener(this);
        Advertisement.Initialize(_gameId, _testMode);

        GameController.Instance.LevelEnded += IncreaseCounterForAds;
    }

    private void OnDestroy()
    {
        DeleteListener();
        GameController.Instance.LevelEnded -= IncreaseCounterForAds;
    }

    private void IncreaseCounterForAds()
    {
        _countOfPlayedGames++;
        if (_countOfPlayedGames >= _countOfPlayedGamesBeforeShowAds)
        {
            _countOfPlayedGames = 0;
            ShowAdsVideo(_video, false);
        }
    }

    private void DeleteListener()
    {
        Advertisement.RemoveListener(this);
    }
    
    public void ShowRewardedVideo( bool isForContinue)
    {
        ShowAdsVideo(_rewardedVideo, isForContinue);
    }

    public void ShowAdsVideo(string placementId, bool isForContinue)
    {
        _isContinue = isForContinue;
        if (Advertisement.IsReady())
        {
            Advertisement.Show(placementId);
        }
    }

    public void OnUnityAdsReady(string placementId)
    {
        //Ads ready
    }

    public void OnUnityAdsDidError(string message)
    {
        //Ads error
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        //Ads started
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            if(placementId == _rewardedVideo)
            {
                if (_isContinue)
                {
                    ContinueButtonClicked?.Invoke();
                }
                else
                {
                    MoneyController.Instance.AddMoneyAmount(_moneyAmountAfterAds);
                }
            }
        }
    }
}
