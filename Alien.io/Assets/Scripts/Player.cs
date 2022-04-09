using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Spawner spawner;

    private void Awake()
    {
        spawner = GameObject.FindObjectOfType<Spawner>();
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
            spawner.ClearRuners(transform, GetComponent<AddRuners>().IsHuman);
            GameObject.FindObjectOfType<GameController>().EndGame2();
        }
        
    }

    private void AlivePlayer()
    {
        GetComponent<Movement>().isStopped = false;
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
                spawner.ClearRuners(transform, GetComponent<AddRuners>().IsHuman);
                GameObject.FindObjectOfType<GameController>().EndGame2();
                yield break;
            }
            yield return null;
        }
    }
}
