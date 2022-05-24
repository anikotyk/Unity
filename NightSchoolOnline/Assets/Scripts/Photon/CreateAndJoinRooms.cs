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
        if (!PhotonNetwork.IsConnected)
        {
            print("Not connected");
            return;
        }
        GetPlayerName();
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(createInput.text, roomOptions, TypedLobby.Default);
       // PhotonNetwork.CreateRoom(createInput.text); 
    }
    

    public void JoinRoom()
    {
        GetPlayerName();
        /*RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 5;
        PhotonNetwork.JoinOrCreateRoom(joinInput.text, roomOptions, null);*/
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
