using System;
using UnityEngine;

public class DebugView : MonoBehaviour
{
    [SerializeField] private GameObject m_MainPanel;
    [SerializeField] private GameObject m_Overlay;

    [SerializeField] private DebugRow m_RowPrefab;

    public void Show()
    {
        if (m_MainPanel.activeSelf) return;
        if (GameManager.Instance.currentPhase != GamePhase.MAIN_MENU) return;
        if (FeatureManager.Instance == null) return;

        m_MainPanel.SetActive(true);
        m_Overlay.SetActive(true);
        foreach (EFeatureType featureType in Enum.GetValues(typeof(EFeatureType)))
        {
            var row = Instantiate(m_RowPrefab, m_MainPanel.transform);
            row.Setup(featureType);
        }
    }

    public void Hide()
    {
        foreach (Transform child in m_MainPanel.transform)
        {
            if (child.gameObject.GetComponent<DebugRow>() != null)
            {
                Destroy(child.gameObject);
            }
        }
        m_MainPanel.SetActive(false);
        m_Overlay.SetActive(false);
    }
}