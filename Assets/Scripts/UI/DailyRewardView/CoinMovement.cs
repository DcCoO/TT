using UnityEngine;

public class CoinMovement : MonoBehaviour
{
    [SerializeField] private float _speed = 5f;

    private Transform _target;
    private CoinIconPool _pool;
    private bool _hasTarget;
    
    private void Update()
    {
        if (!_hasTarget) return;
        
        transform.position = Vector3.Lerp(transform.position, _target.position, _speed * Time.deltaTime);
        
        if (Vector3.Distance(transform.position, _target.position) < 0.1f)
        {
            _hasTarget = false;
            _target = null;
            _pool.ReturnCoin(this);
        }
    }
    
    public void Init(CoinIconPool pool)
    {
        _pool = pool;
    }
    
    public void SetTarget(Transform target)
    {
        _target = target;
        _hasTarget = true;
    }
}
