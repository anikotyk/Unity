using Lofelt.NiceVibrations;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerBar : MonoBehaviour
{
    [SerializeField] private GameObject _barContainer;
    [SerializeField] private Image _barImage;
    [SerializeField] private GameObject _increasePanel;

    [SerializeField] private GameObject _waitingTap;

    [SerializeField] private float _time;
    [SerializeField] private float _decreaseSpeed;
    [SerializeField] private float _increaseSpeed;

    [SerializeField] private float _minPower;

    [SerializeField] private Animator _animator;
    
    private float _barValue;

    public float BarValue => _barValue;
    private Coroutine _decreaseCoroutine;

    private void OnEnable()
    {
        GameObject.FindObjectOfType<BattleController>().HeroWinAction += OnHeroWin;
        GameObject.FindObjectOfType<LevelController>().LevelLoaded += ResetBar;
    }
    

    private void OnHeroWin()
    {
        StartTaping();
    }

    private void StartTaping()
    {
        ResetBar();

        _waitingTap.SetActive(true);
        _barContainer.SetActive(true);
        _increasePanel.SetActive(true);
        _decreaseCoroutine = StartCoroutine(DecreaseCoroutine());
    }

    private IEnumerator DecreaseCoroutine()
    {
        float timer = 0;
        float waitTime = 1f / _decreaseSpeed;
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            timer += waitTime;
            if (timer >= _time)
            {
                CalculatePower();
                yield break;
            }

            _barValue -= 1;
            _barImage.fillAmount = _barValue * 1f / 100f;
            if (_barValue <= 0)
            {
                _barValue = 0;
            }
        }
    }

    public void Increase()
    {
        HapticPatterns.PlayPreset(HapticPatterns.PresetType.RigidImpact);
        _barValue += _increaseSpeed;
        _barImage.fillAmount = _barValue * 1f / 100f;

        if (_barValue >= 100)
        {
            _barValue = 100;
            StopCoroutine(_decreaseCoroutine);
            CalculatePower();
        }
    }

    private void CalculatePower()
    {
        BattleController.Instance.SendBulletsWithForce(_barValue);

        _animator.SetBool("Disappear", true);
        _increasePanel.SetActive(false);
        _waitingTap.SetActive(false);
    }
    
    private void ResetBar()
    {
        _barValue = 0;
        _barImage.fillAmount = 0;
        _animator.SetBool("Disappear", false);
    }
}
