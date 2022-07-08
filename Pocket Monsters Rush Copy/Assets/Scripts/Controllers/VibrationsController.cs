using UnityEngine;
using Lofelt.NiceVibrations;

public class VibrationsController : MonoBehaviour
{
    public static VibrationsController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        if (PlayerPrefs.GetInt("Vibration") < 1)
        {
            PlayerPrefs.SetInt("Vibration", 1);
        }

        SetVibrationsState();
        //HapticController.outputLevel = 0.5f;
    }
    
    public void SetVibrationsState()
    {
        HapticController.hapticsEnabled = (PlayerPrefs.GetInt("Vibration") == 1) && DeviceCapabilities.isVersionSupported;
    }
}
