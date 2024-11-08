using UnityEngine;

public class DebugView : MonoBehaviour
{
    [SerializeField] private GameObject m_MainPanel;
    [SerializeField] private GameObject m_Overlay;

    [SerializeField] private Feature[] m_Features;
    [SerializeField] private DebugRow m_RowPrefab;

    public void Show()
    {
        if (m_MainPanel.activeSelf) return;
        if (GameManager.Instance.currentPhase != GamePhase.MAIN_MENU) return;

        m_MainPanel.SetActive(true);
        m_Overlay.SetActive(true);
        foreach (var feature in m_Features)
        {
            var row = Instantiate(m_RowPrefab, m_MainPanel.transform);
            row.Setup(feature);
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