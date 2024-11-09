using System;
using System.Globalization;
using UnityEngine;

public class DailyRewardManager : SingletonMB<DailyRewardManager>
{
    public static event EventHandler OnNewDailyStreakEvent;
    private const string LastLoginDateKey = "LastLoginDate";
    private const string StreakKey = "Streak";
    
    private int _currentStreak = 1;

    private void Awake()
    {
        DailyRewardView.OnClaimDailyRewardEvent += OnClaimDailyReward;
    }

    private void Start()
    {
        CheckDailyReward();
    }

    private void OnDestroy()
    {
        DailyRewardView.OnClaimDailyRewardEvent -= OnClaimDailyReward;
    }

    private void CheckDailyReward()
    {
        var lastLoginDate = PlayerPrefs.GetString(LastLoginDateKey, string.Empty);

        if (DateTime.TryParse(lastLoginDate, out var lastLogin))
        {
            // If the last login date was before today, update the streak
            if (lastLogin.Date >= DateTime.Now.Date) return;
            
            // Reset the streak if the last login was before today
            if ((DateTime.Now - lastLogin).Days > 1)
            {
                _currentStreak = 1;
            }
            else
            {
                _currentStreak = PlayerPrefs.GetInt(StreakKey, 1) + 1;
            }

            OnNewDailyStreakEvent?.Invoke(this, new DailyStreakEventArgs(_currentStreak));
        }
        else
        {
            // First time playing or no previous record
            _currentStreak = 1;
            OnNewDailyStreakEvent?.Invoke(this, new DailyStreakEventArgs(_currentStreak));
        }
    }
    
    private void OnClaimDailyReward(object sender, EventArgs e)
    {
        if (!(e is DailyClaimedEventArgs dailyClaimedEventArgs)) return;
        
        var rewardAmount = dailyClaimedEventArgs.RewardValue;
        Debug.Log($"Claimed {rewardAmount} amount of daily reward");
        CoinsManager.Instance.AddCoins(rewardAmount);

        //Update the persistent data
        PlayerPrefs.SetString(LastLoginDateKey, DateTime.Now.ToString(CultureInfo.InvariantCulture));
        PlayerPrefs.SetInt(StreakKey, _currentStreak);
        PlayerPrefs.Save();
    }
    
    [ContextMenu("Clear Daily Rewards Data")]
    public void ClearPlayerPrefs()
    {
        PlayerPrefs.DeleteKey(LastLoginDateKey);
        PlayerPrefs.DeleteKey(StreakKey);
        PlayerPrefs.Save();
        Debug.Log("DailyRewards PlayerPrefs cleared.");
    }

    [ContextMenu("Force New Daily Streak")]
    public void ForceNewDailyStreak()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("ForceNewDailyStreak can only be called in play mode");
            return;
        }
        
        _currentStreak++;
        OnNewDailyStreakEvent?.Invoke(this, new DailyStreakEventArgs(_currentStreak));
        Debug.Log($"Forced new daily streak: {_currentStreak}");
    }
}

public class DailyStreakEventArgs : EventArgs
{
    public int CurrentStreak { get; }
    
    public DailyStreakEventArgs(int currentStreak)
    {
        CurrentStreak = currentStreak;
    }
}