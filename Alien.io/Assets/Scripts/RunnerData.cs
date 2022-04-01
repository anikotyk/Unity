using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerData : MonoBehaviour
{
    [SerializeField] private Collider colliderRunner;
    [SerializeField] private SkinnedMeshRenderer skin;

    public Collider ColliderRunner=> colliderRunner;

    public IEnumerator CallEnableRunner()
    {
        skin.enabled = true;

        yield return new WaitForSeconds(0.1f);
        colliderRunner.enabled = true;
        
    }

    public void DisableRunner()
    {
        colliderRunner.enabled = false ;
        skin.enabled = false;
    }
}
