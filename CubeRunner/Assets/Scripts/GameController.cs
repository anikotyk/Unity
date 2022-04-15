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
    public GameObject AudioController;
    public PlayerController player;
   
    public float speedMoveObstacles;
    public int health;
    public int counterGoldWindow;

    public SpawnObstacles spawner;
    
    public GameObject[] hearts;
    
    public GameObject tapText;
    
    public GameObject LoadingCanvas;
    public Text levelNow;
    public Text levelNext;

    public int lengthLevel;

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

    public float progresspercentfloat;
    public float percentsnow;

    public GameObject levelBarContainer;
    public GameObject levelBar;

    public GameObject timer;
    public bool isLevelEnded;

    public GameObject diePrefab;

    public GameObject destroyedPlayer;

    public Material[] crackedMaterials;

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

    public void Awake()
    {
        //PlayerPrefs.SetInt("isFirstTime", 12);
        if (PlayerPrefs.GetInt("isFirstTime") != 18)
        {
            Debug.Log("FIRST TIME");
            PlayerPrefs.SetInt("money", 100);
            PlayerPrefs.SetInt("isFirstTime", 18);
            PlayerPrefs.SetInt("speeder", 3);
            PlayerPrefs.SetInt("speederTime", 2);
            PlayerPrefs.SetInt("speederTimeCount", 1);
            PlayerPrefs.SetInt("speed", 20);
            PlayerPrefs.SetInt("lives", 1);
            PlayerPrefs.SetInt("level", 1);
        }
        adscount = 0;
        isLocked = false;
        ShowLevel();
        ShowSpeed(PlayerPrefs.GetInt("speed"));
        ResetProgressBar();
        ShowHearts();
        health = PlayerPrefs.GetInt("lives");
        lengthLevel = 20;
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
        Destroy(destroyedPlayer);
        player.gameObject.SetActive(true);
        player.StartCoroutine(player.MoveToStart());
        spawner.finishexists = false;
        adscount++;
        ClearObstacles();
        ResetProgressBar();
        ShowHearts();
        spawner.counterObstacles = 0;

        levelEnd.SetActive(false);
        buttonNextLevel.SetActive(false);
        shopButton.SetActive(false);

        StartCoroutine(WaitingForTap());
    }

    public void LevelComplete()
    {
        if(!isLevelEnded){
            spawner.finishexists = false;
            sounds.clip = win;
            sounds.Play();
            isLevelEnded = true;
            GameOver();
            player.StartCoroutine(player.MoveToStart());
            PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level") + 1);
            UpdateSpeed();
            levelBar.GetComponent<RectTransform>().sizeDelta = new Vector2(levelBarContainer.GetComponent<RectTransform>().rect.width, levelBar.GetComponent<RectTransform>().sizeDelta.y);
            spawner.counterObstacles = 0;
            ClearObstacles();

            ShowEndLevel(true);
        }
    }

    public void AddMoney(int amount = 5)
    {
        PlayerPrefs.SetInt("money", PlayerPrefs.GetInt("money")+amount);
    }

    public void NextLevel()
    {
        adscount++;
        ResetProgressBar();
        ShowLevel();
        ShowHearts();
        levelEnd.SetActive(false);
        buttonNextLevel.SetActive(false);

        StartCoroutine(WaitingForTap());
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

    public void LevelLooseToFast()
    {
        ShowEndLevel(false);
    }

    public void ContinuePlaying()
    {
        levelEnd.SetActive(false);
        Destroy(destroyedPlayer);
        player.gameObject.SetActive(true);
        player.StartCoroutine(player.MoveToStart());
        buttonContinue.SetActive(false);
        
        StartCoroutine(WaitingForTap());
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
            if (hearts[i].transform.localScale.x < 1)
            {
                hearts[i].GetComponent<Animation>().Play("HeartReturn");
            }
        }
    }

    public void ClearObstacles()
    {
        spawner.DeleteAllObstacles();
    }

    public void StartRunning()
    {
        isLevelEnded = false;
        speedMoveObstacles = PlayerPrefs.GetInt("speed");
        player.OnGameStart();
        player.indexShownBallState = 0;
        player.SetBall(0);
        player.soundball.Play();
        ShowHearts();
        player.isSpeeder = false;
        spawner.isSpawn = true;
        player.indexShownBallState = 0;


        tapText.SetActive(false);

        shopBtn.GetComponent<AnimController>().Hide();
        adsBtn.GetComponent<AnimController>().Hide();
        speederBtn.GetComponent<AnimController>().Show();
        
        player.StopAllCoroutines();
        player.isPlaying = true;
        
        player.StartCoroutine(player.MovingPlayer());

        spawner.StartAllObstacles();
        spawner.spawner = spawner.StartCoroutine(spawner.Spawner());
        StartCoroutine(ProgressManager(levelBar, levelBarContainer));
    }

    public void GameOver()
    {
        player.isPlaying = false;
        StopAllCoroutines();

        player.StopAllCoroutines();
        player.myRb.velocity = Vector3.zero;
        player.myRb.angularVelocity = Vector3.zero;
        player.soundball.Stop();
        player.isSpeeder = false;
        player.fire.SetActive(false);

        speedMoveObstacles = PlayerPrefs.GetInt("speed");
        ShowSpeed(speedMoveObstacles);

        //player.SetBall(0);
        Material[] mats = new Material[2];
        mats[0] = player.gameObject.GetComponent<MeshRenderer>().materials[0];
        mats[1] = crackedMaterials[4];
        player.gameObject.GetComponent<MeshRenderer>().materials = mats;
        
        GameObject.FindObjectOfType<SpawnObstacles>().StopAllCoroutines();
        GameObject.FindObjectOfType<SpawnObstacles>().StopAllObstacles();
        
        shopBtn.GetComponent<AnimController>().Show();
        adsBtn.GetComponent<AnimController>().Show();
        speederBtn.GetComponent<AnimController>().Hide();
    }

    public IEnumerator WaitingForTap()
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
                        StartRunning();
                        yield break;
                    }
                }
                else
                {
                    StartRunning();
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
            player.indexShownBallState += 1;
            player.SetBall(player.indexShownBallState);
            Material[] mats = new Material[2];
            mats[0] = player.gameObject.GetComponent<MeshRenderer>().materials[0];
            mats[1] = crackedMaterials[health - 1];
            player.gameObject.GetComponent<MeshRenderer>().materials = mats;
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
        progresspercentfloat = 0;
        percentsnow = 0;
        levelBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0, levelBar.GetComponent<RectTransform>().sizeDelta.y);
    }

    public IEnumerator ProgressManager(GameObject progressbar, GameObject parentprogressbar)
    {
        float newpercents = 0;

        while (true)
        {
            if (progresspercentfloat > percentsnow)
            {
                newpercents = progresspercentfloat;

                while (percentsnow < newpercents)
                {
                    percentsnow += 1;
                    yield return ProgressBarFillCoroutine(progressbar, parentprogressbar, percentsnow / 100f);
                }
                if (newpercents >= 100)
                {
                    yield break;
                }
            }

            yield return new WaitForSeconds(1f);
        }
    }

    public IEnumerator ProgressBarFillCoroutine(GameObject progressbar, GameObject parentprogressbar, float progress)
    {
        float width = parentprogressbar.GetComponent<RectTransform>().rect.width * progress;
        float widthdif = width - progressbar.GetComponent<RectTransform>().rect.width;

        while (progressbar.GetComponent<RectTransform>().rect.width < width)
        {
            progressbar.GetComponent<RectTransform>().sizeDelta = new Vector2(progressbar.GetComponent<RectTransform>().sizeDelta.x + 1, progressbar.GetComponent<RectTransform>().sizeDelta.y);

            yield return new WaitForSeconds(0.01f);
        }
        progressbar.GetComponent<RectTransform>().sizeDelta = new Vector2(width, progressbar.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void DieAnim()
    {
        sounds.clip=breakball;
        sounds.Play();
        player.ShowDieAnim();
        //player.gameObject.SetActive(false);
       // destroyedPlayer = Instantiate(diePrefab) as GameObject;
       // destroyedPlayer.transform.localPosition = new Vector3(destroyedPlayer.transform.localPosition.x, destroyedPlayer.transform.localPosition.y, player.transform.localPosition.z);
    }

}




