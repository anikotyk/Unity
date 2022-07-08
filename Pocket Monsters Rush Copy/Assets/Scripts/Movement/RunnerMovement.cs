using UnityEngine;

public class RunnerMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _posMinX;
    [SerializeField] private float _posMaxX;

    [SerializeField] private float _coefTouchDelta = 0.005f;
    [SerializeField] private float _coefTouchDeltaEditor = 0.005f;

    [SerializeField] private bool _isEditor;

    [SerializeField] private FollowPath _followPath;
    
    private Touch _currentTouch;

    private Vector3 _targetPos;
    private float _targetPosX;

    private Vector3 _oldPos;
    private Vector3 _posDelta;
    private Vector3 _forwardMovement;
    private float _rotateAngle;


    private void Awake()
    {
        _targetPos = transform.localPosition;
        if (_isEditor)
        {
            Debug.Log("Editor coef touch");
            _coefTouchDelta = _coefTouchDeltaEditor;
        }
            
    }

    private void LateUpdate()
    {
        GetTargetMovePosition();

        _oldPos = transform.position;
        Move();
        Rotate();
    }

    private void GetTargetMovePosition()
    {
        if (Input.touchCount > 0)
        {
            _currentTouch = Input.GetTouch(0);

            if (_currentTouch.phase == TouchPhase.Moved)
            {
                _targetPosX = _currentTouch.deltaPosition.x * _coefTouchDelta;
                _targetPos = transform.localPosition;
                _targetPos.x += _targetPosX;
                _targetPos.x = Mathf.Clamp(_targetPos.x, _posMinX, _posMaxX);
            }
        }
    }

    private void Move()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, _targetPos, _speed * Time.deltaTime);
    }

    private void Rotate()
    {
        _posDelta = transform.position - _oldPos;
        _forwardMovement = _followPath.GetMovementInFrame();

        _rotateAngle = Vector3.Angle(_forwardMovement, _posDelta + _forwardMovement);
        if (_targetPosX < 0)
        {
            _rotateAngle *= -1;
        }
        transform.localRotation = Quaternion.Euler(0, _rotateAngle * _rotationSpeed * Time.deltaTime, 0);
    }
}
