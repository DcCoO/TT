using UnityEngine;
using UnityEngine.UI;

public class DebugRow : MonoBehaviour
{
    [SerializeField] private Text m_Title;
    [SerializeField] private Toggle m_Toggle;

    private EFeatureType m_FeatureType;

    public void Setup(EFeatureType featureType)
    {
        m_FeatureType = featureType;
        m_Title.text = featureType.ToString();
        m_Toggle.isOn = FeatureManager.Instance.GetFeatureState(featureType);

        m_Toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool value)
    {
        FeatureManager.Instance.SetFeatureState(m_FeatureType, value);
    }

    private void OnDestroy()
    {
        m_Toggle.onValueChanged.RemoveListener(OnValueChanged);        
    }
}
