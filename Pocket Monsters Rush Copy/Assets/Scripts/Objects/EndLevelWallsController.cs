using UnityEngine;

public class EndLevelWallsController : MonoBehaviour
{
    [SerializeField] private Transform[] _brickWalls;
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider _platformCollider;

    public void ShowWalls()
    {
        if (_animator)
        {
            _animator.enabled = true;
        }
        
    }

    private void OnWallsShown()
    {
        _platformCollider.enabled = true;

        SleepRigidbody();
    }
    
    private void SleepRigidbody()
    {
        foreach (Transform wall in _brickWalls)
        {
            for (int i = 0; i < wall.childCount; i++)
            {
                Transform row = wall.GetChild(i);
                foreach(Rigidbody rb in row.GetComponentsInChildren<Rigidbody>())
                {
                    rb.isKinematic = false;
                    rb.Sleep();
                }
            }
        }
    }
}
