using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement : MonoBehaviour
{
    [SerializeField] private float speed=20f;
    [SerializeField] private float speedHorizontal = 20f;
    [SerializeField] private Vector3 direction;
    [SerializeField] private Transform finish;
    [SerializeField] private GameObject[] brokenBalls;

    private bool isRunning = false;
    private Vector3 posStart;
    private float xPosFinish;
    private Rigidbody rb;
    private Vector2 touchPos;
    private int indexShownBallState;
    private bool isSpeeder;

    private void Awake()
    {
        //speed = PlayerPrefs.GetInt("speed");
        indexShownBallState = 0;
        xPosFinish = finish.position.x;
        posStart = transform.localPosition;
        rb = GetComponent<Rigidbody>();
    }


    private void Start()
    {
        ResetPlayer();
    }

    private void FixedUpdate()
    {
        if (!isRunning) return;
        if ((direction.x<0 && transform.localPosition.x<xPosFinish) || (direction.x >= 0 && transform.localPosition.x > xPosFinish))
        {
            OnGameOver();
            GameObject.FindObjectOfType<GameController>().LevelComplete();
            return;
        }

#if UNITY_EDITOR
        touchPos = Input.mousePosition;
#elif UNITY_ANDROID
        touchPos = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)touchPos;
#endif
        touchPos -= new Vector2(Screen.width / 2, Screen.height / 2);
        
        touchPos = touchPos.normalized;

        Vector3 forwardMove = direction * speed * Time.fixedDeltaTime;
        Vector3 horizontalMove = new Vector3(0, 0, touchPos.x)* speedHorizontal * Time.fixedDeltaTime;
        //rb.AddForce((forwardMove + horizontalMove), ForceMode.VelocityChange);
        rb.velocity = (forwardMove + horizontalMove)*40;
    }
    
    public void StartLevel()
    {
        //ResetPlayer();
        isRunning = true;
    }

    public void ResetPlayer()
    {
        ResetPlayerAppearance();
        transform.localPosition = posStart;
    }

    public void ResetPlayerAppearance()
    {
        indexShownBallState = 0;
        OnGameStart();
        SetBall(indexShownBallState);
    }

    public void ContinueLevel()
    {
        OnGameStart();
       // isRunning = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" && !isSpeeder)
        {
            GameObject.FindObjectOfType<GameController>().MinusHealth();
        }
    }

    private void SetBall(int index)
    {
        foreach (GameObject obj in brokenBalls)
        {
            obj.SetActive(false);
        }
        brokenBalls[index].SetActive(true);
    }

    public void OnGameStart()
    {
        foreach (GameObject obj in brokenBalls)
        {
            obj.GetComponent<PartOfBallData>().SetAwakeState();
        }
    }

    public void ShowDieAnim()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        for (int i = 0; i < brokenBalls[indexShownBallState].transform.childCount; i++)
        {
            if(brokenBalls[indexShownBallState].transform.GetChild(i).TryGetComponent<Rigidbody>(out Rigidbody rigidbody))
            {
                rigidbody.isKinematic = false;
                rigidbody.useGravity = true;
            }
        }
    }

    public void MinusHealth()
    {
        indexShownBallState += 1;
        SetBall(indexShownBallState);
    }

    public void OnGameOver()
    {
        isRunning = false;

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.Sleep();
    }
}
