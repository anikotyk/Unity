using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyButton : RewardedAdsButton
{
    [SerializeField] private int _moneyAmountAfterAds = 100;

    protected override void OnAdsCompleted()
    {
        MoneyController.Instance.AddMoneyAmount(_moneyAmountAfterAds);
    }
}
