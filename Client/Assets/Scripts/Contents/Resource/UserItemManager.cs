using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ItemData = System.Collections.Generic.Dictionary<ItemType, long>;


public class UserItemManager : TSingleton<UserItemManager>
{
    private ItemData m_item_data = new ItemData();

    public void InsertItem(ItemType in_item_type, long in_count)
    {
        if (m_item_data.TryGetValue(in_item_type, out var old_count) == false)
            m_item_data.Add(in_item_type, in_count);
        else
            m_item_data[in_item_type] += in_count;
    }

    public long GetItemCount(ItemType in_item_type)
    {
        m_item_data.TryGetValue(in_item_type, out var ret);
        return ret;
    }

    public void Clear()
    {
        m_item_data.Clear();
    }
}
