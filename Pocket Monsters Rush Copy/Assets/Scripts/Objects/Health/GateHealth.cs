using TMPro;
using UnityEngine;

public class GateHealth : Health
{
    [SerializeField] private TextMeshPro _countRequiredBallsText;
    [SerializeField] private Animation _showAnimation;

    protected override void ShowHealth()
    {
        _countRequiredBallsText.text = _healthCurrent+"";
        _showAnimation?.Play();
    }
}
