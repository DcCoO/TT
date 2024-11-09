using UnityEngine;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private ParticleSystem _particleSystem;
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.TryGetComponent(out CollisionHandler _))
        {
            _particleSystem.Play();
        }
    }
}
