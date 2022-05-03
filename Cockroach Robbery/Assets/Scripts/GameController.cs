using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject buttonNewLevel;

    public event UnityAction StartLevel;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        buttonNewLevel.SetActive(true);
    }

    private void Start()
    {
        CocroachesCountControl.Instance.LevelLose += GameOver;
        WavesController.Instance.LevelWin += GameOver;
    }

    private void OnDestroy()
    {
        CocroachesCountControl.Instance.LevelLose -= GameOver;
        WavesController.Instance.LevelWin -= GameOver;
    }

    private void GameOver()
    {
        buttonNewLevel.SetActive(true);
    }

    public void StartGameBtn()
    {
        StartLevel?.Invoke();
        buttonNewLevel.SetActive(false);
    }
}
