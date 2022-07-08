using UnityEngine;

[RequireComponent(typeof(Collider))]
public abstract class Obstacle : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.TryGetComponent<Runner>(out Runner runner))
        {
            runner.Die();
            
            GameController.Instance.LoseLevel();
            GetComponent<Collider>().enabled = false;
        }
    }
}
