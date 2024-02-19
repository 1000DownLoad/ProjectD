using System.Collections.Generic;
using FlexFramework.Excel;
using Framework.DataTable;
using UnityEngine;

public enum ItemType
{
    NONE,
    Weapon,
    Armor,
    Shoes,
    Accessories,
}

namespace DataTable
{
    public class ItemData
    {
        public ItemType item_type;
        public int item_index;
        public string name;
        public string sprite_name;
        public string description;
    }

    // Ÿ�԰� ���� ���̺� ��Ī�� �����ּ���.
    public class ItemDataTable : DataTableBase<ItemDataTable>, IDataTable
    {
        #region CONST

         public int CONST_FATIGUE_RECOVER_TIME { get; private set; }

        #endregion


        // �ѹ� �����Ͱ� ���̸� �����Ҽ� ����.
        // Clear ����.

        // item ��Ʈ ������.
        private Dictionary<int, ItemData> m_common_item_data;

        public void LoadDataTable()
        {
            LoadCommonDataTable();
        }

        public void LoadCommonDataTable()
        {
            WorkBook book = GetCommonRowData();

            // CONST �� ������� �����մϴ�.
            if (book.Contains("CONST") == false)
            {
                Debug.LogError("ItemDataTable - CONST sheet not found");
                Application.Quit();
            }

            book.Foreach("CONST", ParseConstRowData);


            if (book.Contains("ITEM") == false)
            {
                Debug.LogError("ItemDataTable - ITEM sheet not found");
                Application.Quit();
            }  

            // Capacity �� �����Ͽ� ��ųʸ� ����.
            m_common_item_data = new Dictionary<int, ItemData>(book.GetRowCount("ITEM"));

            // ������� ������ ���ٸ��� �Լ��� �־��ּ���.
            book.Foreach("ITEM", ParseCommonItemRowData);

            // �ʿ信 ���� �߰����ּ���.
        }

        public ItemData GetCommonItemData(int in_item_index)
        {
            m_common_item_data.TryGetValue(in_item_index, out var out_data);

            return out_data;
        }

        public ItemType GetItemType(int in_item_index)
        {
            m_common_item_data.TryGetValue(in_item_index, out var out_data);

            return out_data.item_type;
        }

        public void ParseConstRowData(Row in_row) 
        {
            // ���⼭ ������ �߻��Ѵٸ� ���� �����Ⱚ�� Ȯ���غ���.
            var name = in_row[0].String;
            switch (name)
            {
                case "CONST_FATIGUE_RECOVER_TIME":
                {
                    CONST_FATIGUE_RECOVER_TIME = in_row[1].Integer;
                    break;
                }
                default:
                    // ��ġ�ϴ°� ���ٸ� ������ ����!
                    Application.Quit();
                    break;
            }
        }

        public void ParseCommonItemRowData(Row in_row) 
        {
            // ���⼭ ������ �߻��Ѵٸ� ���� �����Ⱚ�� Ȯ���غ���.
            var item_data = new ItemData();
            item_data.item_type = (ItemType)in_row[0].Integer;
            item_data.item_index = in_row[1].Integer;
            item_data.name = in_row[2].String;
            item_data.sprite_name = in_row[3].String;
            item_data.description = in_row[4].String;

            m_common_item_data.Add(item_data.item_index, item_data);
        }

    } 
}


