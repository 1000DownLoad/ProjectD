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

    // 타입과 엑셀 테이블 명칭을 맞춰주세요.
    public class ItemDataTable : DataTableBase<ItemDataTable>
    {
        // 한번 데이터가 쓰이면 변경할수 없다.
        // Clear 금지.
        private Dictionary<ItemType, ItemData> m_common_item_data;

        public void LoadCommonItemDataTable()
        {
            WorkBook book = GetCommonRowData();
            if (book.Contains("ITEM") == false)
            {
                Debug.LogError("ItemDataTable - ITEM sheet not found");
                Application.Quit();
            }
               

            // Capacity 를 지정하여 딕셔너리 생성.
            m_common_item_data = new Dictionary<ItemType, ItemData>(book.GetRowCount("ITEM"));

            // 디버깅이 쉽도록 람다말고 함수를 넣어주세요.
            book.Foreach("ITEM", PraseCommonItemRowData);
        }

        public ItemData GetCommonItemData(ItemType in_item_type)
        {
            m_common_item_data.TryGetValue(in_item_type, out var out_data);

            return out_data;
        }

        public void PraseCommonItemRowData(Row in_row) 
        {
            // 여기서 에러가 발생한다면 엑셀 쓰레기값을 확인해보자.
            var item_data = new ItemData();
            item_data.item_type = (ItemType)in_row[0].Integer;
            item_data.name = in_row[1].String;
            item_data.sprite_name = in_row[2].String;

            m_common_item_data.Add(item_data.item_type, item_data);
        }

    } 
}


