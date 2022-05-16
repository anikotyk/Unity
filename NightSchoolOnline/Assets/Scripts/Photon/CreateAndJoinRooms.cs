using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public InputField createInput;
    public InputField joinInput;
    public InputField nameInput;

    public void CreateRoom()
    {
        GetPlayerName();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createInput.text, roomOptions, null);
    }

    public void JoinRoom()
    {
        GetPlayerName();
        PhotonNetwork.JoinRoom(joinInput.text);
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("GameOnline");
    }

    private void GetPlayerName()
    {
        PhotonNetwork.NickName = nameInput.text;
    }
}
