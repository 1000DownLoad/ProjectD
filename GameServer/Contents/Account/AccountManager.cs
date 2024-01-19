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
        Dictionary<string, AccountInfo> m_account_dic = new Dictionary<string, AccountInfo>();

        public AccountInfo CreateAccount(string in_account_id)
        {
            var account_data = AccountDataTable.GetAccountTableData(1);
            if (account_data == null)
                return null;

            // 서버 저장
            var new_account = new AccountInfo();
            new_account.account_id = in_account_id;
            new_account.user_id = GenerateUniqueUserID();
            new_account.level = 1;
            new_account.cur_exp = 0;
            new_account.cur_energy = account_data.max_energy;
            m_account_dic.Add(in_account_id, new_account);

            // DB 저장
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "UserID", new_account.user_id },
                { "Level", new_account.level },
                { "CurExp", 0 },
                { "CurEnergy", new_account.cur_energy },
            };

            DataBaseManager.Instance.UpdateDataBase("T_Account_Info", in_account_id, data);

            return new_account;
        }

        public AccountInfo GetAccountByAccountID(string in_account_id)
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

        public void SetDataBaseData()
        {
            var collection_data = DataBaseManager.Instance.GetCollectionData("T_Account_Info");

            foreach (var document in collection_data)
            {
                if(document.Exists)
                {
                    var account = document.ToDictionary();

                    var new_account = new AccountInfo();
                    new_account.account_id = document.Id;
                    new_account.user_id = long.Parse(account["UserID"].ToString());
                    new_account.level = int.Parse(account["Level"].ToString());
                    new_account.cur_exp = long.Parse(account["CurExp"].ToString());
                    new_account.cur_energy = long.Parse(account["CurEnergy"].ToString());

                    m_account_dic.Add(new_account.account_id, new_account);
                }
            }
        }
    }
}
