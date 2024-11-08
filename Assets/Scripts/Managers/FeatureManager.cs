using System;
using System.Collections.Generic;
using UnityEngine;

public class FeatureManager : SingletonMB<FeatureManager>
{
    public static event Action<EFeatureType, bool> OnChangeFeatureState;

    [SerializeField] private Feature[] m_features;
    private Dictionary<EFeatureType, Feature> m_featuresMap;

    private void Awake()
    {
        m_featuresMap = new Dictionary<EFeatureType, Feature>();

        foreach (Feature feature in m_features)
        {
            m_featuresMap.Add(feature.FeatureType, feature);
        }
    } 

    public bool GetFeatureState(EFeatureType featureType)
    {
        return m_featuresMap.TryGetValue(featureType, out Feature feature) ? feature : false;
    }

    public void SetFeatureState(EFeatureType featureType, bool state)
    {
        if (m_featuresMap.TryGetValue(featureType, out Feature feature))
        {
            feature.IsEnabled = state;
            OnChangeFeatureState?.Invoke(featureType, state);
        }
    }

    public void SetFeatures(Feature[] features)
    {
        m_features = features;
    }

}
