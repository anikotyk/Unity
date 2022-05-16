using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControll : MonoBehaviour
{
    public PhotonView view;

    private void Start()
    {
        if (view.IsMine)
        {
            transform.position = new Vector3(Random.Range(-3, 3), 1, Random.Range(-3, 3));
        }
    }
}
