using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _speedHorizontal = 20f;
    [SerializeField] private float _velocityCoeff = 40f;
    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] private Transform _finish;
    
    private bool _isRunning;
    private Vector3 _posStart;
    private float _xPosFinish;
    private Rigidbody _rb;
    private Vector2 _touchPos;
    
    private Vector3 _forwardMove;
    private Vector3 _horizontalMove;

    private void Awake()
    {
        _isRunning = false;
        _xPosFinish = _finish.position.x;
        _posStart = transform.localPosition;
        _rb = GetComponent<Rigidbody>();

        ResetPlayerPositionAtStart();
    }

    private void Start()
    {
        GameController.Instance.LevelStarted += OnStartMoving;
        GameController.Instance.LevelContinued += OnStartMoving;
        PlayerController.Instance.LevelComplete += StopRunning;
        HealthController.Instance.LevelLose += StopRunning;
        AdsController.Instance.ContinueButtonClicked += ResetPlayerPositionAtContinue;
        GameController.Instance.LevelEnded += ResetPlayerPositionAtStart;
    }

    private void OnDestroy()
    {
        GameController.Instance.LevelStarted -= OnStartMoving;
        GameController.Instance.LevelContinued -= OnStartMoving;
        PlayerController.Instance.LevelComplete -= StopRunning;
        HealthController.Instance.LevelLose -= StopRunning;
        AdsController.Instance.ContinueButtonClicked -= ResetPlayerPositionAtContinue;
        GameController.Instance.LevelEnded -= ResetPlayerPositionAtStart;
    }

    private void StopRunning()
    {
        _isRunning = false;
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
        _rb.Sleep();

        _rb.isKinematic = true;
        _rb.useGravity = false;
    }

    private void OnStartMoving()
    {
        _touchPos = Vector2.zero;
        _rb.isKinematic = false;
        _rb.useGravity = true;
        _isRunning = true;
    }

    public void ResetPlayerPositionAtStart()
    {
        transform.localPosition = _posStart;
    }

    public void ResetPlayerPositionAtContinue()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, _posStart.y, _posStart.z);
    }

    private void FixedUpdate()
    {
        if (!_isRunning) return;

        if (IsFinishCrossed())
        {
            PlayerController.Instance.OnLevelComplete();
            
            return;
        }

        if (Input.touchCount > 0)
        {
            _touchPos = Input.GetTouch(0).position;
            _touchPos -= new Vector2(Screen.width / 2, Screen.height / 2);
            _touchPos = _touchPos.normalized;
        }

        _forwardMove = _moveDirection * SpeedController.Instance.CurrentSpeed * Time.fixedDeltaTime;
        _horizontalMove = new Vector3(0, 0, _touchPos.x) * _speedHorizontal * Time.fixedDeltaTime;
        _rb.velocity = (_forwardMove + _horizontalMove) * _velocityCoeff;

        GameController.Instance.SetProgress((_posStart.x - transform.localPosition.x) / (_posStart.x - _xPosFinish));
    }

    private bool IsFinishCrossed()
    {
        return (_moveDirection.x < 0 && transform.localPosition.x < _xPosFinish) || (_moveDirection.x >= 0 && transform.localPosition.x > _xPosFinish);
    }
}
