using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase;
using DataTable;
using ItemDatas = System.Collections.Generic.Dictionary<long, long>;

class UserItemManager : TSingleton<UserItemManager>
{
    private ConcurrentDictionary<long, ItemDatas> m_user_item_data = new ConcurrentDictionary<long, ItemDatas>();


    public void InsertItem(long in_user_id, long in_item_index, long in_count)
    {
        if (in_count == 0)
        {
            // 에러로그
            return;
        }

        var item_data = GetItemDatas(in_user_id);
        if(item_data == null)
        {
            var new_datas = new Dictionary<long, long>();
            new_datas.Add(in_item_index, in_count);

            m_user_item_data.TryAdd(in_user_id, new_datas);
        }
        else
        {
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
    }

    public void CreateUserInitData(long in_user_id)
    {
        InsertItem(in_user_id, 1, 1);
        InsertItem(in_user_id, 2, 1);
        UpdateDB(in_user_id);
    }

    public ItemDatas GetItemDatas(long in_user_id)
    {
        if (m_user_item_data.TryGetValue(in_user_id, out var item_datas))
        {
            return item_datas;
        }
        else
        {
            return null;
        }
    }

    public long GetItemCount(long in_user_id, long in_item_index)
    {
        if (false == m_user_item_data.TryGetValue(in_user_id, out var out_item_data))
            return 0;

        out_item_data.TryGetValue(in_item_index, out var out_count);
        return out_count;
    }

    public void UpdateDB(long in_user_id)
    {
        if (false == m_user_item_data.TryGetValue(in_user_id, out var out_item_data))
            return;

        Dictionary<string, object> data = new Dictionary<string, object>();
        foreach (var item_data in out_item_data)
        {
            data.Add(item_data.Key.ToString(), item_data.Value);
        }

        DatabaseManager.Instance.UpdateField("T_Item_Info", in_user_id.ToString(), data);
    }

    public void UpdateDB(long in_user_id, long in_item_index)
    {
        DatabaseManager.Instance.UpdateField("T_Item_Info", in_user_id.ToString(), in_item_index.ToString(), GetItemCount(in_user_id,in_item_index));
    }

    public void FetchDB(long in_user_id)
    {
        var item_data = DatabaseManager.Instance.GetDocumentData("T_Item_Info", in_user_id.ToString());
        if(item_data == null)
            return;

        Clear(in_user_id);

        foreach (var data in item_data)
        {
            if(false == Int16.TryParse(data.Key, out var index))
                   Environment.Exit(1);

            // 아이템 데이터가 존재하는지 확인하고 없으면 예외 처리
            var item = ItemDataTable.Instance.GetItemData(index);

            if(item == null)
                Environment.Exit(1);

            InsertItem(in_user_id, index, Convert.ToInt64(data.Value));
        }

    }

    public void Clear(long in_user_id)
    {
        m_user_item_data.TryRemove(in_user_id, out var value);
    }
}

