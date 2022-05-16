using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject panelWaitingForPlayers;

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;
    }

    public void StartGame()
    {
        Debug.Log("Start game");
        panelWaitingForPlayers.SetActive(false);
    }
}
