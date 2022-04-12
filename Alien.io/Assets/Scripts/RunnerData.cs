using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunnerData : MonoBehaviour
{
    [SerializeField] private Collider _colliderRunner;
    [SerializeField] private SkinnedMeshRenderer _skin;

    public Collider ColliderRunner=> _colliderRunner;

    public IEnumerator CallEnableRunner()
    {
        _skin.enabled = true;

        yield return new WaitForSeconds(0.1f);
        _colliderRunner.enabled = true;
    }

    public void DisableRunner()
    {
        _colliderRunner.enabled = false ;
        _skin.enabled = false;
    }
}
