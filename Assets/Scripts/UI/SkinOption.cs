using UnityEngine;

public class SkinOption : MonoBehaviour
{
    [SerializeField] private Transform m_BrushParent;
    [SerializeField] private BrushMainMenu m_Brush;
    [SerializeField] private ParticleSystem m_SelectParticles;
    [SerializeField, Range(0, 0.5f)] private float m_ParticleScale;
    private SkinData m_SkinData;
    private SkinSelectView m_SkinSelectView;
    private Vector2Int m_SkinIndex;

    private float m_LastClickTime = 0;
    private readonly float m_ClickMaxDuration = 0.2f;

    public void Setup(SkinData skinData, SkinSelectView skinSelectView, Vector2Int skinIndex)
    {
        m_SkinData = skinData;
        m_SkinSelectView = skinSelectView;
        m_SkinIndex = skinIndex;
        m_BrushParent.gameObject.AddComponent<BrushRotation>();
        m_Brush.Set(m_SkinData);
    }

    public void OnMouseDown()
    {
        m_LastClickTime = Time.time;
    }

    public void OnMouseUp()
    {
        if (Time.time - m_LastClickTime < m_ClickMaxDuration)
        {
            m_SkinSelectView.LoadPreview(m_SkinIndex);
            var particle = Instantiate(m_SelectParticles, transform.position, Quaternion.identity);
            particle.transform.localScale *= m_ParticleScale;
            var main = particle.main;
            main.startColor = m_SkinData.Color.m_Colors[0];
            particle.Play();
            Destroy(particle.gameObject, 5);
        }
    }
}
