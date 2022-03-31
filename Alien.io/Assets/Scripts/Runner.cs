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
            if ((addRuners.IsHuman != transform.parent.parent.GetComponent<AddRuners>().IsHuman) && (transform.parent.parent.childCount >= addRuners.transform.childCount))
            {
                
                transform.parent.parent.GetComponent<AddRuners>().AddRunners(addRuners.transform.childCount);
                GameObject.FindObjectOfType<Spawner>().SpawnSquad(1, 3, addRuners.IsHuman);
                //GameObject.FindObjectOfType<GameController>().GetTopPlayers();
                if (addRuners.GetComponent<Movement>())
                {
                    GameObject.FindObjectOfType<Spawner>().ClearChildren(addRuners.transform);
                    GameObject.FindObjectOfType<GameController>().EndGame2();
                }
                else
                {
                    Destroy(addRuners.gameObject);
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
