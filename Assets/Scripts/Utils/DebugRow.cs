using UnityEngine;
using UnityEngine.UI;

public class DebugRow : MonoBehaviour
{
    [SerializeField] private Text m_Title;
    [SerializeField] private Toggle m_Toggle;

    private Feature m_Feature;

    public void Setup(Feature feature)
    {
        m_Feature = feature;
        m_Title.text = feature.name;
        m_Toggle.isOn = feature;

        m_Toggle.onValueChanged.AddListener(OnValueChanged);
    }

    private void OnValueChanged(bool value)
    {
        m_Feature.IsEnabled = value;
    }

    private void OnDestroy()
    {
        m_Toggle.onValueChanged.RemoveListener(OnValueChanged);        
    }
}
