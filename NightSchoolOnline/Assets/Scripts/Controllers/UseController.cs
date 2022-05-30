using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseController : MonoBehaviour
{
    public void UseButton()
    {
        RaycastController raycastController = GameObject.FindObjectOfType<RaycastController>();
        Ray ray = new Ray(raycastController.transform.position, raycastController.transform.forward);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.gameObject.TryGetComponent<IUseObject>(out IUseObject useObject))
            {
                if (PickUpController.Instance.PickedUpObject)
                {
                    useObject.Use(PickUpController.Instance.PickedUpObject.GetComponent<IPickUpObject>().ObjectName);
                }
                else
                {
                    useObject.Use();
                }
                
            }
        }
    }
}
