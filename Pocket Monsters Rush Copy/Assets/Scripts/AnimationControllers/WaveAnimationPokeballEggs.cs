using System.Collections;
using UnityEngine;

public class WaveAnimationPokeballEggs : MonoBehaviour
{
    [SerializeField] private float _delay = 0.2f;

    private void Awake()
    {
        StartCoroutine(StartWaveGroupAnimationCoroutine());
    }

    private IEnumerator StartWaveGroupAnimationCoroutine()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            Transform row = transform.GetChild(i);

            foreach(Animator animator in row.GetComponentsInChildren<Animator>())
            {
                animator.enabled = true;
            }

            yield return new WaitForSeconds(_delay);
        }
    }
}