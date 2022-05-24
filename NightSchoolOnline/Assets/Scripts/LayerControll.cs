using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhotonView))]
public class LayerControll : MonoBehaviour
{
    private PhotonView _view;
    
    private void Start()
    {
        _view = GetComponent<PhotonView>();
        if (_view.IsMine)
        {
            int LayerCameraIgnore = LayerMask.NameToLayer("CameraIgnore");
            SetLayerRecursively(gameObject, LayerCameraIgnore);
            //gameObject.layer = LayerCameraIgnore;
        }
        
    }

    private void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;

        for (int i = 0; i<obj.transform.childCount; i++)
        {
            SetLayerRecursively(obj.transform.GetChild(i).gameObject, layer);
        }
    }
}
