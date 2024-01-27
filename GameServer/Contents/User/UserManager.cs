using DataBase;
using DataTable;
using System;
using System.Collections.Generic;

namespace User
{
    public class UserInfo
    {
        public string   account_id;
        public long     user_id;
        public int      level;
        public long     cur_exp;
        public long     cur_energy;
    }

    public class UserManager : TSingleton<UserManager>
    {
        private Dictionary<long, UserInfo> m_user_dic = new Dictionary<long, UserInfo>();

        public UserInfo CreateUser(string in_account_id)
        {
            var user = GetUserByAccountID(in_account_id);
            if (user != null)
                return user;

            var account_data = UserDataTable.Instance.GetAccountTableData(1);
            if (account_data == null)
                return null;

            // 서버 저장
            var new_user = new UserInfo();
            new_user.account_id = in_account_id;
            new_user.user_id = GenerateUniqueUserID();
            new_user.level = 1;
            new_user.cur_exp = 0;
            new_user.cur_energy = account_data.max_energy;
            InsertUser(new_user);

            return new_user;
        }

        public bool InsertUser(UserInfo in_user)
        {
            if (GetUser(in_user.user_id) != null)
                return false;

            m_user_dic.Add(in_user.user_id, in_user);

            return true;
        }

        public UserInfo GetUser(long in_user_id)
        {
            foreach (var user in m_user_dic)
            {
                if (user.Value.user_id == in_user_id)
                    return user.Value;
            }

            return null;
        }

        public UserInfo GetUserByAccountID(string in_account_id)
        {
            foreach (var user in m_user_dic)
            {
                if (user.Value.account_id == in_account_id)
                    return user.Value;
            }

            return null;
        }

        public void UpdateDB(string in_account_id)
        {
            var user = GetUserByAccountID(in_account_id);
            if (user == null)
                return;

            // DB 저장
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "AccountID", user.account_id },
                { "UserID", user.user_id },
                { "Level", user.level },
                { "CurExp", user.cur_exp },
                { "CurEnergy", user.cur_energy },
            };

            DataBaseManager.Instance.UpdateDataBase("T_User_Info", user.account_id, data);
        }

        public UserInfo GetDB(string in_account_id)
        {
            var user_data = DataBaseManager.Instance.GetDocumentData("T_User_Info", in_account_id);
            if(user_data != null)
            {
                var db_user_info = new UserInfo();
                db_user_info.account_id = user_data["AccountID"].ToString();
                db_user_info.user_id = long.Parse(user_data["UserID"].ToString());
                db_user_info.level = int.Parse(user_data["Level"].ToString());
                db_user_info.cur_exp = long.Parse(user_data["CurExp"].ToString());
                db_user_info.cur_energy = long.Parse(user_data["CurEnergy"].ToString());
                return db_user_info;
            }

            return null;
        }

        public long GenerateUniqueUserID()
        {
            // 현재 시간을 10밀리초 단위로 표현하여 long으로 변환하여 고유한 번호를 생성
            long new_user_id = (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalMilliseconds;

            return new_user_id;
        }
    }
}
