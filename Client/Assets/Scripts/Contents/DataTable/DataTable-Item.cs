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

    // Ÿ�԰� ���� ���̺� ��Ī�� �����ּ���.
    public class ItemDataTable : DataTableBase<ItemDataTable>
    {
        // �ѹ� �����Ͱ� ���̸� �����Ҽ� ����.
        // Clear ����.
        private Dictionary<ItemType, ItemData> m_common_item_data;

        public void LoadCommonItemDataTable()
        {
            WorkBook book = GetCommonRowData();
            if (book.Contains("ITEM") == false)
            {
                Debug.LogError("ItemDataTable - ITEM sheet not found");
                Application.Quit();
            }
               

            // Capacity �� �����Ͽ� ��ųʸ� ����.
            m_common_item_data = new Dictionary<ItemType, ItemData>(book.GetRowCount("ITEM"));

            // ������� ������ ���ٸ��� �Լ��� �־��ּ���.
            book.Foreach("ITEM", PraseCommonItemRowData);
        }

        public ItemData GetCommonItemData(ItemType in_item_type)
        {
            m_common_item_data.TryGetValue(in_item_type, out var out_data);

            return out_data;
        }

        public void PraseCommonItemRowData(Row in_row) 
        {
            // ���⼭ ������ �߻��Ѵٸ� ���� �����Ⱚ�� Ȯ���غ���.
            var item_data = new ItemData();
            item_data.item_type = (ItemType)in_row[0].Integer;
            item_data.name = in_row[1].String;
            item_data.sprite_name = in_row[2].String;

            m_common_item_data.Add(item_data.item_type, item_data);
        }

    } 
}


