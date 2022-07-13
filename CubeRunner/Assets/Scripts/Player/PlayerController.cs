using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private GameObject[] _brokenBalls;
    [SerializeField] private AudioSource _ballAudioSource;
    [SerializeField] private GameObject _fireParticleOnSpeeder;
    [SerializeField] private int _speederIncreaseSpeedAmount = 10;


    private bool _isSpeeder;
    private int _indexShownBrokenBall;
    
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
        
        _isSpeeder = false;
        _indexShownBrokenBall = 0;
        
        SaveAllAwakePosRotOfBallParts();
        _fireParticleOnSpeeder.GetComponent<FollowTarget>().SetTarget(this.gameObject);

        ResetPlayerAppearance();
    }

    private void Start()
    {
        GameController.Instance.LevelStarted += StartRunning;
        GameController.Instance.LevelContinued += StartRunning;
        GameController.Instance.LevelEnded += OnLevelEnded;
        GameObject.FindObjectOfType<ContinueButton>().ContinueButtonClicked += ContinueLevel;
        HealthController.Instance.LevelLose += OnLevelLose;
    }

    private void OnDestroy()
    {
        GameController.Instance.LevelStarted -= StartRunning;
        GameController.Instance.LevelContinued -= StartRunning;
        GameController.Instance.LevelEnded -= OnLevelEnded;
        //GameObject.FindObjectOfType<ContinueButton>().ContinueButtonClicked -= ContinueLevel;
        HealthController.Instance.LevelLose -= OnLevelLose;
    }

    private void StartRunning()
    {
        Camera.main.GetComponent<FollowTarget>().SetTarget(this.gameObject);
        _ballAudioSource.Play();
    }
    
    private void OnLevelEnded()
    {
        ResetPlayerAppearance();
    }

    public void OnLevelComplete()
    {
        LevelComplete?.Invoke();
        OnGameOver();
    }

    private void ResetPlayerAppearance()
    {
        Camera.main.GetComponent<FollowTarget>().SetTarget(this.gameObject);
        _indexShownBrokenBall = 0;
        ReturnAllBallPartsToAwakePosRot();
        ShowBrokenBall(_indexShownBrokenBall);
    }

    private void ContinueLevel()
    {
        ResetPlayerAppearance();
    }

    public void OnObstacleCollid()
    {
        if (!_isSpeeder)
        {
            _indexShownBrokenBall += 1;
            ShowBrokenBall(_indexShownBrokenBall);
            GetDamage?.Invoke();
        }
    }

    private void OnLevelLose()
    {
        OnGameOver();
        Camera.main.GetComponent<FollowTarget>().RemoveTarget();
        ShowDie();
    }

    private void ShowBrokenBall(int index)
    {
        foreach (GameObject obj in _brokenBalls)
        {
            obj.SetActive(false);
        }
        _brokenBalls[index].SetActive(true);
    }

    private void ReturnAllBallPartsToAwakePosRot()
    {
        foreach (GameObject obj in _brokenBalls)
        {
            obj.GetComponent<PartOfBallData>().ReturnBallPartsToAwakePosRot();
        }
    }

    private void SaveAllAwakePosRotOfBallParts()
    {
        foreach (GameObject obj in _brokenBalls)
        {
            obj.GetComponent<PartOfBallData>().SaveAwakePosRotOfBallParts();
        }
    }

    private void ShowDie()
    {
        if (_indexShownBrokenBall == 0)
        {
            _indexShownBrokenBall = 1;
            ShowBrokenBall(_indexShownBrokenBall);
        }
        
        _brokenBalls[_indexShownBrokenBall].GetComponent<PartOfBallData>().TurnOnDiePhysicsOfBallParts();
    }
    
    public void UseSpeeder()
    {
        if (PlayerPrefs.GetInt("speeder") > 0 && !_isSpeeder)
        {
            _isSpeeder = true;
            _fireParticleOnSpeeder.SetActive(true);
            SpeedController.Instance.IncreaseCurrentSpeed(_speederIncreaseSpeedAmount);
            PlayerPrefs.SetInt("speeder", PlayerPrefs.GetInt("speeder") - 1);
            SpeederCountChanged?.Invoke();
            StartCoroutine(OnEndSpeederCoroutine(PlayerPrefs.GetInt("speederTime")));
        }
    }

    private IEnumerator OnEndSpeederCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        DecreaseSpeedAfterSpeeder();
        yield return new WaitForSeconds(2f);

        OnEndSpeeder();
    }

    private void DecreaseSpeedAfterSpeeder()
    {
        SpeedController.Instance.ResetCurrentSpeed();
    }

    private void OnEndSpeeder()
    {
        _isSpeeder = false;
        _fireParticleOnSpeeder.SetActive(false);
    }

    private void OnGameOver()
    {
        _ballAudioSource.Stop();
        DecreaseSpeedAfterSpeeder();
        OnEndSpeeder();
    }
}
