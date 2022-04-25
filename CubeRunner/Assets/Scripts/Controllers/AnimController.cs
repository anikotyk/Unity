using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    [SerializeField] private bool isJustOneAnim;

    public void Show()
    {
        gameObject.SetActive(true);
        if (isJustOneAnim)
        {
            gameObject.GetComponent<Animation>().Play();
        }
        else
        {
            gameObject.GetComponent<Animation>().Play("ButtonsOn");
        }
    }

    public void Hide()
    {
        gameObject.GetComponent<Animation>().Play("ButtonsOff");
    }
}
