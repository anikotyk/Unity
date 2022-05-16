using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefab;
    [SerializeField] private Transform _playerParent;

    private void Start()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            GameController.Instance.StartGame();
        }


        GameObject player = PhotonNetwork.Instantiate(_playerPrefab.name, Vector3.zero, Quaternion.identity);
        player.transform.SetParent(_playerParent);
        player.transform.localPosition = _playerPrefab.transform.localPosition;
    }
}
