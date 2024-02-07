﻿using System;
using System.Collections.Generic;
using Framework;
using UnityEngine;

using ResourceDatas = System.Collections.Generic.Dictionary<ResourceType, long>;

class UserResourceManager : TSingleton<UserResourceManager>
{
    private bool m_is_initialized = false;
    private ResourceDatas m_resource_datas = new ResourceDatas();

    public void Fectch(Dictionary<ResourceType, long> in_data)
    {
        Clear();

        m_resource_datas = in_data;
        m_is_initialized = true;
    }

    public void InsertResource(ResourceType in_resource_type, long in_count)
    {
        if (m_resource_datas.TryGetValue(in_resource_type, out var old_count) == false)
            m_resource_datas.Add(in_resource_type, in_count);
        else
        {
            m_resource_datas[in_resource_type] += in_count;
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
        m_is_initialized = false;
    }

    public bool Isinitialized() 
    {
        return m_is_initialized;
    }
}