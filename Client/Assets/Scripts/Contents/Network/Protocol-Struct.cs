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
    public long CurExp;
    public long CurEnergy;
}

public class GS_USER_BASE_INFO_GET_REQ
{
    public long UserID;
}

public class GS_USER_BASE_INFO_GET_ACK
{
    public int Result;
}