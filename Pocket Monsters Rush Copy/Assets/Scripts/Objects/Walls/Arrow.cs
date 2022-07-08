using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Arrow : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Color _color;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "MainEnemy")
        {
            _sprite.color = _color;
            GetComponent<Collider>().enabled = false;
        }
    }
}
