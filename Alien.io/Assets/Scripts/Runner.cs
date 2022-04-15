using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Runner : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    public Animator animator=>_animator;

    [SerializeField] private Vector3 _startPos;
    private float _rotateTime=0.2f;

    public AddRuners addRunnersComponent;


    private void OnEnable()
    {
        transform.localPosition = _startPos;
    }

    public void SetRun(bool isRun)
    {
        _animator.SetBool("IsRun", isRun);
    }

    public void RotateRunner(Vector3 direction, float rotationSpeed)
    {
        /*transform.parent.rotation = Quaternion.Slerp(
                transform.parent.rotation,
                Quaternion.LookRotation(direction),
                Time.deltaTime * rotationSpeed
        );*/
        LeanTween.rotateLocal(transform.parent.gameObject, Quaternion.LookRotation(direction).eulerAngles, _rotateTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.TryGetComponent<Runner>(out Runner collisionRunner))
        {
            AddRuners addRuners = collisionRunner.addRunnersComponent;
            if (addRuners != null)
            {
                if (addRuners.transform == addRunnersComponent.transform)
                {
                    return;
                }
                if (addRunnersComponent != null && (addRunnersComponent.transform.childCount >= addRuners.transform.childCount))
                {
                    if ((addRunnersComponent.transform.childCount == addRuners.transform.childCount) && addRunnersComponent.transform.GetComponent<MovementNPC>())
                    {
                        return;
                    }

                    bool isWithAnimationDeath = true;

                    if (addRuners.IsHuman == addRunnersComponent.IsHuman)
                    {
                        addRunnersComponent.AddRunners(addRuners.transform.childCount);
                        if (addRunnersComponent.transform.GetComponent<Player>())
                        {
                            GameObject.FindObjectOfType<GameController>().ShowCountAddedRunners(transform.position, addRuners.transform.childCount);
                        }

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
    }
    
    private IEnumerator CallTop()
    {
        yield return new WaitForSeconds(0.5f);
        GameObject.FindObjectOfType<GameController>().GetTopPlayers();
    }

    public void Dead()
    {
        _animator.SetBool("IsDead", true);
    }

    public void Alive()
    {
        _animator.SetBool("IsDead", false);
    }
}

