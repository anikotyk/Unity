using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WallsCoeffAnim : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _coefText;
    [SerializeField] private GameObject _coef;
    [SerializeField] private GameObject _image;
    [SerializeField] private Animator _animator;
    [SerializeField] private Animation _anim;
    
    public static WallsCoeffAnim Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void SetCoef(string coef)
    {
        _coef.SetActive(true);
        _image.SetActive(true);
        _animator.enabled = true;
        _coefText.text = coef;
        _anim.Play();
    }

    public void Hide()
    {
        _animator.enabled = false;
        _coef.SetActive(false);
        _image.SetActive(false);
    }
}
