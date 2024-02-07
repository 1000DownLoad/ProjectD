using FlexFramework.Excel;
using Framework.DataTable;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public enum ResourceType
{
    NONE,
    GEM,
    GOLD,
    FATIGUE,
}

namespace DataTable
{
    public class ResourceData
    {
        public ResourceType resource_type;
        public string name;
        public string sprite_name;
    }


    // 타입과 엑셀 테이블 명칭을 맞춰주세요.
    public class ResourceDataTable : DataTableBase<ResourceDataTable>, IDataTable
    {
        // 한번 데이터가 쓰이면 변경할수 없다.
        // Clear 금지.

        // resource 시트 데이터.
        private Dictionary<ResourceType, ResourceData> m_common_resource_data;

        public void LoadDataTable() 
        {
            LoadCommonDataTable();
        }

        public void LoadCommonDataTable()
        {
            WorkBook book = GetCommonRowData();
            if (book.Contains("RESOURCE") == false) 
            {
                Debug.LogError("ResourceDataTable - RESOURCE sheet not found");
                Application.Quit();
            }

            // Capacity 를 지정하여 딕셔너리 생성.
            m_common_resource_data = new Dictionary<ResourceType, ResourceData>(book.GetRowCount("RESOURCE"));

            // 디버깅이 쉽도록 람다말고 함수를 넣어주세요.
            book.Foreach("RESOURCE", ParseCommonResourceRowData);

            // 필요에 따라 추가해주세요.
        }

        public ResourceData GetCommonResourceData(ResourceType in_resource_type)
        {
            m_common_resource_data.TryGetValue(in_resource_type, out var out_data);

            return out_data;
        }

        public void ParseCommonResourceRowData(Row in_row) 
        {
            // 여기서 에러가 발생한다면 엑셀 쓰레기값을 확인해보자.
            var resource_data = new ResourceData();
            resource_data.resource_type = (ResourceType)in_row[0].Integer;
            resource_data.name = in_row[1].String;
            resource_data.sprite_name = in_row[2].String;

            m_common_resource_data.Add(resource_data.resource_type, resource_data);
        }
    }
}