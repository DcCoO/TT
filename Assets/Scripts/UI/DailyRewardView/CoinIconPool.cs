using System.Collections.Generic;
using UnityEngine;

public class CoinIconPool : MonoBehaviour
{
    [SerializeField] private CoinMovement _coinPrefab;
    [SerializeField] private Transform _coinsContainer;
    [SerializeField] private int _poolSize = 30;

    private readonly Queue<CoinMovement> _pool = new Queue<CoinMovement>();

    private void Start()
    {
        for (int i = 0; i < _poolSize; i++)
        {
            var coin = Instantiate(_coinPrefab, _coinsContainer);
            coin.Init(this);
            coin.gameObject.SetActive(false);
            _pool.Enqueue(coin);
        }
    }

    public CoinMovement GetCoin()
    {
        if (_pool.Count > 0)
        {
            var coin = _pool.Dequeue();
            coin.gameObject.SetActive(true);
            return coin;
        }
        else
        {
            var coin = Instantiate(_coinPrefab, _coinsContainer);
            coin.Init(this);
            return coin;
        }
    }

    public void ReturnCoin(CoinMovement coin)
    {
        coin.gameObject.SetActive(false);
        _pool.Enqueue(coin);
    }
}
