using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float rotationSpeed = 10f;
    private float maxPosX;
    private float minPosX;
    private float maxPosZ;
    private float minPosZ;
    private Vector3 direction;

    private Vector3 touchPos;

    [SerializeField] private Camera cam;


    private float pointer_x;
    private float pointer_y;

    public bool isStopped=false;

    private Spawner spawner;

    public Vector2 startPos;
    public Vector2 directionSwipe;

    private void Awake()
    {
        spawner = GameObject.FindObjectOfType<Spawner>();
        directionSwipe = new Vector3(0, 0.5f);
    }

    public void OnStartMoving()
    {
        maxPosX = spawner._spawnPosCenter.x + spawner._maxSpawnPos;
        minPosX = spawner._spawnPosCenter.x - spawner._maxSpawnPos;
        maxPosZ = spawner._spawnPosCenter.z + spawner._maxSpawnPos;
        minPosZ = spawner._spawnPosCenter.z - spawner._maxSpawnPos;
        isStopped = false;
    }

    //Attach this script to an empty GameObject
//Create some UI Text by going to Create>UI>Text.
//Drag this GameObject into the Text field of your GameObject�s Inspector window.

   
    

    void Update()
    {
        if (isStopped)
        {
            return;
        }

        

#if UNITY_EDITOR
        pointer_x = Input.mousePosition.x;
        pointer_y = Input.mousePosition.y;
        pointer_x -= Screen.width / 2;
        pointer_y -= Screen.height / 2;
#elif UNITY_ANDROID
       /* if (Input.touchCount > 0)
        {
            pointer_x = Input.touches[0].position.x;
            pointer_y = Input.touches[0].position.y;
            pointer_x -= Screen.width/2;
            pointer_y -= Screen.height/2;
        }*/

        
#endif
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    startPos = touch.position;
                    break;
                case TouchPhase.Moved:
                    directionSwipe = touch.position - startPos;
                    break;

                case TouchPhase.Ended:
                    break;
            }
        }

        //direction = new Vector3(pointer_x, 0, pointer_y).normalized * Time.deltaTime * moveSpeed;
        direction = new Vector3(directionSwipe.x, 0, directionSwipe.y).normalized * Time.deltaTime * moveSpeed;

        transform.Translate(direction.x, 0, direction.z, Space.World);
        
        // direction = MovePlayer();
        RotatePlayer(direction);

        if (transform.position.x > maxPosX)
        {
            transform.position = new Vector3(maxPosX, transform.position.y, transform.position.z);
        }else if (transform.position.x < minPosX)
        {
            transform.position = new Vector3(minPosX, transform.position.y, transform.position.z);
        }

        if (transform.position.z > maxPosZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, maxPosZ);
        }
        else if (transform.position.z < minPosZ)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, minPosZ);
        }
    }

    private Vector3 MovePlayer()
    {
        float xValue = Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed;
        float zValue = Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed;
        transform.Translate(xValue, 0, zValue, Space.World);
        return new Vector3(xValue, 0, zValue);
    }



    void RotatePlayer(Vector3 direction)
    {
        if (direction != Vector3.zero)
        {
            foreach(Runner runner in transform.GetComponentsInChildren<Runner>())
            {
                runner.RotateRunner(direction, rotationSpeed);
            }
        }
    }
}