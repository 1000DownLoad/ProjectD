using DataBase;
using DataTable;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

public class UserInfo
{
    public string account_id;
    public long user_id;
    public int level;
    public long exp;
}

public class UserManager : TSingleton<UserManager>
{
    private ConcurrentDictionary<long, UserInfo> m_user_dic = new ConcurrentDictionary<long, UserInfo>();

    public UserInfo CreateUser(string in_account_id)
    {
        var user = GetUserByAccountID(in_account_id);
        if (user != null)
            return user;

        var user_level_data = UserDataTable.Instance.GetLevelTableData(1);
        if (user_level_data == null)
            return null;

        // 유저 생성
        var new_user = new UserInfo();
        new_user.account_id = in_account_id;
        new_user.user_id = GenerateUniqueUserID();
        new_user.level = 1;
        new_user.exp = 0;
        InsertUser(new_user);

        // 유저 초기 데이터 세팅
        CreateUserInitData(new_user.user_id);

        return new_user;
    }

    private void CreateUserInitData(long in_use_id)
    {
        // 유저 초기 필요 데이터 추가
        UserResourceManager.Instance.CreateUserInitData(in_use_id);
        UserItemManager.Instance.CreateUserInitData(in_use_id);
    }

    public bool InsertUser(UserInfo in_user)
    {
        if (GetUser(in_user.user_id) != null)
            return false;

        m_user_dic.TryAdd(in_user.user_id, in_user);

        return true;
    }

    public UserInfo GetUser(long in_user_id)
    {
        if (m_user_dic.TryGetValue(in_user_id, out var out_value))
            return out_value;

        return null;
    }

    public UserInfo GetUserByAccountID(string in_account_id)
    {
        var user_dic = m_user_dic.ToArray();

        foreach (var user in user_dic)
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
                { "Exp", user.exp },
            };

        DataBaseManager.Instance.UpdateField("T_User_Info", user.account_id, data);
    }

    public UserInfo FetchDB(string in_account_id)
    {
        var user_data = DataBaseManager.Instance.GetDocumentData("T_User_Info", in_account_id);
        if (user_data != null)
        {
            var db_user_info = new UserInfo();
            db_user_info.account_id = user_data["AccountID"].ToString();
            db_user_info.user_id = long.Parse(user_data["UserID"].ToString());
            db_user_info.level = int.Parse(user_data["Level"].ToString());
            db_user_info.exp = long.Parse(user_data["Exp"].ToString());
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

    public void UserDataFetchDB(long in_user_id)
    {
        var user = GetUser(in_user_id);
        if (user == null)
            return;

        UserResourceManager.Instance.FetchDB(in_user_id);
        UserItemManager.Instance.FetchDB(in_user_id);
    }

    public void UserDataClear(long in_user_id)
    {
        UserManager.Instance.Clear(in_user_id);
        UserResourceManager.Instance.Clear(in_user_id);
    }

    public void Clear(long in_user_id)
    {
        m_user_dic.TryRemove(in_user_id, out var value);
    }
}
