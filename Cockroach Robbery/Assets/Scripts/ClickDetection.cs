using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickDetection : MonoBehaviour
{
    private Coroutine clickDetectionCoroutine;

    [SerializeField] private LayerMask _wallsLayer;

    private void Start()
    {
        CocroachesCountControl.Instance.LevelLose += EndGame;
        WavesController.Instance.LevelWin += EndGame;
        GameController.Instance.StartLevel += StartGame;

        StartGame();
    }

    private void OnDestroy()
    {
        CocroachesCountControl.Instance.LevelLose -= EndGame;
        WavesController.Instance.LevelWin -= EndGame;
        GameController.Instance.StartLevel -= StartGame;
    }

    private void StartGame()
    {
        clickDetectionCoroutine = StartCoroutine(DetectClick());
    }

    private void EndGame()
    {
        StopCoroutine(clickDetectionCoroutine);
    }

    private IEnumerator DetectClick()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.01f);

            if (Input.GetMouseButtonDown(0) || ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began)))
            {
                Ray raycast;
                if (Input.GetMouseButtonDown(0))
                {
                    raycast = Camera.main.ScreenPointToRay(Input.mousePosition);
                }
                else
                {
                    raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                }


                RaycastHit raycastHit;
                if (Physics.Raycast(raycast, out raycastHit,1000f ,~_wallsLayer))
                {
                    if (raycastHit.collider.TryGetComponent<CocroachGrowth>(out CocroachGrowth cocroach))
                    {
                        if (cocroach.IsAlive)
                        {
                            cocroach.StopBeforeDie();

                            Gun.Instance.Shoot(cocroach.transform);
                        }
                    }
                }
            }
        }

    }
}
