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
    [SerializeField] private GameObject[] hearts;
    [SerializeField] private GameObject tapText;

    [SerializeField] private GameObject LoadingCanvas;
    [SerializeField] private Text levelNow;
    [SerializeField] private Text levelNext;

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
    [SerializeField] private Text speedText;

    [SerializeField] private GameObject speederBtn;
    [SerializeField] private GameObject adsBtn;
    [SerializeField] private GameObject shopBtn;
    
    [SerializeField] private GameObject moneyPanel;

    [SerializeField] private GameObject panelTooFast;
    [SerializeField] private GameObject panelTooFastImage;

    [SerializeField] private GameObject buttonRestartSmall;

    public bool isLocked;

    private int adscount;
    private int health;
    private int counterGoldWindow;
    
    private PlayerController playerNew;
    private AdsController adsController;

    public GameObject toDestroyIfContinueRunning;

    public event UnityAction LevelStarted;
    public event UnityAction LevelContinued;
    public event UnityAction LevelEnded;
    public event UnityAction AddedMoney;
    public event UnityAction NextLevelClicked;
    public event UnityAction LevelLooseScreen;

    private void Awake()
    {
        playerNew = GameObject.FindObjectOfType<PlayerController>();
        adsController = GameObject.FindObjectOfType<AdsController>();
        //PlayerPrefs.SetInt("isFirstTime", 12);
        if (PlayerPrefs.GetInt("isFirstTime") != 18)
        {
            Debug.Log("FIRST TIME");
            PlayerPrefs.SetInt("money", 10000);
            PlayerPrefs.SetInt("isFirstTime", 18);
            PlayerPrefs.SetInt("speeder", 3);
            PlayerPrefs.SetInt("speederTime", 2);
            PlayerPrefs.SetInt("speederTimeCount", 1);
            PlayerPrefs.SetInt("speed", 15);
            PlayerPrefs.SetInt("lives", 1);
            PlayerPrefs.SetInt("level", 1);
        }

        adscount = 0;
        isLocked = false;
        ShowLevel();
        ShowSpeed(PlayerPrefs.GetInt("speed"));
        
        ShowHearts();
        counterGoldWindow = 0;
        moneyPanel.SetActive(false);
        speederBtn.SetActive(false);
        speederText.text ="x"+ PlayerPrefs.GetInt("speeder");
        levelEnd.SetActive(false);
    }

    private void Start()
    {
        shopBtn.GetComponent<AnimController>().Show();
        adsBtn.GetComponent<AnimController>().Show();

        StartCoroutine(WaitingForTap());
    }

    private void OnEnable()
    {
        playerNew.LevelLoose += OnLevelLoose;
        playerNew.SpeedChanged += ShowSpeed;
        playerNew.LevelComplete += LevelComplete;
        playerNew.SpeederCountChanged += UpdateSpeederCount;
        playerNew.GetDamage += MinusHealth;

        adsController.ContinueButtonClicked += ContinuePlaying;
    }

    private void OnDisable()
    {
        playerNew.LevelLoose -= OnLevelLoose;
        playerNew.SpeedChanged -= ShowSpeed;
        playerNew.LevelComplete -= LevelComplete;
        playerNew.SpeederCountChanged -= UpdateSpeederCount;
        playerNew.GetDamage -= MinusHealth;
        adsController.ContinueButtonClicked -= ContinuePlaying;
    }

    public void UpdateSpeed()
    {
        if (PlayerPrefs.GetInt("level") % 5 == 0)
        {
            PlayerPrefs.SetInt("speed", PlayerPrefs.GetInt("speed")+1);
            ShowSpeed(PlayerPrefs.GetInt("speed"));
        }
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
        adscount++;
        LevelEnded?.Invoke();
        ShowHearts();
        levelEnd.SetActive(false);
        buttonNextLevel.SetActive(false);
        shopButton.SetActive(false);

        StartCoroutine(WaitingForTap());
    }

    public void LevelComplete()
    {
        GameOver();
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        UpdateSpeed();
        LevelEnded?.Invoke();

        ShowEndLevel(true);
    }

    public void AddMoney(int amount = 5)
    {
        counterGoldWindow += 1;
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money")+amount);
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
        ShowHearts();
        buttonContinue.SetActive(false);
        
        StartCoroutine(WaitingForTap(true));
    }

    public void ShowLevel()
    {
        levelNow.text = PlayerPrefs.GetInt("level") + "";
        levelNext.text = PlayerPrefs.GetInt("level")+1 + "";
    }

    public void ShowHearts()
    {
        health = PlayerPrefs.GetInt("lives");
        for (int i = 0; i < 5; i++)
        {
            hearts[i].SetActive(false);
        }
        for (int i = 0; i < PlayerPrefs.GetInt("lives"); i++)
        {
            hearts[i].SetActive(true);
            hearts[i].GetComponent<Animation>().Play("HeartReturn");
            /*if (hearts[i].transform.localScale.x < 1)
            {
                hearts[i].GetComponent<Animation>().Play("HeartReturn");
            }*/
        }
    }

    public void NextLevel()
    {
        adscount++;
        NextLevelClicked?.Invoke();
        ShowLevel();
        ShowHearts();
        levelEnd.SetActive(false);
        buttonNextLevel.SetActive(false);

        StartCoroutine(WaitingForTap());
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

        ShowHearts();
        
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

    public IEnumerator WaitingForTap(bool isContinue=false)
    {
        if (adscount != 0 && adscount % 3 == 0 && Advertisement.IsReady())
        {
            isLocked = true;
            adscount = 0;
            adsController.ShowAdsVideo(AdsController._video, false);
        }

        tapText.SetActive(true);
        StartCoroutine(ChangeAlpha(tapText.GetComponent<Text>()));

        yield return new WaitForSeconds(0.5f);
        while (true)
        {
            if ((Input.GetMouseButtonDown(0) || Input.touchCount > 0))
            {
#if !UNITY_EDITOR
            if (Input.GetTouch(0).phase != TouchPhase.Began)
            {
                    yield return new WaitForSeconds(0.01f);
                    continue;
            }
#endif

                if (isLocked)
                {
                    yield return null;
                    continue;
                }
                if (EventSystem.current.currentSelectedGameObject)
                {
                    if (!EventSystem.current.currentSelectedGameObject.CompareTag("Button"))
                    {
                        StartRunning(isContinue);
                        yield break;
                    }
                }
                else
                {
                    StartRunning(isContinue);
                    yield break;
                }
                
            }
            yield return null;
        }
    }

    private void ShowSpeed(int speed)
    {
        speedText.text = "Speed: " + speed;
    }

    private void OnLevelLoose()
    {
        GameOver();
        StartCoroutine(gameovercoroutine());
    }

    public void MinusHealth()
    {
        hearts[health - 1].GetComponent<Animation>().Play("Heart");
        health -= 1;
    }

    public IEnumerator gameovercoroutine()
    {
        yield return new WaitForSeconds(1f);
        LevelLoose();
    }
    
    public IEnumerator ChangeAlpha(Text obj, float min=0.3f, float max=0.8f, float time=1f)
    {
        Color color = obj.color;
        float timeSpan = time / 100f;
        while (true)
        {
            color.a = min;
            while (color.a <= max)
            {
                color.a += 0.01f;
                obj.color = color;
                yield return new WaitForSeconds(timeSpan);
            }

            while (color.a >= min)
            {
                color.a -= 0.01f;
                obj.color = color;
                yield return new WaitForSeconds(timeSpan);
            }
        }
    }
    
    public void LoadScene(string scene)
    {
        if (GameObject.FindObjectOfType<AdsController>())
        {
            GameObject.FindObjectOfType<AdsController>().DeleteListener();
        }
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
    
}