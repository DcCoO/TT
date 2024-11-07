using UnityEngine;

[CreateAssetMenu(fileName = "Feature Settings", menuName = "ScriptableObjects/Feature Settings", order = 1)]
public class FeatureSettings : ScriptableObject
{
    public bool CollisionsEnabled;
    public bool SkinSelectEnabled;
    public bool DailyRewardsEnabled;
    public bool CustomFeatureEnabled;
}
