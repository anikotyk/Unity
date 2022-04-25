using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedController : MonoBehaviour
{
    [SerializeField] private Text speedText;

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

        CurrentSpeed = GetPlayerPrefsSpeed();
        ShowSpeed();
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
            ChangePlayerPrefsSpeed(GetPlayerPrefsSpeed() + 1);
            ChangeCurrentSpeed(GetPlayerPrefsSpeed());
        }
    }

    private void ShowSpeed()
    {
        if (speedText != null)
        {
            speedText.text = "Speed: " + CurrentSpeed;
        }
    }

    public void ChangeCurrentSpeed(int speed)
    {
        CurrentSpeed = speed;
        ShowSpeed();
    }

    public void ChangePlayerPrefsSpeed(int speed)
    {
        PlayerPrefs.SetInt("speed", speed);
    }

    public int GetPlayerPrefsSpeed()
    {
        return PlayerPrefs.GetInt("speed");
    }
}
