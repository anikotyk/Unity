using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Text levelNow;
    [SerializeField] private Text levelNext;

    public static LevelController Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        ShowLevel();
    }

    private void Start()
    {
        GameController.Instance.LevelEnded += NextLevel;
        GameController.Instance.NextLevelClicked += ShowLevel;
    }

    private void OnDestroy()
    {
        GameController.Instance.LevelEnded -= NextLevel;
        GameController.Instance.NextLevelClicked -= ShowLevel;
    }


    public int GetLevel()
    {
        return PlayerPrefs.GetInt("level");
    }

    private void NextLevel()
    {
        PlayerPrefs.SetInt("level", GetLevel()+1);
    }

    private void ShowLevel()
    {
        levelNow.text = GetLevel() + "";
        levelNext.text = GetLevel() + 1 + "";
    }
}