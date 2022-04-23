using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public int health;
    public int counterGoldWindow;

    public SpawnObstacles spawner;
    
    public GameObject[] hearts;
    
    public GameObject tapText;
    
    public GameObject LoadingCanvas;
    public Text levelNow;
    public Text levelNext;

    public GameObject levelEnd;

    public GameObject buttonContinue;
    public GameObject buttonNextLevel;
    public GameObject buttonRestart;

    public GameObject LevelCompletedContainer;
    public GameObject LevelFailedContainer;
    public Text LevelCompletedText;
    public Text LevelFailedText;

    public GameObject endMoneyContainer;
    public Text endMoneyText;

    public GameObject levelBarContainer;
    public GameObject levelBar;

    public GameObject timer;
   
    public GameObject shopButton;

    public Text speederText;
    public Text speedText;

    public GameObject speederBtn;
    public GameObject adsBtn;
    public GameObject shopBtn;

    public AudioClip win;
    public AudioClip gameover;
    public AudioClip getcoin;
    public AudioClip breakball;

    public GameObject moneyPanel;

    public AudioSource sounds;

    public GameObject panelTooFast;
    public GameObject panelTooFastImage;

    public bool isLocked;

    public int adscount;

    public GameObject buttonRestartSmall;

    private Movement playerNew;

    public GameObject toDestroyIfContinueRunning;
    

    public void Awake()
    {
        playerNew = GameObject.FindObjectOfType<Movement>();
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
        ResetProgressBar();
        ShowHearts();
        counterGoldWindow = 0;
        moneyPanel.SetActive(false);
        speederBtn.SetActive(false);
        speederText.text ="x"+ PlayerPrefs.GetInt("speeder");
        levelEnd.SetActive(false);
    }

    public void Start()
    {
        shopBtn.GetComponent<AnimController>().Show();
        adsBtn.GetComponent<AnimController>().Show();

        StartCoroutine(WaitingForTap());
    }

    public void UpdateSpeed()
    {
        if (PlayerPrefs.GetInt("level") % 5 == 0)
        {
            PlayerPrefs.SetInt("speed", PlayerPrefs.GetInt("speed")+1);
            ShowSpeed(PlayerPrefs.GetInt("speed"));
        }
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
        spawner.ClearObstacles();
        playerNew.ResetPlayer();
        ResetProgressBar();
        ShowHearts();

        levelEnd.SetActive(false);
        buttonNextLevel.SetActive(false);
        shopButton.SetActive(false);

        StartCoroutine(WaitingForTap());
    }

    public void LevelComplete()
    {
        sounds.clip = win;
        sounds.Play();
        GameOver();
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
        UpdateSpeed();
        levelBar.GetComponent<RectTransform>().sizeDelta = new Vector2(levelBarContainer.GetComponent<RectTransform>().rect.width, levelBar.GetComponent<RectTransform>().sizeDelta.y);
        playerNew.ResetPlayer();
        spawner.ClearObstacles();

        ShowEndLevel(true);
    }

    public void AddMoney(int amount = 5)
    {
        counterGoldWindow += 1;
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money")+amount);
    }

    public void LevelLoose()
    {
        sounds.clip = gameover;
        sounds.Play();

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
        playerNew.ContinueLevel();
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
        playerNew.health = health;
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
    
    public void StartRunning(bool isContinue=false)
    {
        if (!isContinue)
        {
            spawner.SpawnNewLevel();
            playerNew.ResetPlayer();
        }
        else
        {
            playerNew.ResetPlayerAppearance();
        }
        
        playerNew.StartLevel();
        playerNew.soundball.Play();
        ShowHearts();
        
        tapText.SetActive(false);

        shopBtn.GetComponent<AnimController>().Hide();
        adsBtn.GetComponent<AnimController>().Hide();
        speederBtn.GetComponent<AnimController>().Show();
    }

    public void GameOver()
    {
        //StopAllCoroutines();
        playerNew.soundball.Stop();
        playerNew.OnGameOver();
        
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
            AdsController.ShowAdsVideo(AdsController._video);
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

    public void ShowSpeed(float speed)
    {
        speedText.text = "Speed: " + Math.Round(speed, 1);
    }

    public void MinusHealth()
    {
        hearts[health - 1].GetComponent<Animation>().Play("Heart");
        health -= 1;
        
        if (health <= 0)
        {
            GameOver();
            StartCoroutine(gameovercoroutine());
        }
        else
        {
            playerNew.MinusHealth();
        }
    }

    public IEnumerator gameovercoroutine()
    {
        DieAnim();
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
    
    public void ResetProgressBar()
    {
        levelBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0, levelBar.GetComponent<RectTransform>().sizeDelta.y);
    }
    
    public void SetProgress(float progress)
    {
        levelBar.GetComponent<RectTransform>().sizeDelta = new Vector2(levelBarContainer.GetComponent<RectTransform>().rect.width * progress, levelBar.GetComponent<RectTransform>().sizeDelta.y);
    }
    
    public void DieAnim()
    {
        sounds.clip=breakball;
        sounds.Play();
        Camera.main.GetComponent<FollowTarget>().target = null;
        playerNew.ShowDieAnim();
    }
}