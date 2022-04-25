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
    [SerializeField] private GameObject tapText;

    [SerializeField] private GameObject levelEnd;

    [SerializeField] private GameObject buttonContinue;
    [SerializeField] private GameObject buttonNextLevel;
    [SerializeField] private GameObject buttonRestart;

    [SerializeField] private GameObject LevelCompletedContainer;
    [SerializeField] private GameObject LevelFailedContainer;
    [SerializeField] private Text LevelCompletedText;
    [SerializeField] private Text LevelFailedText;

    [SerializeField] private GameObject endMoneyContainer;
    [SerializeField] private Text endMoneyText;

    [SerializeField] private GameObject timer;

    [SerializeField] private GameObject shopButton;

    [SerializeField] private Text speederText;
    
    [SerializeField] private GameObject speederBtn;
    [SerializeField] private GameObject adsBtn;
    [SerializeField] private GameObject shopBtn;
    
    [SerializeField] private GameObject moneyPanel;

    [SerializeField] private GameObject panelTooFast;
    [SerializeField] private GameObject panelTooFastImage;

    [SerializeField] private GameObject buttonRestartSmall;

    [SerializeField] private GameObject panelWaitingOnClick;
    
    private int counterGoldWindow;
    
    public GameObject toDestroyIfContinueRunning;

    public event UnityAction LevelStarted;
    public event UnityAction LevelContinued;
    public event UnityAction LevelEnded;
    public event UnityAction AddedMoney;
    public event UnityAction NextLevelClicked;
    public event UnityAction LevelLooseScreen;

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
        if (PlayerPrefs.GetInt("isFirstTime") != 1)
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
        
        counterGoldWindow = 0;
        UpdateSpeederCount();
        moneyPanel.SetActive(false);
        speederBtn.SetActive(false);
        levelEnd.SetActive(false);
    }

    private void Start()
    {
        InitEvents();

        shopBtn.GetComponent<AnimController>().Show();
        adsBtn.GetComponent<AnimController>().Show();

        SetStateWaitingOnClick();
    }

    private void InitEvents()
    {
        HealthController.Instance.LevelLoose += OnLevelLoose;
        PlayerController.Instance.LevelComplete += LevelComplete;
        PlayerController.Instance.SpeederCountChanged += UpdateSpeederCount;

        AdsController.Instance.ContinueButtonClicked += ContinuePlaying;
    }
    

    private void OnDestroy()
    {
        HealthController.Instance.LevelLoose -= OnLevelLoose;
        PlayerController.Instance.LevelComplete -= LevelComplete;
        PlayerController.Instance.SpeederCountChanged -= UpdateSpeederCount;
        AdsController.Instance.ContinueButtonClicked -= ContinuePlaying;
    }
    
    private void UpdateSpeederCount()
    {
        speederText.text = "x" + PlayerPrefs.GetInt("speeder");
    }

    public void ShowEndLevel(bool isLevelCompleted=true)
    {
        levelEnd.SetActive(true);
        
        if (isLevelCompleted)
        {
            LevelFailedContainer.SetActive(false);
            LevelCompletedContainer.SetActive(true);
            shopButton.SetActive(true);
            shopButton.GetComponent<Animation>().Play();
            LevelCompletedContainer.GetComponent<Animation>().Play();
            LevelCompletedText.text = "LEVEL " + PlayerPrefs.GetInt("level") + "\nCOMPLETED";
            buttonNextLevel.SetActive(true);
            buttonNextLevel.GetComponent<Animation>().Play();
            buttonContinue.SetActive(false);
            buttonRestart.SetActive(false);
            buttonRestartSmall.SetActive(false);
            endMoneyContainer.SetActive(true);
            endMoneyContainer.GetComponent<Animation>().Play();
            endMoneyText.text = "+"+ (counterGoldWindow*5);
            counterGoldWindow = 0;
            timer.SetActive(false);
        }
        else
        {
            LevelFailedContainer.SetActive(true);
            shopButton.SetActive(false);
            LevelFailedContainer.GetComponent<Animation>().Play();
            LevelCompletedContainer.SetActive(false);
            LevelFailedText.GetComponent<Text>().text = "LEVEL " + PlayerPrefs.GetInt("level") + "\nFAILED";
            buttonNextLevel.SetActive(false);
            buttonContinue.SetActive(true);
            buttonContinue.GetComponent<Animation>().Play();

            buttonRestart.SetActive(false);
            buttonRestartSmall.SetActive(true);
            buttonRestartSmall.GetComponent<Animation>().Play();
            endMoneyContainer.SetActive(false);
            timer.SetActive(true);
            timer.GetComponent<Image>().fillAmount = 1;
            StartCoroutine(AdsCoroutine(5f));
        }
    }
    

    public IEnumerator AdsCoroutine(float time)
    {
        float timeSpan = time / 100.0f;
        while(timer.GetComponent<Image>().fillAmount > 0)
        {
            timer.GetComponent<Image>().fillAmount -= 0.01f;
            yield return new WaitForSeconds(timeSpan);
        }

        buttonContinue.SetActive(false);
        buttonRestart.SetActive(true);
        buttonRestartSmall.SetActive(false);
        buttonRestart.GetComponent<Animation>().Play();
        shopButton.SetActive(true);
        shopButton.GetComponent<Animation>().Play();
    }

    public void RestartLevel()
    {
        LevelEnded?.Invoke();
        levelEnd.SetActive(false);
        buttonNextLevel.SetActive(false);
        shopButton.SetActive(false);

        SetStateWaitingOnClick();
    }

    public void LevelComplete()
    {
        GameOver();
        LevelEnded?.Invoke();

        ShowEndLevel(true);
    }

    public void AddMoney(int amount = 5)
    {
        counterGoldWindow += 1;
        MoneyController.Instance.SetMoney(MoneyController.Instance.GetMoneyAmount() + amount);
        moneyPanel.SetActive(true);
        moneyPanel.GetComponent<Animation>().Play();
        AddedMoney?.Invoke();
    }

    public void LevelLoose()
    {
        LevelLooseScreen?.Invoke();
        if (PlayerPrefs.GetInt("speed")>=45)
        {
            panelTooFast.SetActive(true);
            panelTooFastImage.GetComponent<Animation>().Play();
        }
        else
        {
            ShowEndLevel(false);
        }
    }
    
    public void ContinuePlaying()
    {
        Destroy(toDestroyIfContinueRunning);
        levelEnd.SetActive(false);
        buttonContinue.SetActive(false);

        SetStateWaitingOnClick(true);
    }
    

    public void NextLevel()
    {
        NextLevelClicked?.Invoke();
        levelEnd.SetActive(false);
        buttonNextLevel.SetActive(false);

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
        
        tapText.SetActive(false);

        shopBtn.GetComponent<AnimController>().Hide();
        adsBtn.GetComponent<AnimController>().Hide();
        speederBtn.GetComponent<AnimController>().Show();
    }

    public void GameOver()
    {
        shopBtn.GetComponent<AnimController>().Show();
        adsBtn.GetComponent<AnimController>().Show();
        speederBtn.GetComponent<AnimController>().Hide();
    }

    private void SetStateWaitingOnClick(bool isContinue = false)
    {
        tapText.SetActive(true);
        panelWaitingOnClick.GetComponent<Button>().onClick.RemoveAllListeners();
        panelWaitingOnClick.GetComponent<Button>().onClick.AddListener(()=> {
            StartRunning(isContinue);
            panelWaitingOnClick.SetActive(false);
        });
        panelWaitingOnClick.SetActive(true);
    }

    private void OnLevelLoose()
    {
        GameOver();
        StartCoroutine(GameOverCoroutine());
    }

    public IEnumerator GameOverCoroutine()
    {
        yield return new WaitForSeconds(1f);
        LevelLoose();
    }
}