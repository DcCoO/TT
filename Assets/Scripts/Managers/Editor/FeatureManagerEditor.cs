#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(FeatureManager))]
public class FeatureManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Fill Feature References"))
        {
            FeatureManager featureManager = (FeatureManager)target;
            featureManager.SetFeatures(GetFeatures());
        }
    }

    private Feature[] GetFeatures()
    {
        string[] guids = AssetDatabase.FindAssets($"t:{nameof(Feature)}");

        Feature[] features = new Feature[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            features[i] = AssetDatabase.LoadAssetAtPath<Feature>(path);
        }

        return features;
    }
}

#endif