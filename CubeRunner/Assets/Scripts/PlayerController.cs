using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _speed = 35f;
    private Camera cam;
    private Vector3 _startPos;
    private Vector3 touchPos;
    public Rigidbody myRb;

    public GameObject fire;

    private float _speedrotating = 20f;

    public bool isPlaying = false;
    public bool isSpeeder = false;

    public AudioSource soundball;
    public GameController gameController;

    [SerializeField] private GameObject[] brokenBalls;
    [SerializeField] private GameObject newPlayer;
    public int indexShownBallState;

    

    private void Start()
    {
        
        indexShownBallState = 0;
        SetBall(0);
        fire.SetActive(false);
        isSpeeder = false;
        _startPos = transform.position;
        cam = Camera.main;
        myRb = GetComponent<Rigidbody>();
        gameController = GameObject.FindObjectOfType<GameController>();
    }

    public void SetBall(int index)
    {
        foreach(GameObject obj in brokenBalls)
        {
            obj.SetActive(false);
        }
        brokenBalls[index].SetActive(true);
    }

    public void OnGameStart()
    {
        foreach(GameObject obj in brokenBalls)
        {
            obj.GetComponent<PartOfBallData>().SetAwakeState();
        }
    }

    public void ShowDieAnim()
    {
        for (int i = 0; i < brokenBalls[indexShownBallState].transform.childCount; i++)
        {
            brokenBalls[indexShownBallState].transform.GetChild(i).GetComponent<Rigidbody>().isKinematic = false;
            brokenBalls[indexShownBallState].transform.GetChild(i).GetComponent<Rigidbody>().useGravity = true;
        }
    }

    public void OnSpeeder()
    {
        if (PlayerPrefs.GetInt("speeder") > 0 && isPlaying && !isSpeeder)
        {
            isSpeeder = true;
            fire.SetActive(true);
            gameController.speedMoveObstacles = PlayerPrefs.GetInt("speed") + 10f;
            gameController.spawner.ChangeSpeedInAll(PlayerPrefs.GetInt("speed") + 10f);
            gameController.ShowSpeed(gameController.speedMoveObstacles);
            PlayerPrefs.SetInt("speeder", PlayerPrefs.GetInt("speeder") - 1);
            gameController.speederText.text = "x"+PlayerPrefs.GetInt("speeder");
            StartCoroutine(speederwait(PlayerPrefs.GetInt("speederTime")));
        }
    }
    
    public IEnumerator speederwait(float time)
    {
        yield return new WaitForSeconds(time);
        
        gameController.spawner.ChangeSpeedInAll(PlayerPrefs.GetInt("speed"));
        gameController.speedMoveObstacles = PlayerPrefs.GetInt("speed");
        gameController.ShowSpeed(gameController.speedMoveObstacles);
        yield return new WaitForSeconds(2f);
        
        isSpeeder = false;
        fire.SetActive(false);
    }

    public IEnumerator MovingPlayer()
    {
        _speedrotating= PlayerPrefs.GetInt("speed");
        myRb.freezeRotation = false;
        myRb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY;
        while (true)
        {
#if UNITY_EDITOR
            touchPos = Input.mousePosition;
#elif UNITY_ANDROID
            touchPos = Input.touchCount > 0 ? Input.GetTouch(0).position : (Vector2)touchPos;
#endif
            
            touchPos.z = 5.0f;
            Vector3 screenToWorld = cam.ScreenToWorldPoint(touchPos);

            screenToWorld.x = 45;
            screenToWorld.y = transform.position.y;
            if (screenToWorld.z > 2.42f)
            {
                screenToWorld.z = 2.42f;
            }
            else if (screenToWorld.z < -2.42f)
            {
                screenToWorld.z = -2.42f;
            }

            Vector3 movement = Vector3.Lerp(myRb.position, screenToWorld, _speed * 0.01f);

            transform.Rotate(Vector3.forward * 0.01f * _speedrotating * 100);

            myRb.MovePosition(movement);
            //newPlayer.transform.position = transform.position;
            yield return new WaitForSeconds(0.01f);
        }


    }

    public IEnumerator MoveToStart()
    {
        while (true)
        {
            if (Vector3.Distance(transform.position, _startPos) < 0.01f)
            {
                myRb.freezeRotation=true;
                transform.position = _startPos;
            }
            else
            {
                float step = _speed * 0.01f; 
                transform.position = Vector3.MoveTowards(transform.position, _startPos, step);
            }

            yield return new WaitForSeconds(0.01f);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Obstacle" && !isSpeeder)
        {
            GameObject.FindObjectOfType<GameController>().MinusHealth();
        }
        if(collision.gameObject.tag == "Finish")
        {
            GameObject.FindObjectOfType<GameController>().LevelComplete();
        }
    }
}
