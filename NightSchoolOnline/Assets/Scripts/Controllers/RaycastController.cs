using UnityEngine;

public class RaycastController : MonoBehaviour
{
    [SerializeField] private GameObject _pickUpBtn;
    [SerializeField] private GameObject _useBtn;
    [SerializeField] private float _maxDistance = 5;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit raycastHit;

        _pickUpBtn?.SetActive(false);
        _useBtn?.SetActive(false);

        if (Physics.Raycast(ray, out raycastHit))
        {
            float disctanceError = _maxDistance - raycastHit.distance;
            
            if(disctanceError >= 0)
            {
                if (raycastHit.collider.gameObject.GetComponent<IPickUpObject>()!=null)
                {
                    _pickUpBtn?.SetActive(true);
                }
                
                if (raycastHit.collider.gameObject.TryGetComponent<IUseObject>(out IUseObject useObject))
                {
                    if (useObject.IsCanUse)
                    {
                        _useBtn?.SetActive(true);
                    }
                }
            }
        }
    }
}
