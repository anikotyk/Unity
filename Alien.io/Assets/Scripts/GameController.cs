using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    [SerializeField] private float timeLevel;
    [SerializeField] private float extraTimeLevel;
    [SerializeField] private Text timeText;

    private List<KeyValuePair<AddRuners, int>> topRunners = new List<KeyValuePair<AddRuners, int>>();

    [SerializeField] private InputField nameInput;
    [SerializeField] private AddRuners playerSquad;

    [SerializeField] private GameObject canvasStart;
    [SerializeField] private GameObject canvasPlay;
    [SerializeField] private GameObject canvasGameEnd;
    [SerializeField] private GameObject canvasGameEndTimeOutPanel;
    [SerializeField] private GameObject canvasGameEndKilledPanel;
    [SerializeField] private GameObject extraTimeButton;

    [SerializeField] private Color greenColor;
    [SerializeField] private Color orangeColor;
    [SerializeField] private Color textGreenColor;
    [SerializeField] private Color textOrangeColor;

    [SerializeField] private Sprite humanIcon;
    [SerializeField] private Sprite alienIcon;

    [SerializeField] private GameObject topParent;

    private Spawner spawner;

    private Coroutine setGameTimer;

    private bool isExtraTime=false;

    

    public void OpenScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    private void Awake()
    {
        spawner = GameObject.FindObjectOfType<Spawner>();
        spawner.isStopped = true;
    }

    private void Start()
    {
        if (PlayerPrefs.GetString("PlayerLastName") == "")
        {
            PlayerPrefs.SetString("PlayerLastName", "Player");
        }

        nameInput.placeholder.GetComponent<Text>().text = PlayerPrefs.GetString("PlayerLastName");
        canvasStart.SetActive(true);
        canvasPlay.SetActive(false);
        playerSquad.SetIsRun(false);
        canvasGameEnd.SetActive(false);
        playerSquad.GetComponent<Movement>().isStopped = true;
    }
    
    public void StartGame()
    {
        isExtraTime = false;
        
        if (nameInput.text.Trim() != "")
        {
            playerSquad.nameInTop = nameInput.text;
        }
        else
        {
            playerSquad.nameInTop = nameInput.placeholder.GetComponent<Text>().text;
        }

        //spawner.enabled = true;
        spawner.StartGame();
        spawner.isStopped = false;
        //playerSquad.GetComponent<Movement>().enabled = true;
        playerSquad.GetComponent<Movement>().OnStartMoving();
        playerSquad.SetIsRun(true);
        canvasStart.SetActive(false);
        canvasPlay.SetActive(true);
        setGameTimer = StartCoroutine(SetGameTimer());

    }
    

    public void ConfirmGameOver()
    {
        //spawner.ClearChildren(playerSquad.transform);
        spawner.ClearRuners(playerSquad.transform, playerSquad.IsHuman);
        spawner.EndGame();

        canvasGameEnd.SetActive(false);
        canvasStart.SetActive(true);
        
        playerSquad.OnGameStart();
        playerSquad.SetIsRun(false);
        //playerSquad.GetComponent<Movement>().enabled = false;
    }

    public void ContinueRuning()
    {
        isExtraTime = true;
        canvasGameEnd.SetActive(false);
        StopStartAllRuners(false);
        spawner.isStopped = false;
        canvasPlay.SetActive(true);
        setGameTimer = StartCoroutine(SetGameTimer(extraTimeLevel));
    }

    public void EndGame2(bool isTimeExpired = false)
    {
        StopCoroutine(setGameTimer);
        canvasPlay.SetActive(false);
        spawner.isStopped = true;
        StopStartAllRuners(true);
        canvasGameEnd.SetActive(true);
        if (isExtraTime)
        {
            extraTimeButton.SetActive(false);
        }
        else
        {
            extraTimeButton.SetActive(true);
        }

        canvasGameEndTimeOutPanel.SetActive(isTimeExpired);
        canvasGameEndKilledPanel.SetActive(!isTimeExpired);
    }

    public IEnumerator SetGameTimer(float timetotimer=-1)
    {
        float timer;
        if (timetotimer > 0)
        {
            timer = timetotimer;
        }
        else
        {
            timer = timeLevel;
        }
        
        timeText.text = (int)timer / 60 + ":" + (timer - (int)(timer / 60) * 60);
        while (timer > 0)
        {
            yield return new WaitForSeconds(1f);
            timer -= 1f;
            timeText.text = ((int)timer /60).ToString("00")+":" +(timer- (int)(timer / 60) *60).ToString("00");
        }
        //OpenScene("Menu");
        //EndGame();
        EndGame2(true);
    }


    public void GetTopPlayers()
    {
        AddRuners[] squads = GameObject.FindObjectsOfType<AddRuners>();

        topRunners.Clear();
       
        for (int i = 0; i < squads.Length - 1; i++)
        {
            for (int j = 0; j < squads.Length - i - 1; j++)
            {
                if (squads[j].transform.childCount < squads[j + 1].transform.childCount)
                {
                    AddRuners temp = squads[j];
                    squads[j] = squads[j + 1];
                    squads[j + 1] = temp;
                }
            }
        }
        
        for (int i = 0; i < squads.Length; i++)
        {
            if (i >= 5)
            {
                break;
            }
            topRunners.Add(new KeyValuePair<AddRuners, int>(squads[i], squads[i].transform.childCount));
        }

        while (topRunners.Count < 5)
        {
            topRunners.Add(new KeyValuePair<AddRuners, int>(null, 0));
        }
        
        ShowTop();
    }

    public void CheckInTop(AddRuners runner)
    {
        int index = FindRunnerIndex(runner);
        if (index > -1)
        {
            for(int i = index; i < topRunners.Count - 1; i++)
            {
                topRunners[i] = topRunners[i + 1];
            }
            topRunners[topRunners.Count - 1] = new KeyValuePair<AddRuners, int>(null, 0);
        }
        int indexintop=0;
        for(int i = topRunners.Count-1; i >= 0; i--)
        {
            if(runner.transform.childCount < topRunners[i].Value)
            {
                indexintop = i+1;
                break;
            }
        }

        if (indexintop >= topRunners.Count)
        {
            ShowTop();
            return;
        }

        for (int i = topRunners.Count - 1; i > indexintop; i--)
        {
            topRunners[i] = topRunners[i - 1];
        }


        topRunners[indexintop] = new KeyValuePair<AddRuners, int>(runner, runner.transform.childCount);

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
        for(int i = 0; i < topRunners.Count; i++)
        {
            TopPlaceData data = topParent.transform.GetChild(i).GetComponent<TopPlaceData>();
            if (topRunners[i].Key == null)
            {
                data.runnerName.fontStyle = FontStyle.Normal;
                data.runnerCount.fontStyle = FontStyle.Normal;

                data.runnerName.text = "";
                data.runnerName.color = Color.black;

                data.runnerCount.text = "0";

                data.background.color = greenColor;
                data.runnerCount.color = textGreenColor;
                data.icon.sprite = alienIcon;
            }
            else
            {
                if (topRunners[i].Key.gameObject.GetComponent<Movement>())
                {
                    data.runnerName.fontStyle = FontStyle.Bold;
                    data.runnerCount.fontStyle = FontStyle.Bold;
                }
                else
                {
                    data.runnerName.fontStyle = FontStyle.Normal;
                    data.runnerCount.fontStyle = FontStyle.Normal;
                }
                if (topRunners[i].Key.IsHuman)
                {
                    data.background.color = orangeColor;
                    data.runnerCount.color = textOrangeColor;
                    data.icon.sprite = humanIcon;
                }
                else
                {
                    data.background.color = greenColor;
                    data.runnerCount.color = textGreenColor;
                    data.icon.sprite = alienIcon;
                }

                data.runnerName.text = topRunners[i].Key.nameInTop;
                data.runnerName.color = topRunners[i].Key._colorSquad;
                data.runnerCount.text = topRunners[i].Value + "";
            }
        }
    }

    private int FindRunnerIndex(AddRuners runner)
    {
        for(int i = 0; i < topRunners.Count; i++)
        {
            if(runner == topRunners[i].Key)
            {
                return i;
            }
        }

        return -1;
    }
}
