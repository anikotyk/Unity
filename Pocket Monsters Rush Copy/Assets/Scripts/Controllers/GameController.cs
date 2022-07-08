using Lofelt.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class GameController : MonoBehaviour
{
    [SerializeField] private GameObject[] activeBeforePlay;
    [SerializeField] private GameObject[] activeDuringPlay;
    
    [SerializeField] private GameObject _buttonStartBattle;

    [SerializeField] private GameObject _blackScreen;
    [SerializeField] private Animation _blackScreenAnimation;

    [SerializeField] private GameObject _startPanel;

    [SerializeField] private HapticPatterns.PresetType _vibrationsOnLose = HapticPatterns.PresetType.Failure;

    public event UnityAction GameStart;
    public event UnityAction BattleStart;

    public static GameController Instance { get; private set; }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        ShowWaitingForStart();
    }
    
    public void EndLevel()
    {
        WallsCoeffAnim.Instance.Hide();
        GemsController.Instance.IncreaseCount(LevelGemsController.Instance.GetCount());
        _blackScreen.SetActive(true);
        _blackScreenAnimation.Play();

        NextLevel();
    }

    public void LoseLevel()
    {
        StartCoroutine(LoseLevelCoroutine());
    }

    private IEnumerator LoseLevelCoroutine()
    {
        HapticPatterns.PlayPreset(_vibrationsOnLose);
        SoundsController.Instance.LevelLose();

        yield return new WaitForSeconds(3f);

        _blackScreen.SetActive(true);
        _blackScreenAnimation.Play();
        
        LevelController.Instance.LoadLevel();

        ShowWaitingForStart();
    }

    private void ShowWaitingForStart()
    {
        ActivateCurrentUIElements(false);
        StartCoroutine(WaitingForStart());
    }

    public void ShowStartBattleButton()
    {
        _buttonStartBattle.SetActive(true);
    }
    

    public void NextLevel()
    {
        LevelController.Instance.IncreaseCount(1);

        LevelController.Instance.LoadLevel();
        
        ShowWaitingForStart();
    }

    public void StartGame()
    {
        GameStart?.Invoke();

        ActivateCurrentUIElements(true);
    }

    public void StartBattle()
    {
        BattleStart?.Invoke();

        _buttonStartBattle.SetActive(false);
    }

    private IEnumerator WaitingForStart()
    {
        while (true)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began && IsOverPanel())
            {
                StartGame();
                yield break;
            }
            yield return null;
        }
    }

    private bool IsOverPanel()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current);
        pointerData.position = Input.GetTouch(0).position;

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        if (results.Count > 0)
        {
            if (results[0].gameObject == _startPanel && results.Count == 1)
            {
                return true;
            }
        }
        

        return false;
    }
    
    private void ActivateCurrentUIElements(bool isPlay)
    {
        if (isPlay)
        {
            foreach (GameObject obj in activeBeforePlay)
            {
                if (obj.TryGetComponent<Animator>(out Animator animator))
                {
                    animator.SetBool("IsHide", true);
                }
                else
                {
                    obj.SetActive(false);
                }
            }
            foreach (GameObject obj in activeDuringPlay)
            {
                obj.SetActive(true);
                if (obj.TryGetComponent<Animator>(out Animator animator))
                {
                    animator.SetBool("IsHide", false);
                }
            }
        }
        else
        {
            foreach (GameObject obj in activeDuringPlay)
            {
                if (obj.TryGetComponent<Animator>(out Animator animator))
                {
                    animator.SetBool("IsHide", true);
                }
                else
                {
                    obj.SetActive(false);
                }
            }

            foreach (GameObject obj in activeBeforePlay)
            {
                obj.SetActive(true);
                if (obj.TryGetComponent<Animator>(out Animator animator))
                {
                    animator.SetBool("IsHide", false);
                }
            }
        }
    }
}
