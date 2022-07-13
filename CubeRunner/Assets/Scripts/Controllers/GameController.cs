using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject _tapText;

    [SerializeField] private GameObject _levelEndPanel;

    [SerializeField] private GameObject _buttonContinue;
    [SerializeField] private GameObject _buttonNextLevel;
    [SerializeField] private GameObject _buttonRestart;

    [SerializeField] private GameObject _levelCompletedContainer;
    [SerializeField] private GameObject _levelFailedContainer;
    [SerializeField] private Text _levelCompletedText;
    [SerializeField] private Text _levelFailedText;

    [SerializeField] private GameObject _addedMoneyContainer;
    [SerializeField] private Text _addedMoneyText;

    [SerializeField] private GameObject _timer;

    [SerializeField] private GameObject _shopButton;

    [SerializeField] private Text _speederCountText;
    
    [SerializeField] private GameObject _speederBtn;
    [SerializeField] private GameObject _adsBtn;
    [SerializeField] private GameObject _shopBtn;
    [SerializeField] private GameObject _settingsBtn;

    [SerializeField] private GameObject _plusCoinsPanel;

    [SerializeField] private GameObject _panelTooFast;
    [SerializeField] private GameObject _panelTooFastImage;

    [SerializeField] private GameObject _buttonRestartSmall;

    [SerializeField] private GameObject _panelWaitingForClick;

    [SerializeField] private GameObject _levelBarContainer;
    [SerializeField] private GameObject _levelBar;
    
    private int _counterBrokenGoldWindow;
    
    public GameObject ToDestroyIfContinueRunning;

    public event UnityAction LevelStarted;
    public event UnityAction LevelContinued;
    public event UnityAction LevelEnded;
    public event UnityAction LevelCompleted;
    public event UnityAction AddedMoney;
    public event UnityAction NextLevelClicked;
    public event UnityAction LevelLoseScreen;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        //PlayerPrefs.SetInt("isFirstTime", 0);
        //PlayerPrefs.SetInt("money", 1000);
        if (PlayerPrefs.GetInt("isFirstTime") < 1)
        {
            PlayerPrefs.SetInt("money", 100);
            PlayerPrefs.SetInt("isFirstTime", 1);
            PlayerPrefs.SetInt("speeder", 3);
            PlayerPrefs.SetInt("speederTime", 2);
            PlayerPrefs.SetInt("speederTimeIndex", 1);
            PlayerPrefs.SetInt("speed", 15);
            PlayerPrefs.SetInt("lives", 1);
            PlayerPrefs.SetInt("livesIndex", 1);
            PlayerPrefs.SetInt("level", 1);
        }
        
        _counterBrokenGoldWindow = 0;
        UpdateSpeederCount();
        _plusCoinsPanel.SetActive(false);
        _speederBtn.SetActive(false);
        _levelEndPanel.SetActive(false);
        ResetProgressBar();
    }

    private void Start()
    {
        
        InitEvents();

        _settingsBtn.GetComponent<AnimController>().Show();
        _shopBtn.GetComponent<AnimController>().Show();
        _adsBtn.GetComponent<AnimController>().Show();

        SetStateWaitingOnClick();
    }

    private void InitEvents()
    {
        HealthController.Instance.LevelLose += OnLevelLose;
        PlayerController.Instance.LevelComplete += LevelComplete;
        PlayerController.Instance.SpeederCountChanged += UpdateSpeederCount;

        GameObject.FindObjectOfType<ContinueButton>().ContinueButtonClicked += ContinuePlaying;
    }
    

    private void OnDestroy()
    {
        HealthController.Instance.LevelLose -= OnLevelLose;
        PlayerController.Instance.LevelComplete -= LevelComplete;
        PlayerController.Instance.SpeederCountChanged -= UpdateSpeederCount;
       // GameObject.FindObjectOfType<ContinueButton>().ContinueButtonClicked -= ContinuePlaying;
    }

    public void SetProgress(float progress)
    {
        _levelBar.GetComponent<RectTransform>().sizeDelta = new Vector2(_levelBarContainer.GetComponent<RectTransform>().rect.width * progress, _levelBar.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void ResetProgressBar()
    {
        _levelBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0, _levelBar.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void UpdateSpeederCount()
    {
        _speederCountText.text = "x" + PlayerPrefs.GetInt("speeder");
    }

    public void ShowEndLevel(bool isLevelCompleted=true)
    {
        _levelEndPanel.SetActive(true);
        
        if (isLevelCompleted)
        {
            _levelFailedContainer.SetActive(false);
            _levelCompletedContainer.SetActive(true);
            _shopButton.SetActive(true);
            _shopButton.GetComponent<Animation>().Play();
            _levelCompletedContainer.GetComponent<Animation>().Play();
            _levelCompletedText.text = Lean.Localization.LeanLocalization.GetTranslationText("Level")+" " + PlayerPrefs.GetInt("level") + "\n"+ Lean.Localization.LeanLocalization.GetTranslationText("Completed");
            _buttonNextLevel.SetActive(true);
            _buttonNextLevel.GetComponent<Animation>().Play();
            _buttonContinue.SetActive(false);
            _buttonRestart.SetActive(false);
            _buttonRestartSmall.SetActive(false);
            _addedMoneyContainer.SetActive(true);
            _addedMoneyContainer.GetComponent<Animation>().Play();
            _addedMoneyText.text = "+"+ (_counterBrokenGoldWindow*5);
            _counterBrokenGoldWindow = 0;
            _timer.SetActive(false);
            LevelCompleted?.Invoke();
        }
        else
        {
            _levelFailedContainer.SetActive(true);
            _shopButton.SetActive(false);
            _levelFailedContainer.GetComponent<Animation>().Play();
            _levelCompletedContainer.SetActive(false);
            _levelFailedText.GetComponent<Text>().text = Lean.Localization.LeanLocalization.GetTranslationText("Level") + " " + PlayerPrefs.GetInt("level") + "\n"+ Lean.Localization.LeanLocalization.GetTranslationText("Failed");
            _buttonNextLevel.SetActive(false);
            _buttonContinue.SetActive(true);
            _buttonContinue.GetComponent<Animation>().Play();

            _buttonRestart.SetActive(false);
            _buttonRestartSmall.SetActive(true);
            _buttonRestartSmall.GetComponent<Animation>().Play();
            _addedMoneyContainer.SetActive(false);
            _timer.SetActive(true);
            _timer.GetComponent<Image>().fillAmount = 1;
            StartCoroutine(AdsCoroutine(5f));
        }
    }
    

    private IEnumerator AdsCoroutine(float time)
    {
        float timeSpan = time / 100.0f;
        while(_timer.GetComponent<Image>().fillAmount > 0)
        {
            _timer.GetComponent<Image>().fillAmount -= 0.01f;
            yield return new WaitForSeconds(timeSpan);
        }

        _buttonContinue.SetActive(false);
        _buttonRestart.SetActive(true);
        _buttonRestartSmall.SetActive(false);
        _buttonRestart.GetComponent<Animation>().Play();
        _shopButton.SetActive(true);
        _shopButton.GetComponent<Animation>().Play();
    }

    public void RestartLevel()
    {
        LevelEnded?.Invoke();
        ResetProgressBar();
        _levelEndPanel.SetActive(false);
        _buttonNextLevel.SetActive(false);
        _shopButton.SetActive(false);

        SetStateWaitingOnClick();
    }

    public void LevelComplete()
    {
        SetProgress(1f);
        GameOver();
        LevelEnded?.Invoke();

        ShowEndLevel(true);
    }

    public void AddMoney(int amount = 5)
    {
        _counterBrokenGoldWindow += 1;
        MoneyController.Instance.AddMoneyAmount(amount);
        _plusCoinsPanel.SetActive(true);
        _plusCoinsPanel.GetComponent<Animation>().Play();
        AddedMoney?.Invoke();
    }

    public void LevelLose()
    {
        LevelLoseScreen?.Invoke();
        if (PlayerPrefs.GetInt("speed")>=45)
        {
            _panelTooFast.SetActive(true);
            _panelTooFastImage.GetComponent<Animation>().Play();
        }
        else
        {
            ShowEndLevel(false);
        }
    }
    
    public void ContinuePlaying()
    {
        Destroy(ToDestroyIfContinueRunning);
        _levelEndPanel.SetActive(false);
        _buttonContinue.SetActive(false);

        SetStateWaitingOnClick(true);
    }
    

    public void NextLevel()
    {
        NextLevelClicked?.Invoke();
        ResetProgressBar();
        _levelEndPanel.SetActive(false);
        _buttonNextLevel.SetActive(false);

        SetStateWaitingOnClick();
    }
    
    public void StartRunning(bool isContinue=false)
    {
        if (!isContinue)
        {
            LevelStarted?.Invoke();
        }
        else
        {
            LevelContinued?.Invoke();
        }
        
        _tapText.SetActive(false);

        _settingsBtn.GetComponent<AnimController>().Hide();
        _shopBtn.GetComponent<AnimController>().Hide();
        _adsBtn.GetComponent<AnimController>().Hide();
        _speederBtn.GetComponent<AnimController>().Show();
    }

    public void GameOver()
    {
        _settingsBtn.GetComponent<AnimController>().Show();
        _shopBtn.GetComponent<AnimController>().Show();
        _adsBtn.GetComponent<AnimController>().Show();
        _speederBtn.GetComponent<AnimController>().Hide();
    }

    private void SetStateWaitingOnClick(bool isContinue = false)
    {
        _tapText.SetActive(true);
        _panelWaitingForClick.GetComponent<Button>().onClick.RemoveAllListeners();
        _panelWaitingForClick.GetComponent<Button>().onClick.AddListener(()=> {
            StartRunning(isContinue);
            _panelWaitingForClick.SetActive(false);
        });
        _panelWaitingForClick.SetActive(true);
    }

    private void OnLevelLose()
    {
        GameOver();
        StartCoroutine(GameOverCoroutine());
    }

    private IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1f);
        LevelLose();
    }
}