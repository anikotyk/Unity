using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    private int speed;
    [SerializeField] private float speedHorizontal = 20f;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Transform finish;
    [SerializeField] private GameObject[] brokenBalls;
    [SerializeField] private AudioSource soundball;
    [SerializeField] private GameObject fire;
    [SerializeField] private GameObject levelBarContainer;
    [SerializeField] private GameObject levelBar;
    
    private bool isRunning;
    private Vector3 posStart;
    private float xPosFinish;
    private Rigidbody rb;
    private Vector2 touchPos;
    private int indexShownBallState;
    private bool isSpeeder;

    private int health;

    private GameController gameController;
    private AdsController adsController;

    private Vector3 forwardMove;
    private Vector3 horizontalMove;

    public event UnityAction LevelLoose;
    public event UnityAction LevelComplete;
    public event UnityAction GetDamage;
    public event UnityAction SpeederCountChanged;
    public event UnityAction<int> SpeedChanged;

    private void Awake()
    {
        isRunning = false;
        isSpeeder = false;
        gameController = GameObject.FindObjectOfType<GameController>();
        adsController = GameObject.FindObjectOfType<AdsController>();
        health = PlayerPrefs.GetInt("lives");
        speed = PlayerPrefs.GetInt("speed");
        indexShownBallState = 0;
        xPosFinish = finish.position.x;
        posStart = transform.localPosition;
        rb = GetComponent<Rigidbody>();
        ResetProgressBar();
        GetAllPartsOfBallAwakeState();
        fire.GetComponent<FollowTarget>().SetTarget(this.gameObject);

        ResetPlayer();
    }

    private void OnEnable()
    {
        gameController.LevelStarted += StartRunning;
        gameController.LevelContinued += StartRunning;
        gameController.LevelEnded += OnLevelEnded;
        adsController.ContinueButtonClicked += ContinueLevel;
        gameController.NextLevelClicked += OnNextLevelClicked;
    }

    private void OnDisable()
    {
        gameController.LevelStarted -= StartRunning;
        gameController.LevelContinued -= StartRunning;
        gameController.LevelEnded -= OnLevelEnded;
        adsController.ContinueButtonClicked -= ContinueLevel;
        gameController.NextLevelClicked -= OnNextLevelClicked;
    }

    private void StartRunning()
    {
        health = PlayerPrefs.GetInt("lives");
        touchPos = Vector2.zero;
        Camera.main.GetComponent<FollowTarget>().SetTarget(this.gameObject);
        speed = PlayerPrefs.GetInt("speed");
        rb.isKinematic = false;
        rb.useGravity = true;
        soundball.Play();
        isRunning = true;
    }

    private void FixedUpdate()
    {
        if (!isRunning) return;

        if ((direction.x<0 && transform.localPosition.x<xPosFinish) || (direction.x >= 0 && transform.localPosition.x > xPosFinish))
        {
            LevelComplete?.Invoke();
            OnGameOver();
            SetProgress(1f);
            return;
        }

        if (Input.touchCount > 0)
        {
            touchPos = Input.GetTouch(0).position;
            touchPos -= new Vector2(Screen.width / 2, Screen.height / 2);
            touchPos = touchPos.normalized;
        }
        
        forwardMove = direction * speed * Time.fixedDeltaTime;
        horizontalMove = new Vector3(0, 0, touchPos.x)* speedHorizontal * Time.fixedDeltaTime;
        rb.velocity = (forwardMove + horizontalMove)*40;

        SetProgress((posStart.x - transform.localPosition.x) / (posStart.x - xPosFinish));
    }

    private void SetProgress(float progress)
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

    private void OnNextLevelClicked()
    {
        ResetProgressBar();
    }

    private void ResetPlayer()
    {
        ResetPlayerAppearance();
        transform.localPosition = posStart;
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
        
        transform.localPosition = new Vector3(transform.localPosition.x, posStart.y, posStart.z);
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

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        brokenBalls[indexShownBallState].GetComponent<PartOfBallData>().SetDieState();
    }


    public void OnSpeeder()
    {
        if (PlayerPrefs.GetInt("speeder") > 0 && !isSpeeder)
        {
            isSpeeder = true;
            speed = PlayerPrefs.GetInt("speed") + 10;
            fire.SetActive(true);
            SpeedChanged?.Invoke(speed);
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
        speed = PlayerPrefs.GetInt("speed");
        SpeedChanged?.Invoke(speed);
    }

    private void OnEndSpeeder()
    {
        isSpeeder = false;
        fire.SetActive(false);
    }

    private void OnGameOver()
    {
        isRunning = false;
        soundball.Stop();
        DecreaseSpeedAfterSpeeder();
        OnEndSpeeder();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    public bool CheckIsDead()
    {
        return health <= 0;
    }
}
