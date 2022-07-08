using UnityEngine;

public class BattlePokemonHealth : Health
{
    [SerializeField] private HealthBar _healthBar;
    public HealthBar HealthBar => _healthBar;

    protected override void ShowHealth()
    {
        _healthBar.SetValueBar(_healthCurrent);
    }
}