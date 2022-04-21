using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    private float speed=20f;
    [SerializeField] private float speedHorizontal = 20f;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Transform finish;
    [SerializeField] private GameObject[] brokenBalls;

    private bool isRunning = false;
    private Vector3 posStart;
    private float xPosFinish;
    private Rigidbody rb;
    private Vector2 touchPos;
    private int indexShownBallState;
    private bool isSpeeder;

    public int health;

    [SerializeField] private GameObject fire;

    private GameController gameController;
    

    private void Awake()
    {
        isSpeeder = false;
        gameController = GameObject.FindObjectOfType<GameController>();
        health = PlayerPrefs.GetInt("lives");
        speed = PlayerPrefs.GetInt("speed");
        indexShownBallState = 0;
        xPosFinish = finish.position.x;
        posStart = transform.localPosition;
        rb = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        ResetPlayer();
    }

    private void FixedUpdate()
    {
        if (!isRunning) return;
        if ((direction.x<0 && transform.localPosition.x<xPosFinish) || (direction.x >= 0 && transform.localPosition.x > xPosFinish))
        {
            OnGameOver();
            GameObject.FindObjectOfType<GameController>().LevelComplete();
            return;
        }

#if UNITY_EDITOR
        touchPos = Input.mousePosition;
#elif UNITY_ANDROID
        touchPos = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)touchPos;
#endif
        touchPos -= new Vector2(Screen.width / 2, Screen.height / 2);
        
        touchPos = touchPos.normalized;

        Vector3 forwardMove = direction * speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = new Vector3(0, 0, touchPos.x)* speedHorizontal * Time.fixedDeltaTime;
        rb.velocity = (forwardMove + horizontalMove)*40;

        gameController.SetProgress((posStart.x - transform.localPosition.x) / (posStart.x - xPosFinish));
        // gameController.progresspercentfloat = 100* (posStart.x - transform.localPosition.x) / (posStart.x - xPosFinish);
    }

    public void StartLevel()
    {
        //ResetPlayer();
        Camera.main.GetComponent<CameraController>().player = this.gameObject;
        speed = PlayerPrefs.GetInt("speed");
        isRunning = true;
    }

    public void ResetPlayer()
    {
        Camera.main.GetComponent<CameraController>().player = this.gameObject;
        ResetPlayerAppearance();
        transform.localPosition = posStart;
    }

    public void ResetPlayerAppearance()
    {
        indexShownBallState = 0;
        OnGameStart();
        SetBall(indexShownBallState);
    }

    public void ContinueLevel()
    {
        health = PlayerPrefs.GetInt("lives");
        ResetPlayerAppearance();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        transform.localPosition = new Vector3(transform.localPosition.x, posStart.y, posStart.z);
        
        OnGameStart();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
        // isRunning = true;
    }

    public void OnObstacleCollid()
    {
        if (!isSpeeder)
        {
            health -= 1;
            GameObject.FindObjectOfType<GameController>().MinusHealth();
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

    public void OnGameStart()
    {
        Camera.main.GetComponent<CameraController>().player = this.gameObject;
        foreach (GameObject obj in brokenBalls)
        {
            obj.GetComponent<PartOfBallData>().SetAwakeState();
        }
    }

    public void ShowDieAnim()
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
            speed = PlayerPrefs.GetInt("speed") + 10f;
            fire.SetActive(true);
            gameController.ShowSpeed(speed);
            PlayerPrefs.SetInt("speeder", PlayerPrefs.GetInt("speeder") - 1);
            gameController.speederText.text = "x" + PlayerPrefs.GetInt("speeder");
            StartCoroutine(speederwait(PlayerPrefs.GetInt("speederTime")));
        }
    }

    public IEnumerator speederwait(float time)
    {
        yield return new WaitForSeconds(time);
        DecreaseSpeedAfterSpeeder();
        yield return new WaitForSeconds(2f);

        OnEndSpeeder();
    }

    private void DecreaseSpeedAfterSpeeder()
    {
        speed = PlayerPrefs.GetInt("speed");
        gameController.ShowSpeed(speed);
    }

    private void OnEndSpeeder()
    {
        isSpeeder = false;
        fire.SetActive(false);
    }

    public void MinusHealth()
    {
       
        indexShownBallState += 1;
        SetBall(indexShownBallState);
    }

    public void OnGameOver()
    {
        isRunning = false;
        DecreaseSpeedAfterSpeeder();
        OnEndSpeeder();

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
    }
}
