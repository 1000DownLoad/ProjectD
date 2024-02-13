using Framework.Event;
using System.Collections.Generic;

public class EVENT_USER_DATA_UPDATE : EventData
{
    public int level;
    public long cur_exp;

    public EVENT_USER_DATA_UPDATE(int in_level, long in_exp)
    {
        m_type = typeof(EVENT_USER_DATA_UPDATE);

        level = in_level;
        cur_exp = in_exp;
    }
}

public class EVENT_USER_RESOURCE_DATA_UPDATE : EventData
{
    public Dictionary<ResourceType, long> update_datas;

    public EVENT_USER_RESOURCE_DATA_UPDATE(Dictionary<ResourceType, long> in_datas)
    {
        m_type = typeof(EVENT_USER_RESOURCE_DATA_UPDATE);

        update_datas = in_datas;
    }
}

public class EVENT_USER_ITEM_DATA_UPDATE : EventData
{
    public Dictionary<long, long> update_datas;

    public EVENT_USER_ITEM_DATA_UPDATE(Dictionary<long, long> in_datas)
    {
        m_type = typeof(EVENT_USER_ITEM_DATA_UPDATE);

        update_datas = in_datas;
    }
}
