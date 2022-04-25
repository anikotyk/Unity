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
    
    private bool isSpeeder;

    private int indexShownBallState;
    
    public event UnityAction LevelComplete;
    public event UnityAction GetDamage;
    public event UnityAction SpeederCountChanged;

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
        HealthController.Instance.LevelLoose += OnLevelLoose;
    }

    private void OnDestroy()
    {
        GameController.Instance.LevelStarted -= StartRunning;
        GameController.Instance.LevelContinued -= StartRunning;
        GameController.Instance.LevelEnded -= OnLevelEnded;
        AdsController.Instance.ContinueButtonClicked -= ContinueLevel;
        GameController.Instance.NextLevelClicked -= OnNextLevelClicked;
        HealthController.Instance.LevelLoose -= OnLevelLoose;
    }

    private void StartRunning()
    {
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
        ResetPlayerAppearance();
    }

    public void OnObstacleCollid()
    {
        if (!isSpeeder)
        {
            indexShownBallState += 1;
            SetBall(indexShownBallState);
            GetDamage?.Invoke();
        }
    }

    private void OnLevelLoose()
    {
        OnGameOver();
        Camera.main.GetComponent<FollowTarget>().RemoveTarget();
        ShowDieAnim();
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
            SpeedController.Instance.ChangeCurrentSpeed(SpeedController.Instance.GetPlayerPrefsSpeed() + 10);
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
        SpeedController.Instance.ChangeCurrentSpeed(SpeedController.Instance.GetPlayerPrefsSpeed());
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
}
