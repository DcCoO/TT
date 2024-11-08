using UnityEngine;

[CreateAssetMenu(fileName = "Feature", menuName = "ScriptableObjects/Feature", order = 1)]
public class Feature : ScriptableObject
{
    public EFeatureType FeatureType;
    public bool IsEnabled;

    public static implicit operator bool(Feature feature)
    {
        return feature.IsEnabled;
    }
}
