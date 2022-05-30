using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    private bool isVertical = true;

    public void RotateScreen()
    {
        if (isVertical)
        {
            isVertical = false;
            Screen.orientation = ScreenOrientation.LandscapeLeft;
        }
        else
        {
            isVertical = true;
            Screen.orientation = ScreenOrientation.Portrait;
        }
    }
    
}
