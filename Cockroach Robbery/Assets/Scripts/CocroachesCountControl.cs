using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CocroachesCountControl : MonoBehaviour
{
    [SerializeField] private GameObject _barContainer;
    [SerializeField] private GameObject _bar;

    [SerializeField] private Transform _cocroachContainer;
    [SerializeField] private int _loseCocroachesCount;
    public int LoseCocroachesCount => _loseCocroachesCount;

    private bool _isLevelStarted=false;

    public event UnityAction LevelLose;

    public static CocroachesCountControl Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        SetProgress(0);
    }

    private void Start()
    {
        WavesController.Instance.LevelWin += OnEndLevel;
        GameController.Instance.StartLevel += OnStartLevel;
    }

    private void OnDestroy()
    {
        WavesController.Instance.LevelWin -= OnEndLevel;
        GameController.Instance.StartLevel -= OnStartLevel;
    }

    private void OnStartLevel()
    {
        _isLevelStarted = true;
    }

    private void OnEndLevel()
    {
        _isLevelStarted = false;
    }

    private void Update()
    {
        if (!_isLevelStarted) { return; }

        if(_cocroachContainer.childCount > _loseCocroachesCount)
        {
            LevelLose?.Invoke();
            OnEndLevel();
        }
        else
        {
            SetProgress(_cocroachContainer.childCount * 1f / _loseCocroachesCount);
        }
    }

    private void SetProgress(float progress)
    {
        _bar.GetComponent<RectTransform>().sizeDelta = new Vector2(_barContainer.GetComponent<RectTransform>().rect.width * progress, _bar.GetComponent<RectTransform>().sizeDelta.y);
    }
}
