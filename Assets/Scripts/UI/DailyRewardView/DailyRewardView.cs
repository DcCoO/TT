using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DailyRewardView : View<DailyRewardView>
{
    public static event EventHandler OnClaimDailyRewardEvent;

    [SerializeField] private DailyRewardFeature _dailyRewardFeature;
    [SerializeField] private Button _claimButton;
    [SerializeField] private Transform _claimListItemParent;
    [SerializeField] private ClaimListItem _claimListItemPrefab;
    [Header("Events")]
    [SerializeField] private UnityEvent _onReveal;
    [SerializeField] private UnityEvent _onHide;
    
    private List<ClaimListItem> _claimListItems;
    private int _currentStreak;
    private bool _isClaimed;
    
    protected override void Awake()
    {
        base.Awake();
        
        _claimListItems = new List<ClaimListItem>();
        for (var i = _dailyRewardFeature.DailyRewards.Count - 1; i >= 0 ; i--)
        {
            var dailyReward = _dailyRewardFeature.DailyRewards[i];
            var item = Instantiate(_claimListItemPrefab, _claimListItemParent);
            item.Init(i + 1, dailyReward.Value);
            _claimListItems.Add(item);
        }

        DailyRewardManager.OnNewDailyStreakEvent += OnNewDailyStreak;
        _claimButton.onClick.AddListener(Claim);
    }

    private void Reveal()
    {
        _claimButton.interactable = true;
        _isClaimed = false;
        ProcessPreviousStreak();
        Transition(true);
        _onReveal.Invoke();
    }
    
    private void Hide()
    {
        Transition(false);
        _onHide.Invoke();
        OnClaimDailyRewardEvent?.Invoke(this, 
            new DailyClaimedEventArgs(_dailyRewardFeature.DailyRewards[_currentStreak - 1].Value));
    }

    private void Claim()
    {
        if (_isClaimed) return;
        
        _isClaimed = true;
        _claimButton.interactable = false;
        StartCoroutine(ProcessClaimSequence());
    }

    private IEnumerator ProcessClaimSequence()
    {
        yield return new WaitForSeconds(0.25f);

        _claimListItems[_claimListItems.Count - _currentStreak].SetClaimed(true);
        
        yield return new WaitForSeconds(3f);
        
        Hide();
    }
    
    private void OnNewDailyStreak(object sender, EventArgs e)
    {
        if (!(e is DailyStreakEventArgs dailyStreakEventArgs)) return;
        
        _currentStreak = dailyStreakEventArgs.CurrentStreak;
        Debug.Log($"New daily streak: {_currentStreak}");
        Reveal();
    }

    private void ProcessPreviousStreak()
    {
        for (var i = 0; i < _claimListItems.Count; i++)
        {
            var day = _claimListItems.Count - i;
            var item = _claimListItems[i];
            item.SetClaimed(_currentStreak > day);
        }
    }
}

public class DailyClaimedEventArgs : EventArgs
{
    public int RewardValue { get; }
    
    public DailyClaimedEventArgs(int rewardValue)
    {
        RewardValue = rewardValue;
    }
}