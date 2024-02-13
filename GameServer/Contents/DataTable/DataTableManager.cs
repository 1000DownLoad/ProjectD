using System;
using DataTable;

public partial class DataTableManager : TSingleton<DataTableManager>
{
    public void Initialize()
    {
        UserDataTable.Instance.LoadDataTable();
        ResourceDataTable.Instance.LoadDataTable();
        ItemDataTable.Instance.LoadDataTable();
    }
}