using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private float _timeLevel;
    [SerializeField] private float _extraTimeLevel;
    [SerializeField] private Text _timeText;
    
    [SerializeField] private InputField _nameInput;
    [SerializeField] private AddRuners _playerSquad;

    [SerializeField] private GameObject _canvasStart;
    [SerializeField] private GameObject _canvasPlay;
    [SerializeField] private GameObject _canvasGameEnd;
    [SerializeField] private GameObject _canvasGameEndTimeOutPanel;
    [SerializeField] private GameObject _canvasGameEndKilledPanel;
    [SerializeField] private GameObject _extraTimeButton;

    [SerializeField] private GameObject _addedRunnersCountParent;
    [SerializeField] private GameObject _addedRunnersCountPrefab;

    [SerializeField] private Color _greenColor;
    [SerializeField] private Color _orangeColor;
    [SerializeField] private Color _textGreenColor;
    [SerializeField] private Color _textOrangeColor;

    [SerializeField] private Sprite _humanIcon;
    [SerializeField] private Sprite _alienIcon;

    [SerializeField] private GameObject _topParent;

    private List<KeyValuePair<AddRuners, int>> _topRunners = new List<KeyValuePair<AddRuners, int>>();

    private Spawner _spawner;

    private Coroutine _setGameTimer;

    private bool _isExtraTime=false;

    private int _adsCount = 0;

    private int _topSize = 5;

    private void Awake()
    {
        _spawner = GameObject.FindObjectOfType<Spawner>();
        _spawner.isStopped = true;
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("PlayerLastName") == "")
        {
            PlayerPrefs.SetString("PlayerLastName", "Player");
        }

        _nameInput.placeholder.GetComponent<Text>().text = PlayerPrefs.GetString("PlayerLastName");
        _canvasStart.SetActive(true);
        _canvasPlay.SetActive(false);
        _playerSquad.SetIsRun(false);
        _canvasGameEnd.SetActive(false);
        _playerSquad.GetComponent<Movement>().isStopped = true;
    }
    
    public void OnClickStartGame()
    {
        StartGame();
    }

    private void StartGame()
    {
        _isExtraTime = false;
        
        if (_nameInput.text.Trim() != "")
        {
            _playerSquad.nameInTop = _nameInput.text;
        }
        else
        {
            _playerSquad.nameInTop = _nameInput.placeholder.GetComponent<Text>().text;
        }

        PlayerPrefs.SetString("PlayerLastName", _playerSquad.nameInTop);
        
        _spawner.StartGame();
        _spawner.isStopped = false;
        _playerSquad.GetComponent<Movement>().OnStartMoving();
        _playerSquad.SetIsRun(true);
        _canvasStart.SetActive(false);
        _canvasPlay.SetActive(true);
        _setGameTimer = StartCoroutine(SetGameTimer());

    }
    
    public void OnClickConfirmGameOver()
    {
        ConfirmGameOver();
    }

    private void ConfirmGameOver()
    {
        _adsCount += 1;

        if (_adsCount >= 3)
        {
            AdsManager.ShowAdsVideo(AdsManager.interstitialVideo);
            _adsCount = 0;
        }

        _playerSquad.GetComponent<Player>().KillPlayer();
        _spawner.EndGame();

        _canvasGameEnd.SetActive(false);
        _canvasStart.SetActive(true);
        
        _playerSquad.OnGameStart();
        _playerSquad.SetIsRun(false);
    }

    public void OnExtraTimeButtonClick()
    {
        AdsManager.ShowAdsVideo(AdsManager.rewardedVideo);
    }

    public void ContinueRuningExtraTime()
    {
        _isExtraTime = true;
        _canvasGameEnd.SetActive(false);
        StopStartAllRuners(false);
        _spawner.isStopped = false;
        _canvasPlay.SetActive(true);
        _setGameTimer = StartCoroutine(SetGameTimer(_extraTimeLevel));
    }

    public void EndGame(bool isTimeExpired = false)
    {
        StopCoroutine(_setGameTimer);
        _canvasPlay.SetActive(false);
        _spawner.isStopped = true;
        StopStartAllRuners(true);
        _canvasGameEnd.SetActive(true);
        if (_isExtraTime)
        {
            _extraTimeButton.SetActive(false);
        }
        else
        {
            _extraTimeButton.SetActive(true);
        }

        _canvasGameEndTimeOutPanel.SetActive(isTimeExpired);
        _canvasGameEndKilledPanel.SetActive(!isTimeExpired);
    }

    private IEnumerator SetGameTimer(float timetotimer=-1)
    {
        float timer;
        if (timetotimer > 0)
        {
            timer = timetotimer;
        }
        else
        {
            timer = _timeLevel;
        }
        
        _timeText.text = (int)timer / 60 + ":" + (timer - (int)(timer / 60) * 60);
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1f;
            _timeText.text = ((int)timer /60).ToString("00")+":" +(timer- (int)(timer / 60) *60).ToString("00");
        }
        EndGame(true);
    }


    public void GetTopPlayers()
    {
        _topRunners.Clear();

        foreach (AddRuners addRunners in GameObject.FindObjectsOfType<AddRuners>())
        {
            _topRunners.Add(new KeyValuePair<AddRuners, int>(addRunners, addRunners.transform.childCount));
        }

        _topRunners.Sort((x, y) => (x.Value.CompareTo(y.Value)));

        while (_topRunners.Count < _topSize)
        {
            _topRunners.Add(new KeyValuePair<AddRuners, int>(null, 0));
        }
        
        ShowTop();
    }

    private void StopStartAllRuners(bool isStop)
    {
        foreach(AddRuners addRuners in GameObject.FindObjectsOfType<AddRuners>())
        {
            addRuners.StopStartMoving(isStop);
        }
    }

    private void ShowTop()
    {
        for (int i = 0; i < _topSize; i++)
        {
            TopPlaceData data = _topParent.transform.GetChild(i).GetComponent<TopPlaceData>();
            if (_topRunners[i].Key == null)
            {
                data.runnerName.fontStyle = FontStyle.Normal;
                data.runnerCount.fontStyle = FontStyle.Normal;

                data.runnerName.text = "";
                data.runnerName.color = Color.black;

                data.runnerCount.text = "0";

                data.background.color = _greenColor;
                data.runnerCount.color = _textGreenColor;
                data.icon.sprite = _alienIcon;
            }
            else
            {
                if (_topRunners[i].Key.gameObject.GetComponent<Movement>())
                {
                    data.runnerName.fontStyle = FontStyle.Bold;
                    data.runnerCount.fontStyle = FontStyle.Bold;
                }
                else
                {
                    data.runnerName.fontStyle = FontStyle.Normal;
                    data.runnerCount.fontStyle = FontStyle.Normal;
                }
                if (_topRunners[i].Key.IsHuman)
                {
                    data.background.color = _orangeColor;
                    data.runnerCount.color = _textOrangeColor;
                    data.icon.sprite = _humanIcon;
                }
                else
                {
                    data.background.color = _greenColor;
                    data.runnerCount.color = _textGreenColor;
                    data.icon.sprite = _alienIcon;
                }

                data.runnerName.text = _topRunners[i].Key.nameInTop;
                data.runnerName.color = _topRunners[i].Key.ColorSquad;
                data.runnerCount.text = _topRunners[i].Value + "";
            }
        }
    }
    
    public void ShowCountAddedRunners(Vector3 pos, int count)
    {
        Rect rect = _addedRunnersCountParent.GetComponent<RectTransform>().rect;
        Vector3 position = Camera.main.WorldToViewportPoint(pos);
        Vector2 screenPosition = new Vector2(rect.width * position.x - rect.width / 2, rect.height * position.y - rect.height / 2);

        GameObject countObj = Instantiate(_addedRunnersCountPrefab);
        countObj.transform.parent = _addedRunnersCountParent.transform;
        countObj.transform.localPosition = screenPosition;
        Text textcount = countObj.GetComponentInChildren<Text>();
        textcount.text = "+" + count;
        textcount.color = _playerSquad.GetComponent<AddRuners>().ColorSquad;
        StartCoroutine(ShowCountAddedRunnersCoroutine(countObj));
    }

    private IEnumerator ShowCountAddedRunnersCoroutine(GameObject countObj)
    {
        Text textcount = countObj.GetComponentInChildren<Text>();
        while(textcount.fontSize > 1)
        {
            textcount.fontSize -= 1;
            yield return new WaitForSeconds(0.01f);
        }
        Destroy(countObj);
    }
}
