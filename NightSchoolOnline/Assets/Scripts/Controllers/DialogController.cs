using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogController : MonoBehaviour
{
    [SerializeField] private GameObject _textPlace;
    [SerializeField] private Text _textValue;

    private bool _isNowTexting;
    private DialogObject _nowDialog;
    private bool _unlocked;
    private int _replicaIndex;
    private StarterAssets.FirstPersonController _playerControll;

    private void Awake()
    {
        _textPlace.SetActive(false);
        _playerControll = GameObject.FindObjectOfType<StarterAssets.FirstPersonController>();
    }

    public void StartReplicasSet(DialogObject dialog)
    {
        _playerControll.enabled = false;
        _isNowTexting = true;
        _nowDialog = dialog;
        _replicaIndex = 0;

        _unlocked = true;
        ShowReplica();
        _textPlace.SetActive(true);
    }

    public void ShowReplica()
    {
        if (!_unlocked)
        {
            return;
        }
        if (_replicaIndex >= _nowDialog.replicas[_nowDialog.nowReplicasSet].replicasSet.Count)
        {
            EndReplicasSet();
            return;
        }
        _textValue.text = _nowDialog.GetReplica(_replicaIndex);
        _replicaIndex++;
        StartCoroutine(WaitingForUnlock());
    }

    public IEnumerator WaitingForUnlock()
    {
        _unlocked = false;
        yield return new WaitForSeconds(0.2f);
        _unlocked = true;
    }

    public void EndReplicasSet()
    {
        _isNowTexting = false;
        _unlocked = false;
        _textPlace.SetActive(false);
        _playerControll.enabled = true;
        /*if (_nowDialog.funcsAfterDialogs.Count > _nowDialog.nowReplicasSet)
        {
            _nowDialog.funcsAfterDialogs[_nowDialog.nowReplicasSet].Invoke();
        }
        _nowDialog.funcsAfterEverySet.Invoke();*/
    }

    public IEnumerator CloseReplicas()
    {
        yield return new WaitForSeconds(1f);
        if (!_isNowTexting)
        {
            _textPlace.SetActive(false);
        }
    }
}
