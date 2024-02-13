using System.Collections.Generic;

public class GS_USER_AUTH_TOKEN_REQ
{
    public string AccountID;
}

public class GS_USER_AUTH_TOKEN_ACK
{
    public int Result;
    public string AuthToken;
}

public class GS_USER_LOGIN_REQ
{
    public string AccountID;
}

public class GS_USER_LOGIN_ACK
{
    public int Result;
    public string AccountID;
    public long UserID;
    public int Level;
    public long Exp;
}

public class GS_USER_BASE_INFO_GET_REQ
{
    public long UserID;
}

public class GS_USER_BASE_INFO_GET_ACK
{
    public int Result;

    public Dictionary<ResourceType, long> ResourceDatas;
    public Dictionary<long, long> ItemDatas;
}

public class GS_USER_COMMAND_REQ
{
    public long UserID;
    public string Command;
}

public class GS_USER_COMMAND_ACK
{
    public long Result;
}