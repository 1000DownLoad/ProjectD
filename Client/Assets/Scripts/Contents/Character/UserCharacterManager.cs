using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

public class CharacterInfo
{
    public int character_kind;
    public CharacterType character_type; 
    public int level;
    public long exp;
}

class UserCharacterManager : TSingleton<UserCharacterManager>
{
    private Dictionary<int, CharacterInfo> m_character_datas = new Dictionary<int, CharacterInfo>();

    public void UpdateData(Dictionary<int, CharacterInfo> in_datas)
    {
        foreach (var data in in_datas)
        {
            if (m_character_datas.TryGetValue(data.Key, out var out_data) == false)
                m_character_datas.Add(data.Key, data.Value);
            else
            {
                m_character_datas[data.Key] = data.Value;
            }
        }
    }

    public CharacterInfo GetCharacter(int in_character_kind)
    {
        m_character_datas.TryGetValue(in_character_kind, out var out_data);
        
        return out_data;
    }

    public void Clear()
    {
        m_character_datas.Clear();
    }
}