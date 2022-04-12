using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Spawner _spawner;

    private void Awake()
    {
        _spawner = GameObject.FindObjectOfType<Spawner>();
    }

    public void KillPlayer(bool isWithAnimation = false)
    {
        GetComponent<Movement>().isStopped = true;
        if (isWithAnimation)
        {
            GetComponent<AddRuners>().DeadAllRunners();
            StartCoroutine(SetActiveFalseAfterAnimation());
        }
        else
        {
            _spawner.ClearRuners(transform, GetComponent<AddRuners>().IsHuman);
            GameObject.FindObjectOfType<GameController>().EndGame();
        }
        
    }

    private IEnumerator SetActiveFalseAfterAnimation()
    {
        Animator animator = GetComponentInChildren<Runner>().animator;
        yield return null;
        while (true)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 1)
            {
                yield return new WaitForSeconds(0.3f);
                _spawner.ClearRuners(transform, GetComponent<AddRuners>().IsHuman);
                GameObject.FindObjectOfType<GameController>().EndGame();
                yield break;
            }
            yield return null;
        }
    }
}
