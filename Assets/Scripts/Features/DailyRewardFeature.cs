using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DailyRewardFeature", menuName = "ScriptableObjects/DailyRewardFeature", order = 2)]
public class DailyRewardFeature : Feature
{
    [SerializeField] private List<DailyReward> _dailyRewards;
    
    public List<DailyReward> DailyRewards => _dailyRewards;
}

[System.Serializable]
public class DailyReward
{
    public int Value;
}