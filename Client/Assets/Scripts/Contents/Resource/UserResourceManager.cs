using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

using ResourceDatas = System.Collections.Generic.Dictionary<ResourceType, long>;

class UserResourceManager : TSingleton<UserResourceManager>
{
    private ResourceDatas m_resource_datas = new ResourceDatas();

    public void UpdateData(Dictionary<ResourceType, long> in_datas)
    {
        foreach(var data in in_datas)
        {
            if (m_resource_datas.TryGetValue(data.Key, out var old_count) == false)
                m_resource_datas.Add(data.Key, data.Value);
            else
            {
                m_resource_datas[data.Key] = data.Value;
            }
        }
    }

    public long GetResourceCount(ResourceType in_resource_type)
    {
        m_resource_datas.TryGetValue(in_resource_type, out var ret);
        return ret;
    }

    public void Clear()
    {
        m_resource_datas.Clear();
    }
}