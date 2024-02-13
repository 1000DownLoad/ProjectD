using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemDatas = System.Collections.Generic.Dictionary<long, long>;

public class UserItemManager : TSingleton<UserItemManager>
{
    private ItemDatas m_item_data = new ItemDatas();

    public void UpdateData(Dictionary<long, long> in_datas)
    {
        foreach (var data in in_datas)
        {
            if (m_item_data.TryGetValue(data.Key, out var old_count) == false)
                m_item_data.Add(data.Key, data.Value);
            else
            {
                m_item_data[data.Key] = data.Value;
            }
        }
    }

    public long GetItemCount(long in_item_index)
    {
        if (m_item_data.TryGetValue(in_item_index, out var out_count))
            return out_count;

        return 0;
    }

    public void Clear()
    {
        m_item_data.Clear();
    }
}
