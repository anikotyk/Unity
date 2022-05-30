using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.UI;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    [SerializeField] private InputField _roomNameInput;
    [SerializeField] private GameObject _loadingPanel;
    [SerializeField] private GameObject _errorText;
    [SerializeField] private Toggle _femaleToggle;

    public void CreateOrJoinRoom()
    {
        if (!PhotonNetwork.IsConnected)
        {
            _errorText.GetComponent<Text>().text = "<color='red'>Error.</color> Check you internet connection!";
            _errorText.SetActive(true);
            return;
        }
        _errorText.SetActive(false);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.JoinOrCreateRoom(_roomNameInput.text, roomOptions, TypedLobby.Default);
        _loadingPanel.SetActive(true);
    }
    
    public override void OnJoinedRoom()
    {
        _errorText.SetActive(false);
        if (_femaleToggle.isOn)
        {
            PlayerPrefs.SetInt("isFemale", 1);
        }
        else
        {
            PlayerPrefs.SetInt("isFemale", 0);
        }
        
        
        PhotonNetwork.LoadLevel("GameOnline");
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        _loadingPanel.SetActive(false);
        _errorText.GetComponent<Text>().text = "<color='red'>Error.</color> Try another room name and check your internet connection!";
        _errorText.SetActive(true);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        _loadingPanel.SetActive(false);
        _errorText.GetComponent<Text>().text = "<color='red'>Error.</color> Check your internet connection!";
        _errorText.SetActive(true);
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        _loadingPanel.SetActive(false);
        _errorText.GetComponent<Text>().text = "<color='red'>Error.</color> Check you internet connection!";
        _errorText.SetActive(true);
    }
}
