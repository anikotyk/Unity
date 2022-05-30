using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour, IGameController
{
    [SerializeField] private GameObject _panelGameOver;

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

    }

    public void GameOver()
    {
        _panelGameOver.SetActive(true);
    }
}
