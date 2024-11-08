using System;
using System.Collections.Generic;
using UnityEngine;

public class FeatureManager : SingletonMB<FeatureManager>
{
    public static event Action<EFeatureType, bool> OnChangeFeatureState;

    [SerializeField] private Feature[] _features;
    private Dictionary<EFeatureType, Feature> _featuresMap;

    private void Awake()
    {
        _featuresMap = new Dictionary<EFeatureType, Feature>();

        foreach (Feature feature in _features)
        {
            _featuresMap.Add(feature.FeatureType, feature);
        }
    } 

    public void SetFeatureState(EFeatureType featureType, bool state)
    {
        if (_featuresMap.TryGetValue(featureType, out Feature feature))
        {
            feature.IsEnabled = state;
            OnChangeFeatureState?.Invoke(featureType, state);
        }
    }

    public void SetFeatures(Feature[] features)
    {
        _features = features;
    }

}
