using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListener : MonoBehaviourPunCallbacks
{
    [SerializeField] private Text _wordText;
    private Player _player;

    public void SetPlayerInfo(Player player)
    {
        _player = player;
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log("entered");
        SetPlayerInfo(newPlayer);
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, ExitGames.Client.Photon.Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
        Debug.Log("hew "+ targetPlayer.IsMasterClient+" "+_player.IsMasterClient);
        if(targetPlayer!=null && targetPlayer == _player)
        {
            if (changedProps.ContainsKey("word"))
            {
                _wordText.text = changedProps["word"]+"";
            }
        }
    }
}
