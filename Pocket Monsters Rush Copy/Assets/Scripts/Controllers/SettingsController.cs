using UnityEngine;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    [SerializeField] private Sprite _buttonOffSprite;
    [SerializeField] private Sprite _buttonOnSprite;

    [SerializeField] private GameObject _soundOnImage;
    [SerializeField] private GameObject _soundOffImage;

    [SerializeField] private GameObject _vibrationOnImage;
    [SerializeField] private GameObject _vibrationOffImage;

    [SerializeField] private Image _vibrationButton;
    [SerializeField] private Image _soundButton;

    public static SettingsController Instance { get; private set; }



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
        UpdateButtonsView();
    }

    public void SoundButton()
    {
        if(PlayerPrefs.GetInt("Sound") == 1)
        {
            PlayerPrefs.SetInt("Sound", 2);
        }
        else
        {
            PlayerPrefs.SetInt("Sound", 1);
        }
        UpdateButtonsView();
    }

    public void VibrationButton()
    {
        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            PlayerPrefs.SetInt("Vibration", 2);
        }
        else
        {
            PlayerPrefs.SetInt("Vibration", 1);
        }
        UpdateButtonsView();

        VibrationsController.Instance.SetVibrationsState();
    }

    private void UpdateButtonsView()
    {
        if (PlayerPrefs.GetInt("Sound") == 1)
        {
            _soundButton.sprite = _buttonOnSprite;
            _soundOnImage.SetActive(true);
            _soundOffImage.SetActive(false);
        }
        else
        {
            _soundButton.sprite = _buttonOffSprite;
            _soundOnImage.SetActive(false);
            _soundOffImage.SetActive(true);
        }

        if (PlayerPrefs.GetInt("Vibration") == 1)
        {
            _vibrationButton.sprite = _buttonOnSprite;
            _vibrationOnImage.SetActive(true);
            _vibrationOffImage.SetActive(false);
        }
        else
        {
            _vibrationButton.sprite = _buttonOffSprite;
            _vibrationOnImage.SetActive(false);
            _vibrationOffImage.SetActive(true);
        }
    }
}