using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    [SerializeField] private Text _countText;
    [SerializeField] private Image _countContainer;
    [SerializeField] private GameObject _arrow;
   
    private Transform _playerContainter;

    private float _xMax;
    private float _xMin;
    private float _yMax;
    private float _yMin;

    private float _xPos;
    private float _yPos;
    private float _value;
    private float _angle;

    private Vector3 _direction;
    private Vector2 _dir;
    private Vector3 _screenPoint;
    private Vector3 _screenPointPlayer;
    private Vector2 _playerpos;
    private Vector2 _pos;
    private float offsetY;

    private AddRuners _target;

    public AddRuners Target=>_target;
    
    public void Initialize(Transform playerContainterData, float xMaxData, float xMinData, float yMaxData, float yMinData)
    {
        _playerContainter = playerContainterData;
        _xMax = xMaxData;
        _xMin = xMinData;
        _yMax = yMaxData;
        _yMin = yMinData;
    }

    public void SetTarget(AddRuners addRunners)
    {
        _target = addRunners;
    }

    private void Update()
    {
        if (_target != null) {
            Point(_target, _target.transform.childCount);
        }
        else
        {
            PointerSetActive(false);
        }
    }

    private void Point(AddRuners addRunners, int count)
    {
        _screenPointPlayer = Camera.main.WorldToViewportPoint(_playerContainter.GetChild(0).position);
        _playerpos = new Vector2(transform.parent.parent.GetComponent<RectTransform>().rect.width * _screenPointPlayer.x - transform.parent.parent.GetComponent<RectTransform>().rect.width / 2, transform.parent.parent.GetComponent<RectTransform>().rect.height * _screenPointPlayer.y - transform.parent.parent.GetComponent<RectTransform>().rect.height / 2);

        _screenPoint = Camera.main.WorldToViewportPoint(addRunners.transform.position);
        
        _countText.text = count + "";
        _countContainer.color = addRunners.ColorSquad;
        _arrow.GetComponent<Image>().color = addRunners.ColorSquad;

        if (!(_screenPoint.z > 0 && _screenPoint.x > 0 && _screenPoint.x < 1 && _screenPoint.y > 0 && _screenPoint.y < 1))
        {
            offsetY = _playerpos.y;

            _direction = addRunners.transform.position - _playerContainter.position;
            _dir = new Vector2(_direction.x, _direction.z);

            _angle = Vector2.SignedAngle(new Vector2(0, 1), _dir.normalized);
            _arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, _angle));
            

            if (_angle <= 0 && _dir.x != 0)
            {
                _value = (_xMax / _dir.x) * _dir.y;
                if (_value < _yMax && _value > _yMin)
                {
                    _xPos = _xMax;
                    _yPos = _value;
                }
            }
            if (_angle > 0 && _dir.x != 0)
            {
                _value = (_xMin / _dir.x) * _dir.y;
                if (_value < _yMax && _value > _yMin)
                {
                    _xPos = _xMin;
                    _yPos = _value;
                }
            }
            if (Mathf.Abs(_angle) <= 90 && _dir.y != 0)
            {
                _value = (_yMax / _dir.y) * _dir.x;
                if (_value < _xMax && _value > _xMin)
                {
                    _xPos = _value;
                    _yPos = _yMax;
                }
            }
            if (Mathf.Abs(_angle) > 90 && _dir.y != 0)
            {
                _value = (_yMin / _dir.y) * _dir.x;
                if (_value < _xMax && _value > _xMin)
                {
                    _xPos = _value;
                    _yPos = _yMin;
                }
            }
            
            
            if(_yPos >= _yMin - offsetY)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(_xPos, _yPos + offsetY, 0), 0.1f);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(_xPos, _yPos, 0), 0.1f);
            }
        }
        else
        {
            _pos = new Vector2(transform.parent.parent.GetComponent<RectTransform>().rect.width * _screenPoint.x - transform.parent.parent.GetComponent<RectTransform>().rect.width / 2, transform.parent.parent.GetComponent<RectTransform>().rect.height * _screenPoint.y - transform.parent.parent.GetComponent<RectTransform>().rect.height / 2);

            transform.localPosition = Vector3.Lerp(transform.localPosition , _pos, 0.8f);
            _arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 180));
        }

        PointerSetActive(true);
    }

    private void PointerSetActive(bool isActive)
    {
        _countContainer.gameObject.SetActive(isActive);
        _arrow.SetActive(isActive);
    }
}
