using FlexFramework.Excel;
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

    public static class ResourceDataTable
    {
        private static Dictionary<ResourceType, ResourceData> m_resource_data = new Dictionary<ResourceType, ResourceData>();

        public readonly static string FilePath = "../DataTable/Common/ResourceTable.xlsx";

        public static void LoadResourceDataTable()
        {
            m_resource_data.Clear();

            var fs = new FileStream(FilePath, FileMode.Open);
            byte[] bytes = new byte[fs.Length];
            fs.Read(bytes, 0, (int)fs.Length);

            WorkBook book = new WorkBook(bytes);

            var doc = book["RESOURCE"];

            for (int row = 1; row < doc.Rows.Count; row++)
            {
                var row_data = doc.Rows[row];

                var resource_data = new ResourceData();
                resource_data.resource_type = (ResourceType)row_data[0].Integer;
                resource_data.name = row_data[1].String;
                resource_data.sprite_name = row_data[2].String;

                m_resource_data.Add(resource_data.resource_type, resource_data);
            }

            fs.Close();
        }

        static public ResourceData GetResourceData(ResourceType in_resource_type)
        {
            m_resource_data.TryGetValue(in_resource_type, out var out_data);

            return out_data;
        }
    }
}