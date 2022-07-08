using UnityEngine;

public class CameraTargetPoint : MonoBehaviour
{
    [SerializeField] private Transform _runner;
    [SerializeField] private float _coef;

    [SerializeField] private bool _isXAxis = true;

    private void Update()
    {
        if (_isXAxis)
        {
            transform.localPosition = new Vector3(_runner.localPosition.x * _coef, transform.localPosition.y, transform.localPosition.z);
        }
        else
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, _runner.position.z * _coef);
        }
    }
}
