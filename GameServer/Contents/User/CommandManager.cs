using DataBase;
using DataTable;
using System;
using System.Collections.Generic;

public class CommandManager : TSingleton<CommandManager>
{
    private Dictionary<string, Action<long, string>> m_command_dic = new Dictionary<string, Action<long, string>>();

    public void Initialize()
    {
        InsertCommand("/addres", GM_COMMAND_ADD_RESOURCE);
    }

    public void InvokeCommand(string in_command_key, long in_user_id, string in_command)
    {
        if (m_command_dic.TryGetValue(in_command_key, out var out_action))
            out_action.Invoke(in_user_id, in_command);
    }

    private bool InsertCommand(string in_command_key, Action<long, string> in_action)
    {
        if (m_command_dic.ContainsKey(in_command_key))
            return false;

        m_command_dic.Add(in_command_key, in_action);

        return true;
    }

    private void GM_COMMAND_ADD_RESOURCE(long in_user_id, string in_command)
    {
        var user = UserManager.Instance.GetUser(in_user_id);
        if (user == null)
            return;

        // 문자열 분리
        string[] split_str = in_command.Split(' ');
        if (split_str.Length != 3)
            return;

        var resource_type = (ResourceType)(long.Parse(split_str[1]));
        var count = long.Parse(split_str[2]);

        Dictionary<string, object> data = new Dictionary<string, object>();
        data.Add(resource_type.ToString(), count);

        // 자원 추가
        UserResourceManager.Instance.InsertResource(in_user_id, resource_type, count);

        // DB 갱신
        UserResourceManager.Instance.UpdateDB(in_user_id, resource_type);

        // TODO : 클라이언트 갱신
    }
}
