using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AddRuners : MonoBehaviour
{
    [Header(" Formation Settings ")]
    [Range(0f, 1f)] [SerializeField] private float radiusFactor;
    [Range(0f, 1f)] [SerializeField] private float angleFactor;

    [Header(" Settings ")]
    [SerializeField] private GameObject humanRunnerPrefab;
    [SerializeField] private GameObject alienRunnerPrefab;

    public Color _colorSquad;
    public bool IsHuman;
    public string nameInTop;
    //public bool IsHuman=>IsHuman;
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
            GameObject.FindObjectOfType<CameraFollow>().SetCameraOffset(radiusFactor * Mathf.Sqrt(count) * 10);
            if (count == 1)
            {
                GameObject.FindObjectOfType<CameraFollow>().SetOffsetX(-0.5f);
            }
        }
        else
        {
            nameInTop = GameObject.FindObjectOfType<Spawner>().pGen.GenerateRandomFirstName();
        }
    }

    void Update()
    {
        FermatSpiralPlacement();
    }

    private void FermatSpiralPlacement()
    {
        float goldenAngle = 137.5f * angleFactor;

        for (int i = 0; i < transform.childCount; i++)
        {
            float x = radiusFactor * Mathf.Sqrt(i + 1) * Mathf.Cos(Mathf.Deg2Rad * goldenAngle * (i + 1));
            float z = radiusFactor * Mathf.Sqrt(i + 1) * Mathf.Sin(Mathf.Deg2Rad * goldenAngle * (i + 1));

            Vector3 runnerLocalPosition = new Vector3(x, 0, z);
            transform.GetChild(i).localPosition = Vector3.Lerp(transform.GetChild(i).localPosition, runnerLocalPosition, 0.1f);
        }
    }

    public float GetSquadRadius()
    {
        return radiusFactor * Mathf.Sqrt(transform.childCount);
    }


    public void AddRunners(int amount)
    {
        if (transform.childCount > 60)
        {
            return;
        }
        for (int i = 0; i < amount; i++)
        {
            GameObject runnerInstance;
            if (IsHuman)
            {
                runnerInstance = Instantiate(humanRunnerPrefab, transform);
            }
            else
            {
                runnerInstance = Instantiate(alienRunnerPrefab, transform); 
            }
            

            runnerInstance.GetComponentInChildren<SkinnedMeshRenderer>().material = new Material(Shader.Find("Standard"));
            runnerInstance.GetComponentInChildren<SkinnedMeshRenderer>().material.color = _colorSquad;
        }
        if (GetComponent<Movement>())
        {
            //Camera.main.fieldOfView += amount;
            GameObject.FindObjectOfType<CameraFollow>().SetCameraOffset(GetSquadRadius()*10);
            GameObject.FindObjectOfType<CameraFollow>().SetOffsetX(0);
        }
        //GameObject.FindObjectOfType<GameController>().CheckInTop(this);
        GameObject.FindObjectOfType<GameController>().GetTopPlayers();
    }

    public void SetIsRun(bool isRun)
    {
        foreach (Runner runner in transform.GetComponentsInChildren<Runner>())
        {
            runner.SetRun(isRun);
        }
    }
}
