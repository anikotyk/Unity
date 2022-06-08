using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelController : MonoBehaviour
{
    [SerializeField] private Text _levelNow;
    [SerializeField] private Text _levelNext;

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
        GameController.Instance.LevelCompleted += NextLevel;
        GameController.Instance.NextLevelClicked += ShowLevel;
    }

    private void OnDestroy()
    {
        GameController.Instance.LevelCompleted -= NextLevel;
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
        _levelNow.text = GetLevel() + "";
        _levelNext.text = GetLevel() + 1 + "";
    }
}
