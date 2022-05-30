using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(PhotonView))]
public class GameControllerOnline : MonoBehaviour, IGameController
{
    [SerializeField] private GameObject _panelWaitingForPlayer;
    [SerializeField] private Text _textPanelWaitingForPlayer;
    [SerializeField] private GameObject _panelGameOver;

    private PhotonView _view;

    public static GameControllerOnline Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;

        _view = GetComponent<PhotonView>();
        _textPanelWaitingForPlayer.text = "Waiting for second player\nRoom name - " + PhotonNetwork.CurrentRoom.Name;
        _panelWaitingForPlayer.SetActive(true);
    }

   
    public void StartGame()
    {
        _panelWaitingForPlayer.SetActive(false);
        _view.RPC("StartGamePun", RpcTarget.Others);
    }

    [PunRPC]
    private void StartGamePun()
    {
        _panelWaitingForPlayer.SetActive(false);
    }

    public void GameOver()
    {
        GameOverPun();
        _view.RPC("GameOverPun", RpcTarget.Others);
    }
    
    [PunRPC]
    private void GameOverPun()
    {
        _panelGameOver.SetActive(true);
    }

}
