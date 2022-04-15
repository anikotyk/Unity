using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObstacles : MonoBehaviour
{
    public GameObject[] obstacles;
    public Coroutine spawner;
    public System.Random random;

    public float timePause = 0.8f;

    public int counterObstacles;
    private GameController gameController;
    public GameObject finishPrefab;
    public bool isSpawn = true;
    public bool finishexists = false;

    public void Start()
    {
        counterObstacles = 0;
        gameController = GameObject.FindObjectOfType<GameController>();
        random = new System.Random();
    }

    public void ChangeSpeedInAll(float speed)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<MoveObstacle>())
            {
                transform.GetChild(i).GetComponent<MoveObstacle>()._speed = speed;
                transform.GetChild(i).GetComponent<MoveObstacle>().StopMove();
                transform.GetChild(i).GetComponent<MoveObstacle>().StartMove();
            }
        }
    }

    public void DeleteAllObstacles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }

    public void StopAllObstacles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<MoveObstacle>())
            {
                 transform.GetChild(i).GetComponent<MoveObstacle>().enabled = false;
            }
        }
    }

    public void StartAllObstacles()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<MoveObstacle>())
            {
                transform.GetChild(i).GetComponent<MoveObstacle>().enabled = true;
                transform.GetChild(i).GetComponent<MoveObstacle>().StartMove();
            }
        }
    }

    public IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(timePause);
            Spawn();
        }
    }

    public void Spawn()
    {
        if (!isSpawn)
        {
            return;
        }
        if (counterObstacles >= gameController.lengthLevel && !finishexists)
        {
            isSpawn = false;
            finishexists = true;
            GameObject finish = Instantiate(finishPrefab, transform);
            
            finish.transform.localPosition = new Vector3(-30, finish.transform.localPosition.y, finish.transform.localPosition.z);
            finish.GetComponent<MoveObstacle>().StartMove();
            return;
        }
        int i = random.Next(0, obstacles.Length);
        GameObject newObstacle = Instantiate(obstacles[i], transform);
        newObstacle.transform.localPosition = new Vector3(-30, newObstacle.transform.localPosition.y, newObstacle.transform.localPosition.z);

        newObstacle.GetComponent<MoveObstacle>().StartMove();
        if (obstacles[i].name != "WindowGold")
        {
            counterObstacles++;
            gameController.progresspercentfloat += 100.0f / gameController.lengthLevel;
        }
        else
        {
            gameController.counterGoldWindow += 1;
        }
    }
}
