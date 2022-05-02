using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocroachChildbearing : MonoBehaviour
{
    private CocroachMovement movement;

    private bool isCanGiveBirth;

    private float timer;
    private float timeBeforeLabor;

    private void Awake()
    {
        movement = GetComponent<CocroachMovement>();
        timer = 0;
        timeBeforeLabor = Random.Range(5, 20);
    }

    public void BecomeAdult()
    {
        isCanGiveBirth = true;
    }

    public void CantGiveBirth()
    {
        isCanGiveBirth = false;
    }


    private void Update()
    {
        if (!isCanGiveBirth) { return; }
        timer += Time.deltaTime;
        if (timer >= timeBeforeLabor)
        {
            timer = 0;
            timeBeforeLabor = Random.Range(5, 20);
            GiveBirth();
        }
    }

    private void GiveBirth()
    {
        GetComponent<Collider>().enabled = false;
        movement.EndMovement();
        StartCoroutine(CreateChildCoroutine());
    }

    private IEnumerator CreateChildCoroutine()
    {
        int cnt = Random.Range(1, 3);
        yield return new WaitForSeconds(0.5f);
        for (int i=0; i<cnt; i++)
        {
            CocroachSpawner.Instance.SpawnCocroachAtPosWithDirection(transform.localPosition, -transform.forward);
            yield return new WaitForSeconds(1f);
        }

        yield return new WaitForSeconds(1f);
        movement.StartMovement();
        GetComponent<Collider>().enabled = true;
    }
}
