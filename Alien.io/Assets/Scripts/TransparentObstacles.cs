using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentObstacles : MonoBehaviour
{
    private List<GameObject> currentObstacles;
    private List<GameObject> transparentObstacles;

    [SerializeField] private Transform playerContainer;
    private Transform cam;
    [SerializeField] private LayerMask environmentLayer;

    private void Awake()
    {
        currentObstacles = new List<GameObject>();
        transparentObstacles = new List<GameObject>();

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
        currentObstacles.Clear();
        float cameraPlayerDistance = Vector3.Magnitude(cam.position - playerContainer.position);

        Ray rayForward = new Ray(cam.position, playerContainer.position-cam.position);
        Ray rayBackward = new Ray(playerContainer.position, cam.position - playerContainer.position);

        var hitsForward = Physics.RaycastAll(rayForward, cameraPlayerDistance);
        var hitsBackward = Physics.RaycastAll(rayBackward, cameraPlayerDistance);

        foreach (var hit in hitsForward)
        {
            if(IsInLayerMask(hit.collider.gameObject, environmentLayer))
            {
                if (!currentObstacles.Contains(hit.collider.gameObject))
                {
                    currentObstacles.Add(hit.collider.gameObject);
                }
            }
        }

        foreach (var hit in hitsBackward)
        {
            if (IsInLayerMask(hit.collider.gameObject, environmentLayer))
            {
                if (!currentObstacles.Contains(hit.collider.gameObject))
                {
                    currentObstacles.Add(hit.collider.gameObject);
                }
            }
        }
    }

    private void SetCurrentObstaclesTransparent()
    {
        for(int i = 0; i < currentObstacles.Count; i++)
        {
            GameObject obj = currentObstacles[i];
            if (!transparentObstacles.Contains(obj))
            {
                SetAlpha(obj, 0.3f);
                transparentObstacles.Add(obj);
            }
        }
    }

    private void SetPreviousObstaclesNotTransparent()
    {
        for(int i = 0; i < transparentObstacles.Count; i++)
        {
            GameObject obj = transparentObstacles[i];
            if (!currentObstacles.Contains(obj))
            {
                SetAlpha(obj, 1f);
                transparentObstacles.Remove(obj);
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
            Debug.Log(renderer.gameObject.name+" "+ renderer.material.color);
        }
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((layerMask.value & (1 << obj.layer)) > 0);
    }
    
}
