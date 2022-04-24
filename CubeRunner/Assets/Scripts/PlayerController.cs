using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject[] brokenBalls;
    [SerializeField] private AudioSource soundball;
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject levelBarContainer;
    [SerializeField] private GameObject levelBar;
    
    private int health;
    private bool isSpeeder;

    private int indexShownBallState;

    public event UnityAction LevelLoose;
    public event UnityAction LevelComplete;
    public event UnityAction GetDamage;
    public event UnityAction SpeederCountChanged;
    public event UnityAction<int> SpeedChanged;

    public static PlayerController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        
        isSpeeder = false;
        health = PlayerPrefs.GetInt("lives");
        indexShownBallState = 0;
        
        ResetProgressBar();
        GetAllPartsOfBallAwakeState();
        fire.GetComponent<FollowTarget>().SetTarget(this.gameObject);

        ResetPlayer();
    }

    private void Start()
    {
        GameController.Instance.LevelStarted += StartRunning;
        GameController.Instance.LevelContinued += StartRunning;
        GameController.Instance.LevelEnded += OnLevelEnded;
        AdsController.Instance.ContinueButtonClicked += ContinueLevel;
        GameController.Instance.NextLevelClicked += OnNextLevelClicked;
    }

    private void OnDestroy()
    {
        GameController.Instance.LevelStarted -= StartRunning;
        GameController.Instance.LevelContinued -= StartRunning;
        GameController.Instance.LevelEnded -= OnLevelEnded;
        AdsController.Instance.ContinueButtonClicked -= ContinueLevel;
        GameController.Instance.NextLevelClicked -= OnNextLevelClicked;
    }

    private void StartRunning()
    {
        health = PlayerPrefs.GetInt("lives");
        Camera.main.GetComponent<FollowTarget>().SetTarget(this.gameObject);
        soundball.Play();
    }
    
    public void SetProgress(float progress)
    {
        levelBar.GetComponent<RectTransform>().sizeDelta = new Vector2(levelBarContainer.GetComponent<RectTransform>().rect.width * progress, levelBar.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void ResetProgressBar()
    {
        levelBar.GetComponent<RectTransform>().sizeDelta = new Vector2(0, levelBar.GetComponent<RectTransform>().sizeDelta.y);
    }

    private void OnLevelEnded()
    {
        ResetProgressBar();
        ResetPlayer();
    }

    public void OnLevelComplete()
    {
        LevelComplete?.Invoke();
        OnGameOver();
        SetProgress(1f);
    }

    private void OnNextLevelClicked()
    {
        ResetProgressBar();
    }

    private void ResetPlayer()
    {
        ResetPlayerAppearance();
        //transform.localPosition = posStart;
    }

    private void ResetPlayerAppearance()
    {
        Camera.main.GetComponent<FollowTarget>().SetTarget(this.gameObject);
        indexShownBallState = 0;
        SetAllPartsOfBallToAwakeState();
        SetBall(indexShownBallState);
    }

    private void ContinueLevel()
    {
        health = PlayerPrefs.GetInt("lives");
        ResetPlayerAppearance();
        //transform.localPosition = new Vector3(transform.localPosition.x, posStart.y, posStart.z);
    }

    public void OnObstacleCollid()
    {
        if (!isSpeeder)
        {
            health -= 1;
            GetDamage?.Invoke();
            indexShownBallState += 1;
            SetBall(indexShownBallState);
            if (health <= 0)
            {
                LevelLoose?.Invoke();
                OnGameOver();
                Camera.main.GetComponent<FollowTarget>().RemoveTarget();
                ShowDieAnim();
            }
        }
    }

    private void SetBall(int index)
    {
        foreach (GameObject obj in brokenBalls)
        {
            obj.SetActive(false);
        }
        brokenBalls[index].SetActive(true);
    }

    private void SetAllPartsOfBallToAwakeState()
    {
        foreach (GameObject obj in brokenBalls)
        {
            obj.GetComponent<PartOfBallData>().SetAwakeState();
        }
    }

    private void GetAllPartsOfBallAwakeState()
    {
        foreach (GameObject obj in brokenBalls)
        {
            obj.GetComponent<PartOfBallData>().GetAwakeState();
        }
    }

    private void ShowDieAnim()
    {
        if (indexShownBallState == 0)
        {
            indexShownBallState = 1;
            SetBall(indexShownBallState);
        }
        
        brokenBalls[indexShownBallState].GetComponent<PartOfBallData>().SetDieState();
    }


    public void OnSpeeder()
    {
        if (PlayerPrefs.GetInt("speeder") > 0 && !isSpeeder)
        {
            isSpeeder = true;
            fire.SetActive(true);
            SpeedChanged?.Invoke(PlayerPrefs.GetInt("speed") + 10);
            PlayerPrefs.SetInt("speeder", PlayerPrefs.GetInt("speeder") - 1);
            SpeederCountChanged?.Invoke();
            StartCoroutine(speederwait(PlayerPrefs.GetInt("speederTime")));
        }
    }

    private IEnumerator speederwait(float time)
    {
        yield return new WaitForSeconds(time);
        DecreaseSpeedAfterSpeeder();
        yield return new WaitForSeconds(2f);

        OnEndSpeeder();
    }

    private void DecreaseSpeedAfterSpeeder()
    {
        SpeedChanged?.Invoke(PlayerPrefs.GetInt("speed"));
    }

    private void OnEndSpeeder()
    {
        isSpeeder = false;
        fire.SetActive(false);
    }

    private void OnGameOver()
    {
        soundball.Stop();
        DecreaseSpeedAfterSpeeder();
        OnEndSpeeder();
    }

    public bool CheckIsDead()
    {
        return health <= 0;
    }
}
