using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speedHorizontal = 20f;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Transform finish;

    private int speed;
    private bool isRunning;

    private Vector3 posStart;
    private float xPosFinish;
    private Rigidbody rb;
    private Vector2 touchPos;


    private Vector3 forwardMove;
    private Vector3 horizontalMove;

    private void Awake()
    {
        isRunning = false;
        speed = PlayerPrefs.GetInt("speed");
        xPosFinish = finish.position.x;
        posStart = transform.localPosition;
        rb = GetComponent<Rigidbody>();

        ResetPlayerPositionAtStart();
    }

    private void Start()
    {
        GameController.Instance.LevelStarted += OnStartMoving;
        GameController.Instance.LevelContinued += OnStartMoving;
        PlayerController.Instance.SpeedChanged += UpdateSpeed;
        PlayerController.Instance.LevelComplete += StopRunning;
        PlayerController.Instance.LevelLoose += StopRunning;
        AdsController.Instance.ContinueButtonClicked += ResetPlayerPositionAtContinue;
        GameController.Instance.LevelEnded += ResetPlayerPositionAtStart;
    }

    private void OnDestroy()
    {
        GameController.Instance.LevelStarted -= OnStartMoving;
        GameController.Instance.LevelContinued -= OnStartMoving;
        PlayerController.Instance.SpeedChanged -= UpdateSpeed;
        PlayerController.Instance.LevelComplete -= StopRunning;
        PlayerController.Instance.LevelLoose -= StopRunning;
        AdsController.Instance.ContinueButtonClicked -= ResetPlayerPositionAtContinue;
        GameController.Instance.LevelEnded -= ResetPlayerPositionAtStart;
    }

    private void UpdateSpeed(int newSpeed)
    {
        speed = newSpeed;
    }

    private void StopRunning()
    {
        isRunning = false;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();

        rb.isKinematic = true;
        rb.useGravity = false;
    }

    private void OnStartMoving()
    {
        touchPos = Vector2.zero;
        speed = PlayerPrefs.GetInt("speed");
        rb.isKinematic = false;
        rb.useGravity = true;
        isRunning = true;
    }

    public void ResetPlayerPositionAtStart()
    {
        transform.localPosition = posStart;
    }

    public void ResetPlayerPositionAtContinue()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, posStart.y, posStart.z);
    }

    private void FixedUpdate()
    {
        if (!isRunning) return;

        if ((direction.x < 0 && transform.localPosition.x < xPosFinish) || (direction.x >= 0 && transform.localPosition.x > xPosFinish))
        {
            PlayerController.Instance.OnLevelComplete();
            
            return;
        }

        if (Input.touchCount > 0)
        {
            touchPos = Input.GetTouch(0).position;
            touchPos -= new Vector2(Screen.width / 2, Screen.height / 2);
            touchPos = touchPos.normalized;
        }

        forwardMove = direction * speed * Time.fixedDeltaTime;
        horizontalMove = new Vector3(0, 0, touchPos.x) * speedHorizontal * Time.fixedDeltaTime;
        rb.velocity = (forwardMove + horizontalMove) * 40;

        PlayerController.Instance.SetProgress((posStart.x - transform.localPosition.x) / (posStart.x - xPosFinish));
    }
}
