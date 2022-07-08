using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsController : MonoBehaviour
{
    public static ArrowsController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void TurnOnArrowsColliders()
    {
        for(int i = 0; i<transform.childCount; i++)
        {
            transform.GetChild(i).GetComponent<Collider>().enabled = true;
        }
    }
}
