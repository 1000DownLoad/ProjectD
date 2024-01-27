using Framework.Event;

public class EVENT_USER_DATA_UPDATE : EventData
{
    public int level;
    public long cur_exp;
    public long cur_energy;

    public EVENT_USER_DATA_UPDATE(int in_level, long in_exp, long in_energy)
    {
        m_type = typeof(EVENT_USER_DATA_UPDATE);

        level = in_level;
        cur_exp = in_exp;
        cur_energy = in_energy;
    }
}