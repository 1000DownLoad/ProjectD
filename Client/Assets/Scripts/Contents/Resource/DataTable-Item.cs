using System.Collections;
using System.Collections.Generic;
using FlexFramework.Excel;
using Framework.DataTable;
using UnityEditor;
using UnityEngine;

namespace DataTable
{
    public class ItemData
    {
        public ItemType item_type;
        public string name;
        public string sprite_name;
    }


    public class ItemDataTable : DataTableBase<ItemDataTable>
    {
        private static Dictionary<ItemType, ItemData> m_common_item_data = new Dictionary<ItemType, ItemData>();

        public void LoadCommonItemDataTable()
        {
            m_common_item_data.Clear();

            WorkBook book = GetCommonRowData();

            var doc = book["ITEM"];

            for (int row = 1; row < doc.Rows.Count; row++)
            {
                var row_data = doc.Rows[row];

                var item_data = new ItemData();
                item_data.item_type = (ItemType)row_data[0].Integer;
                item_data.name = row_data[1].String;
                item_data.sprite_name = row_data[2].String;

                m_common_item_data.Add(item_data.item_type, item_data);
            }

        }

        public ItemData GetCommonItemData(ItemType in_item_type)
        {
            m_common_item_data.TryGetValue(in_item_type, out var out_data);

            return out_data;
        }

    } 
}


