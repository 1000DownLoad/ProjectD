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

    // 타입과 엑셀 테이블 명칭을 맞춰주세요.
    public class ItemDataTable : DataTableBase<ItemDataTable>, IDataTable
    {
        #region CONST

         public int CONST_FATIGUE_RECOVER_TIME { get; private set; }

        #endregion


        // 한번 데이터가 쓰이면 변경할수 없다.
        // Clear 금지.

        // item 시트 데이터.
        private Dictionary<int, ItemData> m_common_item_data;

        public void LoadDataTable()
        {
            LoadCommonDataTable();
        }

        public void LoadCommonDataTable()
        {
            WorkBook book = GetCommonRowData();

            // CONST 를 가장먼저 세팅합니다.
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

            // Capacity 를 지정하여 딕셔너리 생성.
            m_common_item_data = new Dictionary<int, ItemData>(book.GetRowCount("ITEM"));

            // 디버깅이 쉽도록 람다말고 함수를 넣어주세요.
            book.Foreach("ITEM", ParseCommonItemRowData);

            // 필요에 따라 추가해주세요.
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
            // 여기서 에러가 발생한다면 엑셀 쓰레기값을 확인해보자.
            var name = in_row[0].String;
            switch (name)
            {
                case "CONST_FATIGUE_RECOVER_TIME":
                {
                    CONST_FATIGUE_RECOVER_TIME = in_row[1].Integer;
                    break;
                }
                default:
                    // 일치하는게 없다면 데이터 오류!
                    Application.Quit();
                    break;
            }
        }

        public void ParseCommonItemRowData(Row in_row) 
        {
            // 여기서 에러가 발생한다면 엑셀 쓰레기값을 확인해보자.
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


