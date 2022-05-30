using Photon.Pun;
using UnityEngine;

public class SpawnPlayers : MonoBehaviour
{
    [SerializeField] private GameObject _playerPrefabMale;
    [SerializeField] private GameObject _playerPrefabFemale;
    [SerializeField] private Transform _playerParent;

    [SerializeField] private Vector3 _positionFirst;
    [SerializeField] private Vector3 _positionSecond;
    [SerializeField] private Vector3 _rotationFirst;
    [SerializeField] private Vector3 _rotationSecond;

    private void Start()
    {
        GameObject playerPrefab;
        if (PlayerPrefs.GetInt("isFemale") == 1)
        {
            playerPrefab = _playerPrefabFemale;
            
        }
        else
        {
            playerPrefab = _playerPrefabMale;
        }

        GameObject player = PhotonNetwork.Instantiate(playerPrefab.name, Vector3.zero, Quaternion.identity);
        player.transform.SetParent(_playerParent);
        player.transform.localPosition = playerPrefab.transform.localPosition;

       
        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            GameObject.FindObjectOfType<StarterAssets.FirstPersonController>().transform.position = _positionSecond;
            GameObject.FindObjectOfType<StarterAssets.FirstPersonController>().transform.rotation = Quaternion.Euler(_rotationSecond);

            GameControllerOnline.Instance.StartGame();
        }
        else
        {
            GameObject.FindObjectOfType<StarterAssets.FirstPersonController>().transform.position = _positionFirst;
            GameObject.FindObjectOfType<StarterAssets.FirstPersonController>().transform.rotation = Quaternion.Euler(_rotationFirst);
        }

    }
}
