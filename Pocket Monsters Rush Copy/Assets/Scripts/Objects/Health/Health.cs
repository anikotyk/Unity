using UnityEngine;

public abstract class Health : MonoBehaviour
{
    [SerializeField] protected int _maxHealth;
    protected int _health;
    protected int _healthCurrent;

    protected virtual void Awake()
    {
        ResetHealth();
    }

    protected virtual void ResetHealth()
    {
        _health = _maxHealth;
        _healthCurrent = _maxHealth;
        ShowHealth();
    }

    public virtual bool CheckIsAlive()
    {
        return (_health > 0);
    }

    public virtual bool CheckIsAliveCurrent()
    {
        return (_healthCurrent > 0);
    }

    public virtual void GetDamage(int damage)
    {
        _health -= damage;
        if (_health < 0)
        {
            _health = 0;
        }
    }

    public virtual void GetCurrentDamage(int damage)
    {
        _healthCurrent -= damage;
        if (_healthCurrent < 0)
        {
            _healthCurrent = 0;
        }
        ShowHealth();
    }

    protected virtual void ShowHealth()
    {
        //Show healthCurrent
    }
}
