using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class WavesController : MonoBehaviour
{
    [SerializeField] private Transform _wavesContainer;
    [SerializeField] private GameObject _wavePrefab;

    [SerializeField] private float _levelTimeInSeconds;
    [SerializeField] private int _wavesCountMin;
    [SerializeField] private int _wavesCountMax;

    [SerializeField] private GameObject _barContainer;
    [SerializeField] private GameObject _bar;

    private int _wavesCount;
    public int WavesCount => _wavesCount;
    private float _levelTimer;
    private float _wavesTimer;
    private float _waveTimeInSeconds;

    private Coroutine _wavesCoroutine;

    public event UnityAction WaveSpawn;
    public event UnityAction LevelWin;

    public static WavesController Instance { get; private set; }

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
        CocroachesCountControl.Instance.LevelLose += EndLevel;
        GameController.Instance.StartLevel += StartLevel;
    }

    private void OnDestroy()
    {
        CocroachesCountControl.Instance.LevelLose -= EndLevel;
        GameController.Instance.StartLevel -= StartLevel;
    }

    private void StartLevel()
    {
        _levelTimer = 0;
        _wavesTimer = 0;
        _wavesCount = Random.Range(_wavesCountMin, _wavesCountMax);
        _waveTimeInSeconds = _levelTimeInSeconds*1f / (_wavesCount+1);
        ClearWavesContainer();
        SpawnWavesImages();
        _wavesCoroutine = StartCoroutine(WavesCoroutine());
    }

    private void EndLevel()
    {
        StopCoroutine(_wavesCoroutine);
    }

    private IEnumerator WavesCoroutine()
    {
        while(_levelTimer < _levelTimeInSeconds)
        {
            if(_wavesTimer >= _waveTimeInSeconds)
            {
                WaveSpawn?.Invoke();
                _wavesTimer = 0;
            }
            yield return new WaitForSeconds(1f);
            _wavesTimer += 1;
            _levelTimer += 1;
            SetProgress(_levelTimer*1f/ _levelTimeInSeconds);
        }
        LevelWin?.Invoke();
    }

    private void ClearWavesContainer()
    {
        for(int i = 0; i < _wavesContainer.childCount; i++)
        {
            Destroy(_wavesContainer.GetChild(i).gameObject);
        }
    }

    private void SpawnWavesImages()
    {
        for (int i = 0; i < _wavesCount; i++)
        {
            GameObject wave = Instantiate(_wavePrefab);
            wave.transform.SetParent(_wavesContainer);
        }
    }

    private void SetProgress(float progress)
    {
        _bar.GetComponent<RectTransform>().sizeDelta = new Vector2(_barContainer.GetComponent<RectTransform>().rect.width * progress, _bar.GetComponent<RectTransform>().sizeDelta.y);
    }
}
