using System;
using System.Collections.Generic;
using Framework;

public class UserInfo
{
    public string account_id;
    public long user_id;
    public int level;
    public long exp;
}

public class UserManager : TSingleton<UserManager>
{
    private UserInfo m_user = null;
    public bool m_is_init_data = false;
    public string m_auth_token = string.Empty;

    public void SetUser(UserInfo in_user_info)
    {
        m_user = in_user_info;
    }

    public void SetInitData(bool in_init_data)
    {
        m_is_init_data = in_init_data;
    }

    public void UpdateUser(string in_account_id, long in_user_id, int in_level, long in_exp)
    {
        m_user.account_id = in_account_id;
        m_user.user_id = in_user_id;
        m_user.level = in_level;
        m_user.exp = in_exp;
    }

    public UserInfo GetUser()
    {
        return m_user;
    }
}
