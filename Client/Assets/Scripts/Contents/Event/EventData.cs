using Framework.Event;

public class EVENT_USER_DATA_UPDATE : EventData
{
    public long user_id;

    public EVENT_USER_DATA_UPDATE(long in_user_id)
    {
        m_type = typeof(EVENT_USER_DATA_UPDATE);

        user_id = in_user_id;
    }
}