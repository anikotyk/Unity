using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocroachesCountControl : MonoBehaviour
{
    [SerializeField] private GameObject _barContainer;
    [SerializeField] private GameObject _bar;

    [SerializeField] private Transform _cocroachContainer;
    [SerializeField] private int _maxCocroachesCount;

    private void Update()
    {
        SetProgress(_cocroachContainer.childCount/ _maxCocroachesCount);
    }

    private void SetProgress(float progress)
    {
        _bar.GetComponent<RectTransform>().sizeDelta = new Vector2(_barContainer.GetComponent<RectTransform>().rect.width * progress, _bar.GetComponent<RectTransform>().sizeDelta.y);
    }
}
