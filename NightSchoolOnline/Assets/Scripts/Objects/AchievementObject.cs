using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementObject : MonoBehaviour
{
    [SerializeField] private Image _starImage;
    public Image StarImage => _starImage;
    [SerializeField] private Text _achievementName;
    public Text AchievementName=> _achievementName;
}
