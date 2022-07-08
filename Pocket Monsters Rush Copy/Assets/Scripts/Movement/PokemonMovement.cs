using UnityEngine;

public class PokemonMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _rotationSpeed;

    [SerializeField] private float _posMinX;
    [SerializeField] private float _posMaxX;
    
    [SerializeField] private FollowPath _followPath;

    private Transform _followTarget;
    [SerializeField] private Vector3 _targetPosOffset;
    
    private Vector3 _newPos;
    private float _newPosX;

    private Vector3 _oldPos;
    private Vector3 _posDelta;
    private Vector3 _forwardMovement;
    private float _rotateAngle;

    private void Awake()
    {
        _followTarget = GameObject.FindObjectOfType<RunnerMovement>().transform;
        _newPos = transform.localPosition;
    }
    

    private void FixedUpdate()
    {
        GetTargetMovePosition();

        _oldPos = transform.position;
        Move();
        Rotate();
    }

    private void GetTargetMovePosition()
    {
        _newPosX = _followTarget.localPosition.x + _targetPosOffset.x;
        _newPos = transform.localPosition;
        _newPos.x = Mathf.Clamp(_newPosX, _posMinX, _posMaxX);
    }

    private void Move()
    {
        transform.localPosition = Vector3.Lerp(transform.localPosition, _newPos, _speed * Time.deltaTime);
    }

    private void Rotate()
    {
        _posDelta = transform.position - _oldPos;
        _forwardMovement = _followPath.GetMovementInFrame();

        _rotateAngle = Vector3.Angle(_forwardMovement, _posDelta + _forwardMovement);
        if (_newPosX < 0)
        {
            _rotateAngle *= -1;
        }

        transform.localRotation = Quaternion.Euler(0, _rotateAngle * _rotationSpeed * Time.deltaTime, 0);
    }
}
