using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointerToPlayers : MonoBehaviour
{
    [SerializeField] private GameObject _pointersParent;

    [SerializeField] private GameObject _pointerPrefab;

    [SerializeField] private int _countShownPointers;

    [SerializeField] private Transform _squadsParent;

    [SerializeField] private Transform _playerContainter;

    private float _xMax;
    private float _xMin;
    private float _yMax;
    private float _yMin;
    
    private Dictionary<Pointer, AddRuners> _squadsNear = new Dictionary<Pointer, AddRuners>();
    private List<KeyValuePair<AddRuners, float>> _squadsNearTop = new List<KeyValuePair<AddRuners, float>>();

    private GameController _gameController;

    private void Awake()
    {
        _xMax = _pointersParent.GetComponent<RectTransform>().rect.width / 2 - _pointerPrefab.GetComponent<RectTransform>().rect.width/2 - 20;
        _xMin = -_xMax;
        _yMax = _pointersParent.GetComponent<RectTransform>().rect.height / 2 - _pointerPrefab.GetComponent<RectTransform>().rect.height / 2 - 20;
        _yMin = -_yMax;

        _gameController = GameObject.FindObjectOfType<GameController>() ;
        for (int i = 0; i < _countShownPointers; i++)
        {
            GameObject pointer = Instantiate(_pointerPrefab, _pointersParent.transform);
            pointer.GetComponent<Pointer>().Initialize(_playerContainter, _xMax, _xMin, _yMax, _yMin);
        }
    }

    private void Update()
    {
        GetNearest();
        ShowPointers();
    }

    private void GetNearest()
    {
        _squadsNearTop.Clear();
        for (int i = 0; i < _squadsParent.childCount; i++)
        {
            float distance = Vector3.Distance(_playerContainter.position, _squadsParent.GetChild(i).position);
            _squadsNearTop.Add(new KeyValuePair<AddRuners, float>(_squadsParent.GetChild(i).GetComponent<AddRuners>(), distance));
        }
        _squadsNearTop.Sort((x, y) => (x.Value.CompareTo(y.Value)));
    }

    private void ShowPointers()
    {
        for(int i = 0; i < _pointersParent.transform.childCount; i++)
        {
            Pointer pointer = _pointersParent.transform.GetChild(i).GetComponent<Pointer>();
            if(_squadsNear.ContainsKey(pointer))
            {
                int index = FindIndexByKey(_squadsNearTop, _squadsNear[pointer]);
                if (index == -1 || index >= _countShownPointers)
                {
                    _squadsNear[pointer].isPointedNow = false;
                    pointer.SetTarget(null);
                    _squadsNear.Remove(pointer);
                }
            }
        }

        int cnt = Mathf.Min(_squadsNearTop.Count, _countShownPointers);
        for (int i = 0; i < cnt; i++)
        {
            if (!_squadsNearTop[i].Key.isPointedNow)
            {
                Pointer pointer = GetPointerWithoutTarget();
                pointer.SetTarget(_squadsNearTop[i].Key);
                _squadsNearTop[i].Key.isPointedNow = true;
                _squadsNear.Add(pointer, _squadsNearTop[i].Key);
            }
        }
    }

    private int FindIndexByKey(List<KeyValuePair<AddRuners, float>> array, AddRuners elem)
    {
        for(int i=0; i<array.Count; i++)
        {
            if (array[i].Key == elem)
            {
                return i;
            }
        }
        return -1;
    }

    private Pointer GetPointerWithoutTarget()
    {
        for(int i = 0; i < _pointersParent.transform.childCount; i++)
        {
            if (_pointersParent.transform.GetChild(i).GetComponent<Pointer>().Target == null)
            {
                return _pointersParent.transform.GetChild(i).GetComponent<Pointer>();
            }
        }
        return null;
    }
    
}
