using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public Animator animator;

    public void SetRun(bool isRun)
    {
        animator.SetBool("IsRun", isRun);
    }

    public void RotateRunner(Vector3 direction, float rotationSpeed)
    {
        transform.parent.rotation = Quaternion.Slerp(
                transform.parent.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * rotationSpeed
        );
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.parent && collision.transform.parent.parent && collision.transform.parent.parent.TryGetComponent<AddRuners>(out AddRuners addRuners))
        {
            if (transform.parent && transform.parent.parent && transform.parent.parent.GetComponent<AddRuners>() && (addRuners.IsHuman != transform.parent.parent.GetComponent<AddRuners>().IsHuman) && (transform.parent.parent.childCount >= addRuners.transform.childCount))
            {
                transform.parent.parent.GetComponent<AddRuners>().AddRunners(addRuners.transform.childCount);

                if (addRuners.GetComponent<Movement>())
                {
                    //GameObject.FindObjectOfType<Spawner>().ClearChildren(addRuners.transform);
                    GameObject.FindObjectOfType<Spawner>().ClearRuners(addRuners.transform, addRuners.IsHuman);
                    GameObject.FindObjectOfType<GameController>().EndGame2();
                }
                else
                {
                    GameObject.FindObjectOfType<Spawner>().KillSquad(addRuners);
                }

                StartCoroutine(CallTop());
            }
        }
    }
    
    private IEnumerator CallTop()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject.FindObjectOfType<GameController>().GetTopPlayers();
    }

    
}

