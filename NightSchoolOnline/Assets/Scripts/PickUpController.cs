using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField] private Transform _containerForPickedUpObjects;
    [SerializeField] private GameObject _throwButton;
    private GameObject _pickedUpObject;
    public GameObject PickedUpObject=> _pickedUpObject;

    public void PickUpButton()
    {
        RaycastController raycastController = GameObject.FindObjectOfType<RaycastController>();
        Ray ray = new Ray(raycastController.transform.position, raycastController.transform.forward);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.gameObject.TryGetComponent<PickUpObject>(out PickUpObject pickUpObject))
            {
                pickUpObject.PickUp();
            }
        }
    }

    public void ThrowButton()
    {
        Throw(_pickedUpObject);
        _throwButton.SetActive(false);
    }

    public void PickUp(GameObject obj)
    {
        _pickedUpObject = obj;
        _pickedUpObject.transform.position = _containerForPickedUpObjects.position;
        _pickedUpObject.transform.parent = _containerForPickedUpObjects;
        _pickedUpObject.transform.localRotation = _pickedUpObject.GetComponent<PickUpObject>().PickedRotation;
        _throwButton.SetActive(true);
    }

    public void Throw(GameObject obj)
    {
        _pickedUpObject.transform.parent = null;
        _pickedUpObject.GetComponent<PhysicsEvents>().PhysicsOnForAllPlayers();
        _pickedUpObject = null;
    }
}
