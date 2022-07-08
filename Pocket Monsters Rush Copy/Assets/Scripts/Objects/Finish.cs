using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Finish : MonoBehaviour
{
    private bool _isAlreadyFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (_isAlreadyFinished) { return; }

        if (other.TryGetComponent<Runner>(out Runner runner))
        {
            runner.OnFinishReached();
            _isAlreadyFinished = true;
            GetComponent<Collider>().enabled = false;
        }
    }
}
