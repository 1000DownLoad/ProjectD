using FlexFramework.Excel;
using Framework.DataTable;
using System.Collections.Generic;
using System.IO;

public enum ResourceType
{
    NONE,
    GEM,
    GOLD,
    ENERGY,
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
    public class ResourceDataTable : DataTableBase<ResourceDataTable>
    {
        private static Dictionary<ResourceType, ResourceData> m_common_resource_data = new Dictionary<ResourceType, ResourceData>();

        public void LoadCommonResourceDataTable()
        {
            m_common_resource_data.Clear();

            WorkBook book = GetCommonRowData();

            var doc = book["RESOURCE"];

            for (int row = 1; row < doc.Rows.Count; row++)
            {
                var row_data = doc.Rows[row];

                var resource_data = new ResourceData();
                resource_data.resource_type = (ResourceType)row_data[0].Integer;
                resource_data.name = row_data[1].String;
                resource_data.sprite_name = row_data[2].String;

                m_common_resource_data.Add(resource_data.resource_type, resource_data);
            }
        }

        public ResourceData GetCommonResourceData(ResourceType in_resource_type)
        {
            m_common_resource_data.TryGetValue(in_resource_type, out var out_data);

            return out_data;
        }
    }
}