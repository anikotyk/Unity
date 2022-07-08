using UnityEngine;

public class GateTrack : MonoBehaviour
{
    [SerializeField] private Pokemon _target;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<Runner>(out Runner runner))
        {
            runner.StartShooting(_target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.TryGetComponent<Runner>(out Runner runner))
        {
            runner.EndShooting();
        }
    }
}
