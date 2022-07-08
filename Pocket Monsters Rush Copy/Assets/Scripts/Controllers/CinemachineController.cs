using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CinemachineController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _normalCamera;
    [SerializeField] private CinemachineVirtualCamera _endCamera;
    [SerializeField] private CinemachineVirtualCamera _startCamera;
    [SerializeField] private CinemachineVirtualCamera _battleCenterCamera;
    [SerializeField] private CinemachineVirtualCamera _battleHeroCamera;
    [SerializeField] private CinemachineVirtualCamera _battlePowerCamera;
    [SerializeField] private CinemachineVirtualCamera _battlePowerForwardCamera;
    [SerializeField] private CinemachineVirtualCamera _dieCamera;

    [SerializeField] private CinemachineVirtualCamera[] _camerasFollowEnemyRigidbody;
    [SerializeField] private CinemachineVirtualCamera[] _camerasFollowHeroBattlePoint;
    [SerializeField] private CinemachineVirtualCamera[] _camerasFollowEnemyPoint;

    private List<CinemachineVirtualCamera> _cameras;

    public static CinemachineController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        CreateCamerasList();
    }
    
    private void OnEnable()
    {
        GameObject.FindObjectOfType<GameController>().GameStart += SetCameraNormal;
        GameObject.FindObjectOfType<GameController>().BattleStart += SetCameraBattleCenter;
        GameObject.FindObjectOfType<BattleController>().HeroWinAction += SetCameraBattleForwardEnd;
        GameObject.FindObjectOfType<Runner>().PreBattleStarted += SetCameraBattleCenter;
        GameObject.FindObjectOfType<LevelController>().LevelLoaded += ResetController;
    }

    private void Start()
    {
        ResetController();
    }


    private void ResetController()
    {
        foreach(CinemachineVirtualCamera cam in _camerasFollowEnemyRigidbody)
        {
            cam.Follow = LevelController.Instance.LevelData.EnemyRigidbody.transform;
            cam.LookAt = LevelController.Instance.LevelData.EnemyRigidbody.transform;
        }

        foreach (CinemachineVirtualCamera cam in _camerasFollowHeroBattlePoint)
        {
            cam.Follow = LevelController.Instance.LevelData.HeroPoint;
            cam.LookAt = LevelController.Instance.LevelData.HeroPoint;
        }

        foreach (CinemachineVirtualCamera cam in _camerasFollowEnemyPoint)
        {
            cam.Follow = LevelController.Instance.LevelData.EnemyPointCam;
            cam.LookAt = LevelController.Instance.LevelData.EnemyPointCam;
        }

        SetCameraStart();
    }

    private void CreateCamerasList()
    {
        _cameras = new List<CinemachineVirtualCamera>();
        _cameras.Add(_normalCamera);
        _cameras.Add(_endCamera);
        _cameras.Add(_startCamera);
        _cameras.Add(_battleCenterCamera);
        _cameras.Add(_battleHeroCamera);
        _cameras.Add(_battlePowerCamera);
        _cameras.Add(_battlePowerForwardCamera);
        _cameras.Add(_dieCamera);
    }

    public void SetCameraEnd()
    {
        TurnOffCameras();

        _endCamera.Priority = 1;
    }

    public void SetCameraNormal()
    {
        TurnOffCameras();

        _normalCamera.Priority = 1;
    }

    public void SetCameraStart()
    {
        TurnOffCameras();
        
        _startCamera.Priority = 1;
    }

    public void SetCameraBattleCenter()
    {
        TurnOffCameras();

        _battleCenterCamera.Priority = 1;
    }

    public void SetCameraBattleHero()
    {
        TurnOffCameras();

        _battleHeroCamera.Priority = 1;
    }

    public void SetCameraBattlePower()
    {
        TurnOffCameras();

        _battlePowerCamera.Priority = 1;
    }

    public void SetCameraBattleForwardEnd()
    {
        TurnOffCameras();

        _battlePowerForwardCamera.Priority = 1;
    }

    public void SetCameraDie()
    {
        TurnOffCameras();

        _dieCamera.Priority = 1;
    }

    private void TurnOffCameras()
    {
        foreach (CinemachineVirtualCamera cam in _cameras)
        {
            cam.Priority = 0;
        }
    }
}
