using System;
using System.Collections.Generic;
using Framework;

class UserResourceData
{
    public UserResourceData(ResourceType in_resource_type, long in_count) { resource_type = in_resource_type; count = in_count; }

    public ResourceType resource_type;
    public long count;
}

class ResourceManager : TSingleton<ResourceManager>
{
    ResourceManager() { }

    private Dictionary<ResourceType, UserResourceData> m_resource_dic = new Dictionary<ResourceType, UserResourceData>();

    protected override void OnCreateSingleton()
    {
        InsertResource(ResourceType.GEM, 123456780);
        InsertResource(ResourceType.GOLD, 123456000);
        InsertResource(ResourceType.ENERGY, 150);
    }

    public void InsertResource(ResourceType in_resource_type, long in_count)
    {
        if(m_resource_dic.ContainsKey(in_resource_type))
        {
            m_resource_dic[in_resource_type].count += in_count;
        }
        else
        {
            m_resource_dic.Add(in_resource_type, new UserResourceData(in_resource_type, in_count));
        }
    }

    public UserResourceData GetResourceData(ResourceType in_resource_type)
    {
        m_resource_dic.TryGetValue(in_resource_type, out var out_data);

        return out_data;
    }

    public void Clear()
    {
        m_resource_dic.Clear();
    }
}
