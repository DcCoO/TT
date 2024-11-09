using System;
using System.Collections.Generic;

[Serializable]
public class SkinDataList
{
    public List<SkinData> m_List = new List<SkinData>();

    public SkinData this[int key]
    {
        get => m_List[key];
        set => m_List[key] = value;        
    }

    public int Count => m_List.Count;
}
