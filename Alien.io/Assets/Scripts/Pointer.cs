using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pointer : MonoBehaviour
{
    [SerializeField] private Text _countText;
    [SerializeField] private Image _countContainer;
    [SerializeField] private GameObject _arrow;
   

    private Transform playerContainter;
    private float xMax;
    private float xMin;
    private float yMax;
    private float yMin;

    private float xPos;
    private float yPos;
    private float value;
    private float angle;

    private Vector3 direction;
    private Vector2 dir;
    private Vector3 screenPoint;

    private AddRuners target;
    private int targetCount;

    public AddRuners Target=>target;
    

    public void Initialize(Transform playerContainterData, float xMaxData, float xMinData, float yMaxData, float yMinData)
    {
        playerContainter = playerContainterData;
        xMax = xMaxData;
        xMin = xMinData;
        yMax = yMaxData;
        yMin = yMinData;
    }

    public void SetTarget(AddRuners addRunners, int count)
    {
        target = addRunners;
        targetCount = count;
    }

    private void Update()
    {
        if (target != null) {
            Point(target, target.transform.childCount);
        }
    }

    private void Point(AddRuners addRunners, int count)
    {
        Vector3 screenPointPlayer = Camera.main.WorldToViewportPoint(playerContainter.GetChild(0).position);
        Vector2 playerpos = new Vector2(transform.parent.parent.GetComponent<RectTransform>().rect.width * screenPointPlayer.x - transform.parent.parent.GetComponent<RectTransform>().rect.width / 2, transform.parent.parent.GetComponent<RectTransform>().rect.height * screenPointPlayer.y - transform.parent.parent.GetComponent<RectTransform>().rect.height / 2);

        screenPoint = Camera.main.WorldToViewportPoint(addRunners.transform.position);
        Vector2 pos = new Vector2(transform.parent.parent.GetComponent<RectTransform>().rect.width * screenPoint.x - transform.parent.parent.GetComponent<RectTransform>().rect.width / 2, transform.parent.parent.GetComponent<RectTransform>().rect.height * screenPoint.y - transform.parent.parent.GetComponent<RectTransform>().rect.height / 2);

        
        _countText.text = count + "";
        _countContainer.color = addRunners._colorSquad;
        _arrow.GetComponent<Image>().color = addRunners._colorSquad;
        if (!(screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1))
        {
            float offsetY = playerpos.y;

            direction = addRunners.transform.position - playerContainter.position;
            dir = new Vector2(direction.x, direction.z);

            angle = Vector2.SignedAngle(new Vector2(0, 1), dir.normalized);
            _arrow.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
            

            if (angle <= 0 && dir.x != 0)
            {
                value = (xMax / dir.x) * dir.y;
                if (value < yMax && value > yMin)
                {
                    xPos = xMax;
                    yPos = value;
                }
            }
            if (angle > 0 && dir.x != 0)
            {
                value = (xMin / dir.x) * dir.y;
                if (value < yMax && value > yMin)
                {
                    xPos = xMin;
                    yPos = value;
                }
            }
            if (Mathf.Abs(angle) <= 90 && dir.y != 0)
            {
                value = (yMax / dir.y) * dir.x;
                if (value < xMax && value > xMin)
                {
                    xPos = value;
                    yPos = yMax;
                }
            }
            if (Mathf.Abs(angle) > 90 && dir.y != 0)
            {
                value = (yMin / dir.y) * dir.x;
                if (value < xMax && value > xMin)
                {
                    xPos = value;
                    yPos = yMin;
                }
            }
            
            
            if(yPos >= yMin - offsetY)
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(xPos, yPos + offsetY, 0), 0.1f);
            }
            else
            {
                transform.localPosition = Vector3.Lerp(transform.localPosition, new Vector3(xPos, yPos, 0), 0.1f);
            }
        }
        else
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition , pos, 0.8f);
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
