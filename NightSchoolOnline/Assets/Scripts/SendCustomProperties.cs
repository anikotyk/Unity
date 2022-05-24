using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendCustomProperties : MonoBehaviour
{
    private ExitGames.Client.Photon.Hashtable _customWord = new ExitGames.Client.Photon.Hashtable();


    private void SendWord(string word)
    {
        _customWord["word"] = word;
        PhotonNetwork.SetPlayerCustomProperties(_customWord);
    }
    
    public void SendBtn(string word)
    {
        SendWord(word);
    }
}
