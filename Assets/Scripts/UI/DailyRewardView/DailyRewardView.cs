using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DailyRewardView : View<DailyRewardView>
{
    public static event EventHandler OnRevealDailyRewardEvent;
    public static event EventHandler OnClaimDailyRewardEvent;

    [SerializeField] private DailyRewardFeature _dailyRewardFeature;
    [SerializeField] private Button _claimButton;
    [SerializeField] private Transform _claimListItemParent;
    [SerializeField] private ClaimListItem _claimListItemPrefab;
    [SerializeField] private TMP_Text _coinsLabel;
    [SerializeField] private CoinIconPool _coinIconPool;
    [SerializeField] private Transform _coinsTarget;
    [Header("Events")]
    [SerializeField] private UnityEvent _onReveal;
    [SerializeField] private UnityEvent _onHide;
    
    private List<ClaimListItem> _claimListItems;
    private int _currentStreak;
    private bool _isClaimed;
    private NumberClassifier _numberClassifier;
    
    protected override void Awake()
    {
        base.Awake();
        
        _numberClassifier = new NumberClassifier(_dailyRewardFeature.DailyRewards);
        
        _claimListItems = new List<ClaimListItem>();
        for (var i = _dailyRewardFeature.DailyRewards.Count - 1; i >= 0 ; i--)
        {
            var dailyReward = _dailyRewardFeature.DailyRewards[i];
            var item = Instantiate(_claimListItemPrefab, _claimListItemParent);
            item.Init(i + 1, dailyReward.Value, _numberClassifier.GetGroup(dailyReward.Value));
            _claimListItems.Add(item);
        }

        DailyRewardManager.OnNewDailyStreakEvent += OnNewDailyStreak;
        CoinsManager.OnCoinsChangedEvent += OnCoinsChanged;
        _claimButton.onClick.AddListener(Claim);
    }

    protected override void OnDestroySpecific()
    {
        base.OnDestroySpecific();
        
        DailyRewardManager.OnNewDailyStreakEvent -= OnNewDailyStreak;
        CoinsManager.OnCoinsChangedEvent -= OnCoinsChanged;
        _claimButton.onClick.RemoveListener(Claim);
    }

    private void Reveal()
    {
        OnRevealDailyRewardEvent?.Invoke(this, EventArgs.Empty);
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
        var currentCoins = CoinsManager.Instance.Coins;
        var currentReward = _dailyRewardFeature.DailyRewards[_currentStreak - 1].Value;
        var currentListItem = _claimListItems[_claimListItems.Count - _currentStreak];
        
        currentListItem.SetClaimed(true);
        
        for (int i = 0; i < 30; i++)
        {
            var coin = _coinIconPool.GetCoin();
            coin.transform.position = currentListItem.GetPosition();
            coin.SetTarget(_coinsTarget);
            UpdateCoinsLabel(currentCoins += (currentReward / 30));
            
            yield return new WaitForSeconds(0.05f);
        }
        
        yield return new WaitForSeconds(2f);
        
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
        Debug.Log($"ProcessPreviousStreak: {_currentStreak}");
        for (var i = 0; i < _claimListItems.Count; i++)
        {
            var day = _claimListItems.Count - i;
            var item = _claimListItems[i];
            item.SetClaimed(_currentStreak > day);
        }
    }
    
    private void OnCoinsChanged(object sender, EventArgs e)
    {
        if (!(e is CoinsChangedEventArgs coinsChangedEventArgs)) return;
        
        UpdateCoinsLabel(coinsChangedEventArgs.CurrentCoins);
    }
    
    private void UpdateCoinsLabel(int coins)
    {
        _coinsLabel.SetText( $"{coins}");
    }
    
    
    private class NumberClassifier
    {
        private readonly List<int> _lowGroup;
        private readonly List<int> _midGroup;
        private readonly List<int> _highGroup;

        public NumberClassifier(List<DailyReward> rewards)
        {
            var numbers = rewards.Select(x => x.Value).ToList();
            
            numbers.Sort();
        
            var groupSize = numbers.Count / 3;
        
            _lowGroup = numbers.Take(groupSize).ToList();
            _midGroup = numbers.Skip(groupSize).Take(groupSize).ToList();
            _highGroup = numbers.Skip(2 * groupSize).ToList();
        }

        public int GetGroup(int number)
        {
            if (_lowGroup.Contains(number))
                return 1;
            if (_midGroup.Contains(number))
                return 2;
            if (_highGroup.Contains(number))
                return 3;
        
            return -1;
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