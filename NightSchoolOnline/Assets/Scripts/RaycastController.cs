using UnityEngine;

public class RaycastController : MonoBehaviour
{
    [SerializeField] private GameObject _pickUpBtn;
    [SerializeField] private float _maxDistance = 5;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit))
        {
            float disctanceError = _maxDistance - raycastHit.distance;
            if (raycastHit.collider.gameObject.GetComponent<PickUpObject>() && disctanceError >= 0)
            {
                _pickUpBtn?.SetActive(true);
            }
            else
            {
                _pickUpBtn?.SetActive(false);
            }
        }
        else
        {
            _pickUpBtn?.SetActive(false);
        }
    }
}
