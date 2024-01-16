using System;
using System.Collections.Generic;
using Framework;

namespace Account
{
    public class AccountInfo
    {
        public string account_id;
        public long   user_id;
        public int    level;
        public long   cur_exp;
        public long   cur_energy;
    }

    public class AccountManager : TSingleton<AccountManager>
    {
        private AccountInfo m_account = new AccountInfo();

        public void SetAccount(AccountInfo in_account)
        {
            m_account = in_account;
        }

        public void UpdateAccount(string in_account_id, long in_user_id, int in_level, long in_cur_exp, long in_cur_energy)
        {
            m_account.account_id = in_account_id;
            m_account.user_id = in_user_id;
            m_account.level = in_level;
            m_account.cur_exp = in_cur_exp;
            m_account.cur_energy = in_cur_energy;
        }

        public AccountInfo GetAccount()
        {
            return m_account;
        }
    }
}
