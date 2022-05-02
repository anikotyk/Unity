using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedController : MonoBehaviour
{
    [SerializeField] private Text _speedText;

    public int CurrentSpeed { get; private set; }

    public static SpeedController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        ResetCurrentSpeed();
    }

    private void Start()
    {
        GameController.Instance.LevelEnded += CheckUpdateSpeed;
    }

    private void OnDestroy()
    {
        GameController.Instance.LevelEnded -= CheckUpdateSpeed;
    }
    
    private void CheckUpdateSpeed()
    {
        if (LevelController.Instance.GetLevel() % 5 == 0)
        {
            IncreasePlayerPrefsSpeed();
            ResetCurrentSpeed();
        }
    }

    private void ShowSpeed()
    {
        if (_speedText != null)
        {
            _speedText.text = "Speed: " + CurrentSpeed;
        }
    }

    public void ResetCurrentSpeed()
    {
        CurrentSpeed = GetPlayerPrefsSpeed();
        ShowSpeed();
    }

    public void IncreaseCurrentSpeed(int amount)
    {
        CurrentSpeed += amount;
        ShowSpeed();
    }
    
    public void IncreasePlayerPrefsSpeed(int amount=1)
    {
        PlayerPrefs.SetInt("speed", GetPlayerPrefsSpeed()+amount);
    }

    public int GetPlayerPrefsSpeed()
    {
        return PlayerPrefs.GetInt("speed");
    }
}
