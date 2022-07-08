using TMPro;
using UnityEngine;

public abstract class CountableObjectsController<T> : MonoBehaviour where T : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI _countText;
    [SerializeField] protected Animation[] _animationsOnShowCount;
    [SerializeField] protected bool _nonSigned;
    protected int _count;

    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if (Instance != null && Instance != GetComponent<T>())
        {
            Destroy(this);
            return;
        }
        
        Instance = GetComponent<T>();
        _count = 0;
        ShowCount();
    }

    public virtual void ResetCount()
    {
        SetCount(0);
    }

    public virtual int GetCount()
    {
        return _count;
    }
    
    public virtual void DecreaseCount(int amount)
    {
        if (_nonSigned)
        {
            SetCount(Mathf.Max(GetCount() - amount, 0));
        }
        else
        {
            SetCount(GetCount() - amount);
        }
    }

    public virtual void IncreaseCount(int amount)
    {
        SetCount(GetCount() + amount);
    }

    protected virtual void SetCount(int count)
    {
        _count = count;
        ShowCount();
    }

    protected virtual void ShowCount()
    {
        if (_countText != null)
        {
            _countText.text = GetCount() + "";
        }
        

        foreach (Animation animation in _animationsOnShowCount)
        {
            animation?.Play();
        }
    }
}
