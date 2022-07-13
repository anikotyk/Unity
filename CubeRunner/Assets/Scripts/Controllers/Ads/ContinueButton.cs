using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ContinueButton : RewardedAdsButton
{
    public event UnityAction ContinueButtonClicked;

    protected override void OnAdsCompleted()
    {
        ContinueButtonClicked?.Invoke();
    }
}
