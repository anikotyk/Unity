using UnityEngine;
using UnityEngine.Events;

public class LevelController : CountableObjectsController<LevelController>
{
    public LevelData LevelData;
    [SerializeField] private GameObject[] _levels;
    private GameObject _level = null;

    public event UnityAction LevelLoaded;

    protected override void Awake()
    {
        base.Awake();

        if (PlayerPrefs.GetInt("Level") < 1)
        {
            PlayerPrefs.SetInt("Level", 1);
        }

        LoadLevel();
        ShowCount();
    }
    
    public override int GetCount()
    {
        return PlayerPrefs.GetInt("Level");
    }

    protected override void SetCount(int count)
    {
        PlayerPrefs.SetInt("Level", count);
        ShowCount();
    }

    protected override void ShowCount()
    {
        if (_countText != null)
        {
            _countText.text = "level "+ GetCount();
        }


        foreach (Animation animation in _animationsOnShowCount)
        {
            animation?.Play();
        }
    }

    public void LoadLevel()
    {
        if (_level != null)
        {
            Destroy(_level);
        }
        
        int levelIndex = Mathf.Abs(GetCount() % _levels.Length - 1);
        _level = Instantiate(_levels[levelIndex]);
        LevelData = _level.GetComponent<LevelData>();
        LevelLoaded?.Invoke();
    }
}
