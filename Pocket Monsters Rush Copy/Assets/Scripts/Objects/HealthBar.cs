using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image _bar;
    [SerializeField] private GameObject _canvas;

    private void Awake()
    {
        ResetBar();
    }

    public void Activate()
    {
        _canvas.SetActive(true);
    }

    public void Deactivate()
    {
        _canvas.SetActive(false);
    }

    public void ResetBar()
    {
        _bar.fillAmount = 1;
    }

    public void SetValueBar(int value)
    {
        _bar.fillAmount = value*1f/100f;
    }
}
