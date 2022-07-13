using UnityEngine;
using UnityEngine.Advertisements;

public class AdsInitializer : MonoBehaviour, IUnityAdsInitializationListener
{
    [SerializeField] private string _androidGameId;
    [SerializeField] private string _iOSGameId;
    [SerializeField] private bool _testMode;

    private string _gameId;

    void Awake()
    {
        InitializeAds();
    }

    public void InitializeAds()
    {
        _gameId = (Application.platform == RuntimePlatform.IPhonePlayer)
            ? _iOSGameId
            : _androidGameId;

        if (Advertisement.isInitialized)
        {
            GameObject.FindObjectOfType<ContinueButton>().LoadAd();
            GameObject.FindObjectOfType<MoneyButton>().LoadAd();
            InterstitialAd.S.LoadAd();
        }
        else
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        GameObject.FindObjectOfType<ContinueButton>().LoadAd();
        GameObject.FindObjectOfType<MoneyButton>().LoadAd();
        InterstitialAd.S.LoadAd();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }
}