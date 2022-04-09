using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    public Animator animator;
    [SerializeField] private Vector3 startPos;

    private void OnEnable()
    {
        transform.localPosition = startPos;
    }

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
            if(collision.transform.parent.parent == transform.parent.parent)
            {
                return;
            }
            if (transform.parent && transform.parent.parent && transform.parent.parent.GetComponent<AddRuners>() && (transform.parent.parent.childCount >= addRuners.transform.childCount))   
            {
                if((transform.parent.parent.childCount == addRuners.transform.childCount) && transform.parent.parent.GetComponent<MovementNPC>())
                {
                    return;
                }

                bool isWithAnimationDeath = true;

                if(addRuners.IsHuman == transform.parent.parent.GetComponent<AddRuners>().IsHuman)
                {
                    transform.parent.parent.GetComponent<AddRuners>().AddRunners(addRuners.transform.childCount);
                    isWithAnimationDeath = false;
                }
                

                if (addRuners.TryGetComponent<Player>(out Player player))
                {
                    player.KillPlayer(isWithAnimationDeath);
                    
                }
                else
                {
                    GameObject.FindObjectOfType<Spawner>().KillSquad(addRuners, isWithAnimationDeath);
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

    public void Dead()
    {
        animator.SetBool("IsDead", true);
    }

    public void Alive()
    {
        animator.SetBool("IsDead", false);
    }
}

