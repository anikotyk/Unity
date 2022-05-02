using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CocroachMovement))]
[RequireComponent(typeof(CocroachChildbearing))]
public class CocroachGrowth : MonoBehaviour
{
    [SerializeField] private Vector3 minSize;
    [SerializeField] private Vector3 maxSize;

    private CocroachMovement movement;
    private CocroachChildbearing childbearing;

    private bool isAlive;
    public bool IsAlive=>isAlive;
    private bool isAdult;

    private float timeOfGrowth;
    private float timeElapsed;

    private Vector3 startSize;

    private void Awake()
    {
        movement = GetComponent<CocroachMovement>();
        childbearing = GetComponent<CocroachChildbearing>();
        isAlive = true;
        isAdult = false;
        timeElapsed = 0;
        timeOfGrowth = Random.Range(5f, 15f);
    }

    private void Update()
    {
        if (!isAdult && isAlive)
        {
            Growth();
        }
    }

    public void GenerateRandomSize()
    {
        startSize = Vector3.Lerp(minSize, maxSize, Random.Range(0f, 1f));
        transform.localScale = startSize;
    }

    public void SetMinSize()
    {
        startSize = minSize;
        transform.localScale = minSize;
    }

    private void Growth()
    {
        if(Vector3.Distance(transform.localScale, maxSize) < 0.00001f)
        {
            transform.localScale = maxSize;
            isAdult = true;
            childbearing.BecomeAdult();
            return;
        }
        transform.localScale = Vector3.Lerp(startSize, maxSize, timeElapsed / timeOfGrowth);
        timeElapsed += Time.deltaTime;
    }

    public void StopBeforeDie()
    {
        movement.EndMovement();
        childbearing.CantGiveBirth();
        isAlive = false;
    }

    public void Die()
    {
       
        //Start Animation Of Death
        //wait until the end
        StartCoroutine(WaitAndDestroy());
    }

    private IEnumerator WaitAndDestroy()
    {
        yield return new WaitForSeconds(1f);
        Destroy(this.gameObject);
    }
    
}
