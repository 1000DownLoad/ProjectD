using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase;
using DataTable;
using ItemData = System.Collections.Generic.Dictionary<long, long>;

class UserItemManager : TSingleton<UserItemManager>
{
    private ConcurrentDictionary<long, ItemData> m_user_item_data = new ConcurrentDictionary<long, ItemData>();


    public void InsertItem(long in_user_id, long in_item_index, long in_count)
    {
        if (in_count == 0)
        {
            // 에러로그
            return;
        }

        var item_data = GetOrCreateUserItemData(in_user_id);
        if (item_data.ContainsKey(in_item_index))
        {
            item_data[in_item_index] += in_count;

            if (item_data[in_item_index] < 0)
                item_data[in_item_index] = 0;
        }
        else
        {
            if (in_count < 0)
                in_count = 0;

            item_data.Add(in_item_index, in_count);
        }

    }

    public void CreateUserInitData(long in_user_id)
    {
        InsertItem(in_user_id, 1, 1);
        InsertItem(in_user_id, 2, 1);
        UpdateDB(in_user_id);
    }


    public ItemData GetOrCreateUserItemData(long in_user_id)
    {
        if (m_user_item_data.TryGetValue(in_user_id, out var out_item_data))
            return out_item_data;

        ItemData new_item_data = new ItemData();
        bool ret = m_user_item_data.TryAdd(in_user_id, new_item_data);

        if (false == ret)
        {
            Console.WriteLine("CreateAndInsertUserItemData Fail");

            Environment.Exit(1);
        }

        return new_item_data;
    }

    public long GetItemCount(long in_user_id, long in_item_index)
    {
        if (false == m_user_item_data.TryGetValue(in_item_index, out var out_item_data))
            return 0;

        out_item_data.TryGetValue(in_item_index, out var out_count);
        return out_count;
    }

    public void UpdateDB(long in_user_id)
    {
        if (false == m_user_item_data.TryGetValue(in_user_id, out var out_item_data))
            return;

        Dictionary<string, object> data = new Dictionary<string, object>();
        foreach (var item_data in data)
        {
            data.Add(item_data.Key.ToString(), item_data.Value);
        }

        DataBaseManager.Instance.UpdateField("T_Item_Info", in_user_id.ToString(), data);
    }

    public void UpdateDB(long in_user_id, long in_item_index)
    {
        DataBaseManager.Instance.UpdateField("T_Item_Info", in_user_id.ToString(), in_item_index.ToString(), GetItemCount(in_user_id,in_item_index));
    }

    public void FetchDB(long in_user_id)
    {
        var item_data = DataBaseManager.Instance.GetDocumentData("T_Item_Info", in_user_id.ToString());
        if(item_data == null)
            return;

        Clear(in_user_id);

        foreach (var data in item_data)
        {
            if(false == Int16.TryParse(data.Key, out var index))
                   Environment.Exit(1);

            // 아이템 데이터가 존재하는지 확인하고 없으면 예외 처리
            var item = ItemDataTable.Instance.GetItemDataByIndex(index);

            if(item == null)
                Environment.Exit(1);

            InsertItem(in_user_id, index, Convert.ToInt64(data.Value));
        }

    }

    public void Clear(long in_user_id)
    {
        m_user_item_data.TryRemove(in_user_id, out var value);
    }

    public GS_USER_ITEM_FETCH_ACK CreateFetchProtocolStruct(long in_user_id)
    {
        GS_USER_ITEM_FETCH_ACK ret = new GS_USER_ITEM_FETCH_ACK();

        m_user_item_data.TryGetValue(in_user_id, out var item_data);

        ret.UserID = in_user_id;

        ret.FetchData = item_data;

        return ret;
    }
}

