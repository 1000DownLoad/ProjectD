using System;
using System.Collections.Generic;
using Framework;

namespace User
{
    public class UserInfo
    {
        public long user_id;
        public bool is_init_data = false;
    }

    public class UserManager : TSingleton<UserManager>
    {
        private UserInfo m_user = new UserInfo();

        public void SetUserID(long in_user_id)
        {
            m_user.user_id = in_user_id;
        }

        public void SetUserInitData(bool in_user_init_data)
        {
            m_user.is_init_data = in_user_init_data;
        }

        public UserInfo GetUser()
        {
            return m_user;
        }
    }
}
