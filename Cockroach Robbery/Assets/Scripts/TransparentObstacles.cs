using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObstacles : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private LayerMask _wallsLayer;

    private List<GameObject> _currentObstacles;
    private List<GameObject> _transparentObstacles;

    private Transform cam;

    private void Awake()
    {
        _currentObstacles = new List<GameObject>();
        _transparentObstacles = new List<GameObject>();

        cam = Camera.main.transform;
    }

    private void Update()
    {
        GetCurrentObstacles();
        SetPreviousObstaclesNotTransparent();
        SetCurrentObstaclesTransparent();
    }

    private void GetCurrentObstacles()
    {
        _currentObstacles.Clear();
        float cameraPlayerDistance = Vector3.Magnitude(cam.position - _player.position);

        Ray rayForward = new Ray(cam.position, _player.position - cam.position);
        Ray rayBackward = new Ray(_player.position, cam.position - _player.position);

        var hitsForward = Physics.RaycastAll(rayForward, cameraPlayerDistance);
        var hitsBackward = Physics.RaycastAll(rayBackward, cameraPlayerDistance);

        foreach (var hit in hitsForward)
        {
            if (IsInLayerMask(hit.collider.gameObject, _wallsLayer))
            {
                if (!_currentObstacles.Contains(hit.collider.gameObject))
                {
                    _currentObstacles.Add(hit.collider.gameObject);
                }
            }
        }

        foreach (var hit in hitsBackward)
        {
            if (IsInLayerMask(hit.collider.gameObject, _wallsLayer))
            {
                if (!_currentObstacles.Contains(hit.collider.gameObject))
                {
                    _currentObstacles.Add(hit.collider.gameObject);
                }
            }
        }
    }

    private void SetCurrentObstaclesTransparent()
    {
        for (int i = 0; i < _currentObstacles.Count; i++)
        {
            GameObject obj = _currentObstacles[i];
            if (!_transparentObstacles.Contains(obj))
            {
                SetAlpha(obj, 0.3f);
                _transparentObstacles.Add(obj);
            }
        }
    }

    private void SetPreviousObstaclesNotTransparent()
    {
        for (int i = 0; i < _transparentObstacles.Count; i++)
        {
            GameObject obj = _transparentObstacles[i];
            if (!_currentObstacles.Contains(obj))
            {
                SetAlpha(obj, 1f);
                _transparentObstacles.Remove(obj);
            }
        }
    }

    private void SetAlpha(GameObject obj, float alpha)
    {
        Color col;
        if (obj.GetComponent<Renderer>())
        {
            col = obj.GetComponent<Renderer>().material.color;
            obj.GetComponent<Renderer>().material.color = new Color(col.r, col.g, col.b, alpha);
        }

        foreach (Renderer renderer in obj.GetComponentsInChildren<Renderer>())
        {
            col = renderer.material.color;
            col.a = alpha;
            renderer.material.color = col;

        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }

}
