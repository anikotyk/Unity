using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Settings : MonoBehaviour
{
    public event UnityAction LanguageChanged;

    public static Settings Instance { get; private set; }

    private void Awake()
    {

        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void RotateScreen(bool isVertical)
    {
        if (isVertical)
        {
            isVertical = true;
            Screen.orientation = ScreenOrientation.Portrait;
        }
        else
        {
            isVertical = false;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
    }

    public void InvokeLanguageChanged()
    {
        LanguageChanged?.Invoke();
    }
}
