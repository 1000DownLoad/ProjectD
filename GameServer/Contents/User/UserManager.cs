using DataBase;
using System;
using System.Collections.Generic;

namespace User
{
    public class UserInfo
    {
        public long user_id;
    }

    public class UserManager : TSingleton<UserManager>
    {
        private Dictionary<long, UserInfo> m_user_dic = new Dictionary<long, UserInfo>();

        public UserInfo InsertUser(long in_user_id)
        {
            // 서버 저장
            var new_user = new UserInfo();
            new_user.user_id = in_user_id;
            m_user_dic.Add(in_user_id, new_user);

            return new_user;
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

        public void UpdateDataBase(long in_user_id)
        {
            var user = GetUser(in_user_id);
            if (user == null)
                return;

            // DB 저장
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                { "UserID", user.user_id },
            };

            DataBaseManager.Instance.UpdateDataBase("T_User_Info", in_user_id.ToString(), data);
        }

        public bool LoadDataBaseData(long in_user_id)
        {
            var user = GetUser(in_user_id);
            if (user == null)
                return false;

            var user_data = DataBaseManager.Instance.GetDocumentData("T_User_Info", in_user_id.ToString());
            if(user_data != null)
            {
                user.user_id = long.Parse(user_data["UserID"].ToString());
                return true;
            }

            return false;
        }
    }
}
