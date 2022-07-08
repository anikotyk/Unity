using PathCreation;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    private PathCreator _pathCreator;
    [SerializeField] private EndOfPathInstruction _endOfPathInstruction;
    [SerializeField] private bool _rotationIgnoreXZ;

    private Transform _startPoint;

    [SerializeField] private float _speed;
    
    private float _distanceTravelled;
    private Quaternion _newRotation;
    
    private bool _isMoving = false;
    
    private void OnEnable()
    {
        GameObject.FindObjectOfType<GameController>().GameStart += StartMove;
        GameObject.FindObjectOfType<Runner>().PreBattleStarted += StopMove;
        GameObject.FindObjectOfType<LevelController>().LevelLoaded += ResetFollowPath;
    }


    private void Start()
    {
        ResetFollowPath();
    }

    private void ResetFollowPath()
    {
        
        _startPoint = LevelController.Instance.LevelData.StartPoint;
        _pathCreator = LevelController.Instance.LevelData.PathCreator;
        
        ResetPosition();
    }
    
    public void ResetPosition()
    {
        transform.position = _startPoint.position;
        if (_pathCreator != null)
        {
            transform.position = _pathCreator.path.GetClosestPointOnPath(transform.position);
            _distanceTravelled = _pathCreator.path.GetClosestDistanceAlongPath(transform.position);
            transform.rotation = GetRotation();
        }
        else
        {
            transform.rotation = _startPoint.rotation;
        }
    }

    

    public void StartMove()
    {
        _isMoving = true;
    }

    public void StopMove()
    {
        _isMoving = false;
    }

    public Vector3 GetMovementInFrame()
    {
        return transform.forward * _speed *Time.deltaTime;
    }

    private void Update()
    {
        if (!_isMoving) { return; }
        if (_pathCreator != null)
        {
            _distanceTravelled += _speed * Time.deltaTime;
           
            transform.position = _pathCreator.path.GetPointAtDistance(_distanceTravelled, _endOfPathInstruction);
            transform.rotation = GetRotation();
        }
    }
    

    private Quaternion GetRotation()
    {
        if (_rotationIgnoreXZ)
        {
            _newRotation = _pathCreator.path.GetRotationAtDistance(_distanceTravelled, _endOfPathInstruction);
            _newRotation.x = 0;
            _newRotation.z = 0;

            return _newRotation;
        }
        else
        {
            return _pathCreator.path.GetRotationAtDistance(_distanceTravelled, _endOfPathInstruction);
        }
    }
}
