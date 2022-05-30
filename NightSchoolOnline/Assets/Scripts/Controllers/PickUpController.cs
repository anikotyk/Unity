using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpController : MonoBehaviour
{
    [SerializeField] private Transform _containerForPickedUpObjects;
    [SerializeField] private GameObject _throwButton;
    private GameObject _pickedUpObject;
    public GameObject PickedUpObject=> _pickedUpObject;

    public static PickUpController Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        _throwButton.SetActive(false);
    }

    public void PickUpButton()
    {
        RaycastController raycastController = GameObject.FindObjectOfType<RaycastController>();
        Ray ray = new Ray(raycastController.transform.position, raycastController.transform.forward);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
        {
            if (raycastHit.collider.gameObject.TryGetComponent<IPickUpObject>(out IPickUpObject pickUpObject))
            {
                pickUpObject.PickUp();
            }
        }
    }

    public void ThrowButton()
    {
        Throw();
        _throwButton.SetActive(false);
    }

    public void PickUp(GameObject obj)
    {
        if (_pickedUpObject != null)
        {
            Throw();
        }
        _pickedUpObject = obj;
        _pickedUpObject.transform.position = _containerForPickedUpObjects.position;
        _pickedUpObject.transform.parent = _containerForPickedUpObjects;
        _pickedUpObject.GetComponent<IPickUpObject>().SetPickedSettings();
        _throwButton.SetActive(true);
    }

    public void Throw()
    {
        _pickedUpObject.transform.parent = null;
        _pickedUpObject.GetComponent<PhysicObject>().PhysicsOn();
        _pickedUpObject = null;
        _throwButton.SetActive(false);
    }

    public void DestroyPickedObject()
    {
        if (_pickedUpObject == null)
        {
            return;
        }
        _pickedUpObject.GetComponent<PhysicObject>().DestroyObject();
        _pickedUpObject = null;
        _throwButton.SetActive(false);
    }
}
