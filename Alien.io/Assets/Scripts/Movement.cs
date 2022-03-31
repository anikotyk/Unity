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

    private Transform targetRunner;

    private void Awake()
    {
        spawner = GameObject.FindObjectOfType<Spawner>();
        targetRunner = GetComponent<AddRuners>().targetRunner;
    }

    public void OnStartMoving()
    {
        maxPosX = spawner._spawnPosCenter.x + spawner._maxSpawnPos;
        minPosX = spawner._spawnPosCenter.x - spawner._maxSpawnPos;
        maxPosZ = spawner._spawnPosCenter.z + spawner._maxSpawnPos;
        minPosZ = spawner._spawnPosCenter.z - spawner._maxSpawnPos;
    }
   
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
        if (Input.touchCount > 0)
        {
            pointer_x = Input.touches[0].position.x;
            pointer_y = Input.touches[0].position.y;
            pointer_x -= Screen.width/2;
            pointer_y -= Screen.height/2;
        }
#endif

        direction = new Vector3(pointer_x, 0, pointer_y).normalized * Time.deltaTime * moveSpeed;
        targetRunner.Translate(direction.x, 0, direction.z, Space.World);
         
        // direction = MovePlayer();
        RotatePlayer(direction);

        if (targetRunner.position.x > maxPosX)
        {
            targetRunner.position = new Vector3(maxPosX, targetRunner.position.y, targetRunner.position.z);
        }else if (targetRunner.position.x < minPosX)
        {
            targetRunner.position = new Vector3(minPosX, targetRunner.position.y, targetRunner.position.z);
        }

        if (targetRunner.position.z > maxPosZ)
        {
            targetRunner.position = new Vector3(targetRunner.position.x, targetRunner.position.y, maxPosZ);
        }
        else if (targetRunner.position.z < minPosZ)
        {
            targetRunner.position = new Vector3(targetRunner.position.x, targetRunner.position.y, minPosZ);
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
