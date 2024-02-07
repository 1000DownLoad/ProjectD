using System.Collections.Generic;


public class GS_USER_ITEM_FETCH_REQ
{
    public long UserID;
}

public class GS_USER_ITEM_FETCH_ACK
{
    public long UserID;
    public Dictionary<long, long> FetchData;
}
