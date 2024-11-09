using System;
using UnityEngine;

public class CoinsManager : SingletonMB<CoinsManager>
{
    public static event EventHandler OnCoinsChangedEvent;
    private const string CoinKey = "Coin";
    
    private int _coins;

    public int Coins
    {
        get => _coins;
        set
        {
            _coins = value;
            PlayerPrefs.SetInt(CoinKey, _coins);
            PlayerPrefs.Save();
        }
    }
    
    private void Awake()
    {
       _coins = PlayerPrefs.GetInt(CoinKey, 0);
       DailyRewardView.OnRevealDailyRewardEvent += OnRevealDailyReward;
    }

    private void OnRevealDailyReward(object sender, EventArgs e)
    {
        OnCoinsChangedEvent?.Invoke(this, new CoinsChangedEventArgs(Coins));
    }

    protected override void OnDestroySpecific()
    {
        base.OnDestroySpecific();
        
        DailyRewardView.OnRevealDailyRewardEvent -= OnRevealDailyReward;
    }
    
    public void AddCoins(int coins)
    {
        Coins += coins;
        OnCoinsChangedEvent?.Invoke(this, new CoinsChangedEventArgs(Coins));
    }
    
    public void RemoveCoins(int coins)
    {
        Coins -= coins;
        OnCoinsChangedEvent?.Invoke(this, new CoinsChangedEventArgs(Coins));
    }
    
    [ContextMenu("Clear Daily Rewards Data")]
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(CoinKey);
        PlayerPrefs.Save();
        Debug.Log("Coins PlayerPrefs cleared.");
    }

    [ContextMenu("Add 10 Coins")]
    public void ForceAddCoins()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("ForceAddCoins can only be called in play mode");
            return;
        }
        
        AddCoins(10);
        Debug.Log("Forced add coins: 10");
    }
}

public class CoinsChangedEventArgs : EventArgs
{
    public int CurrentCoins { get; }
    
    public CoinsChangedEventArgs(int currentCoins)
    {
        CurrentCoins = currentCoins;
    }
}