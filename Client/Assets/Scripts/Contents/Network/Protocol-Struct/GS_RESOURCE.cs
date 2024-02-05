using System.Collections.Generic;

public class GS_USER_RESOURCE_FETCH_REQ
{
    public long UserID;
}

public class GS_USER_RESOURCE_FETCH_ACK
{
    public long UserID;

    public Dictionary<ResourceType, long> FetchDatas;
}