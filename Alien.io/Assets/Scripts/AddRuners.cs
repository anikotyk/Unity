using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddRuners : MonoBehaviour
{
    [Range(0f, 1f)] [SerializeField] private float _radiusFactor;
    [Range(0f, 1f)] [SerializeField] private float _angleFactor;

    public bool isPointedNow = false;
    public string nameInTop;
    public bool IsHuman;

    private Color _colorSquad;
    public Color ColorSquad=> _colorSquad;
    
    private Spawner _spawner;
    
    private void Awake()
    {
        isPointedNow = false;
        _spawner = GameObject.FindObjectOfType<Spawner>();
    }

    private void OnEnable()
    {
        OnGameStart();
    }

    public void StopStartMoving(bool isStop)
    {
        if (GetComponent<Movement>())
        {
            GetComponent<Movement>().isStopped = isStop;
        }
        else
        {
            GetComponent<MovementNPC>().isStopped = isStop;
        }
        SetIsRun(!isStop);
    }
    
    public void OnGameStart()
    {
        _colorSquad = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);

        if (GetComponent<Movement>() != null)
        {
            IsHuman = (Random.value > 0.5f);
            int count = Random.Range(1, 3);
            AddRunners(count);
            GameObject.FindObjectOfType<CameraFollow>().coefHowNeadCam = 15f;
            GameObject.FindObjectOfType<CameraFollow>().SetCameraOffset(_radiusFactor * Mathf.Sqrt(count));
            if (count == 1)
            {
                GameObject.FindObjectOfType<CameraFollow>().SetOffsetX(-0.5f);
            }
        }
        else
        {
            nameInTop = _spawner.pGen.GenerateRandomFirstName();
        }
        FermatSpiralPlacement(1f);
    }

    private void Update()
    {
        //FermatSpiralPlacement();
    }

    private void FermatSpiralPlacement(float lerpcoef=0.1f)
    {
        float goldenAngle = 137.5f * _angleFactor;

        for (int i = 0; i < transform.childCount; i++)
        {
            float x = _radiusFactor * Mathf.Sqrt(i + 1) * Mathf.Cos(Mathf.Deg2Rad * goldenAngle * (i + 1));
            float z = _radiusFactor * Mathf.Sqrt(i + 1) * Mathf.Sin(Mathf.Deg2Rad * goldenAngle * (i + 1));

            Vector3 runnerLocalPosition = new Vector3(x, 0, z);
            transform.GetChild(i).localPosition = Vector3.Lerp(transform.GetChild(i).localPosition, runnerLocalPosition, lerpcoef);
        }
    }

    public float GetSquadRadius()
    {
        return _radiusFactor * Mathf.Sqrt(transform.childCount);
    }


    public void AddRunners(int amount)
    {
        if (transform.childCount > 500)
        {
            return;
        }

        for (int i = 0; i < amount; i++)
        {
            GameObject runnerInstance;
            if (IsHuman)
            {
                runnerInstance = _spawner.GetHuman();
            }
            else
            {
                runnerInstance = _spawner.GetAlien();
                
            }

            runnerInstance.transform.SetParent(transform);
            runnerInstance.transform.localPosition = Vector3.zero;
            runnerInstance.GetComponentInChildren<SkinnedMeshRenderer>().material = new Material(Shader.Find("Standard"));
            runnerInstance.GetComponentInChildren<SkinnedMeshRenderer>().material.color = _colorSquad;

            Runner runner = runnerInstance.GetComponentInChildren<Runner>();

            runner.SetRun(true);
            runnerInstance.SetActive(true);
            runner.addRunnersComponent = this;
            runnerInstance.GetComponent<RunnerData>().StartCoroutine(runnerInstance.GetComponent<RunnerData>().CallEnableRunner());
        }
        if (GetComponent<Movement>())
        {
            if (transform.childCount > 25)
            {
                GameObject.FindObjectOfType<CameraFollow>().coefHowNeadCam = 7f;
            }
            else if (transform.childCount > 21)
            {
                GameObject.FindObjectOfType<CameraFollow>().coefHowNeadCam = 8f;
            }
            else if(transform.childCount > 18)
            {
                GameObject.FindObjectOfType<CameraFollow>().coefHowNeadCam = 9f;
            }
            else if (transform.childCount > 15)
            {
                GameObject.FindObjectOfType<CameraFollow>().coefHowNeadCam = 10f;
            }
            else if (transform.childCount > 12)
            {
                GameObject.FindObjectOfType<CameraFollow>().coefHowNeadCam = 11f;
            }
            else if(transform.childCount > 10)
            {
                GameObject.FindObjectOfType<CameraFollow>().coefHowNeadCam = 12f;
            }
            else if (transform.childCount > 5)
            {
                GameObject.FindObjectOfType<CameraFollow>().coefHowNeadCam = 13f;
            }
            else if (transform.childCount > 3)
            {
                GameObject.FindObjectOfType<CameraFollow>().coefHowNeadCam = 14f;
            }
            GameObject.FindObjectOfType<CameraFollow>().SetCameraOffset(GetSquadRadius());
            GameObject.FindObjectOfType<CameraFollow>().SetOffsetX(0);
        }
        GameObject.FindObjectOfType<GameController>().GetTopPlayers();
        FermatSpiralPlacement(1f);
        if (transform.GetComponent<MovementNPC>())
        {
            transform.GetComponent<MovementNPC>().RotateAllPlayers();
        }else if (transform.GetComponent<Movement>())
        {
            transform.GetComponent<Movement>().RotateAllPlayers();
        }
    }

    public void SetIsRun(bool isRun)
    {
        foreach (Runner runner in transform.GetComponentsInChildren<Runner>())
        {
            runner.SetRun(isRun);
        }
    }

    public void DeadAllRunners()
    {
        foreach (Runner runner in transform.GetComponentsInChildren<Runner>())
        {
            runner.Dead();
        }
        TurnOffOnAllColliders();
    }

    public void TurnOffOnAllColliders(bool turnOn=false)
    {
        foreach(RunnerData data in GetComponentsInChildren<RunnerData>())
        {
            data.ColliderRunner.enabled = turnOn;
        }
    }
}
