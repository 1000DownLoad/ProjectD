using DataBase;
using DataTable;
using System;
using System.Collections.Generic;

namespace Account
{
    public class AccountInfo
    {
        public string account_id;
        public long user_id;
        public int level;
        public long cur_exp;
        public long cur_energy;
    }

    public class AccountManager : TSingleton<AccountManager>
    {
        private Dictionary<string, AccountInfo> m_account_dic = new Dictionary<string, AccountInfo>();

        public AccountInfo InsertAccount(string in_account_id)
        {
            var account_data = AccountDataTable.GetAccountTableData(1);
            if (account_data == null)
                return null;

            var new_account = new AccountInfo();
            new_account.account_id = in_account_id;
            new_account.user_id = GenerateUniqueUserID();
            new_account.level = 1;
            new_account.cur_exp = 0;
            new_account.cur_energy = account_data.max_energy;
            m_account_dic.Add(in_account_id, new_account);

            return new_account;
        }

        public AccountInfo GetAccount(string in_account_id)
        {
            if (m_account_dic.TryGetValue(in_account_id, out var out_account))
                return out_account;

            return null;
        }

        public AccountInfo GetAccountByUserID(long in_user_id)
        {
            foreach(var account in m_account_dic)
            {
                if (account.Value.user_id == in_user_id)
                    return account.Value;
            }

            return null;
        }

        public long GenerateUniqueUserID()
        {
            // 현재 시간을 10밀리초 단위로 표현하여 long으로 변환하여 고유한 번호를 생성
            long new_user_id = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            return new_user_id;
        }

        public void UpdateDataBase(string in_account_id)
        {
            var account = GetAccount(in_account_id);
            if (account == null)
                return;

            // DB 저장
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "AccountID", account.account_id },
                { "UserID", account.user_id },
                { "Level", account.level },
                { "CurExp", account.cur_exp },
                { "CurEnergy", account.cur_energy },
            };

            DataBaseManager.Instance.UpdateDataBase("T_Account_Info", in_account_id, data);
        }

        public bool LoadDataBase(string in_account_id)
        {
            var account = GetAccount(in_account_id);
            if (account == null)
                return false;

            var account_data = DataBaseManager.Instance.GetDocumentData("T_Account_Info", in_account_id);
            if (account_data != null)
            {
                account.account_id = account_data["AccountID"].ToString();
                account.user_id = long.Parse(account_data["UserID"].ToString());
                account.level = int.Parse(account_data["Level"].ToString());
                account.cur_exp = long.Parse(account_data["CurExp"].ToString());
                account.cur_energy = long.Parse(account_data["CurEnergy"].ToString());

                return true;
            }

            return false;
        }
    }
}
