using System;
using System.Collections.Generic;
using Framework;

class AccountManager : TSingleton<AccountManager>
{
    public int m_user_level = 0;
    public long m_user_exp = 0;

    protected override void OnCreateSingleton()
    {
        m_user_level = 5;
        m_user_exp = 200;
    }
}
