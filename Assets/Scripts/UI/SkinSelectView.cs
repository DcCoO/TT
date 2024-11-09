using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SkinSelectView : MonoBehaviour
{

    [SerializeField] private Transform m_SkinsParent;
    [SerializeField] private Transform m_BottomLeft;
    [SerializeField] private Transform m_TopRight;
    [SerializeField] private List<SkinDataList> m_Skins;
    [SerializeField] private Vector2Int m_Elements;
    [SerializeField] private SkinOption m_SkinOptionPrefab;
    [SerializeField] private BrushMainMenu m_PreviewBrush;
    [SerializeField, Range(0f, 1f)] private float m_ScrollSpeed = 0.5f;

    private Transform m_LeftMostOption;
    private Transform m_RightMostOption;
    private float m_XBoundLeft;
    private float m_XBoundRight;
    private Vector3 m_MouseLastPosition;

    private Vector2Int m_CurrentSkinIndex = Vector2Int.zero;

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (m_BottomLeft == null || m_TopRight == null) return;
        var center = (m_TopRight.position + m_BottomLeft.position) * 0.5f;
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(center, m_TopRight.position - m_BottomLeft.position);
        Gizmos.DrawSphere((m_TopRight.position + m_BottomLeft.position) * 0.5f, 0.5f);
        Vector2 size = m_TopRight.position - m_BottomLeft.position;
        var space = new Vector2(size.x / m_Elements.x, size.y / m_Elements.y);
        Gizmos.DrawWireCube(center, new Vector3(space.x, space.y, 0));

        Gizmos.color = Color.green;
        float originX = center.x - space.x * ((float)Mathf.Min(m_Skins[0].Count, m_Elements.x) - 1f) * 0.5f;
        float originY = center.y - space.y * ((float)Mathf.Min(m_Skins.Count, m_Elements.y) - 1f) * 0.5f;

        //print($"BBBBBBBBB {Mathf.Min(m_Skins[0].Count, m_Elements.x)} {Mathf.Min(m_Skins.Count, m_Elements.y)}");

        for (int i = 0; i < 2; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                Gizmos.DrawSphere(new Vector3(originX + j * space.x, originY + i * space.y, center.z), 0.3f);
            }
        }
    }
#endif

    private void LoadSkins()
    {
        if (m_Skins == null) return;
        if (m_Skins.Count == 0) return;

        Vector2 size = m_TopRight.position - m_BottomLeft.position;
        var space = new Vector2(size.x / m_Elements.x, size.y / m_Elements.y);

        var center = (m_TopRight.position + m_BottomLeft.position) * 0.5f;
        var z = m_SkinsParent.position.z;
        m_SkinsParent.position = new Vector3(center.x, center.y, z);

        float originX = center.x - space.x * (Mathf.Min(m_Skins[0].Count, m_Elements.x) - 1f) * 0.5f;
        float originY = center.y - space.y * (Mathf.Min(m_Skins.Count, m_Elements.y) - 1f) * 0.5f;

        for (int i = 0; i < m_Skins.Count; ++i) 
        {
            for (int j = 0; j < m_Skins[i].Count; ++j)
            {
                SkinData skin = m_Skins[i][j];
                var skinOption = Instantiate(m_SkinOptionPrefab, m_SkinsParent);
                skinOption.transform.position = new Vector3(originX + j * space.x, originY + i * space.y, z);
                skinOption.Setup(skin, this, new Vector2Int(i, j));

                    print(skinOption.transform.position);
                if (j == 0)
                {
                    m_LeftMostOption = skinOption.transform;
                    m_XBoundLeft = skinOption.transform.position.x;
                }
                else if (j == m_Elements.x - 1)
                {
                    m_XBoundRight = skinOption.transform.position.x;
                }
                else if (j == m_Skins[i].Count - 1)
                {
                    m_RightMostOption = skinOption.transform;
                }
            }            
        }

        LoadPreview(m_CurrentSkinIndex);
    }

    public void LoadPreview(Vector2Int skinIndex)
    {
        m_CurrentSkinIndex = skinIndex;
        m_PreviewBrush.Set(m_Skins[m_CurrentSkinIndex.x][m_CurrentSkinIndex.y]);
        GameManager.Instance.m_SkinFromSkinSelect = m_Skins[m_CurrentSkinIndex.x][m_CurrentSkinIndex.y];
    }

    private void OnEnable()
    {
        LoadSkins();
    }

    private void OnDisable()
    {
        foreach (Transform child in m_SkinsParent)
        {
            Destroy(child.gameObject);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            m_MouseLastPosition = Input.mousePosition;
        }
        else if (Input.GetMouseButton(0))
        {
            var offset = Input.mousePosition - m_MouseLastPosition;
            offset.y = offset.z = 0;

            var originalPosition = m_SkinsParent.position;

            m_SkinsParent.position += offset * Time.deltaTime * m_ScrollSpeed;
            if (m_LeftMostOption.position.x > m_XBoundLeft)
            {
                m_SkinsParent.position = originalPosition;
            }
            else if (m_RightMostOption.position.x < m_XBoundRight)
            {
                m_SkinsParent.position = originalPosition;
            }

            m_MouseLastPosition = Input.mousePosition;
        }
    }



}
