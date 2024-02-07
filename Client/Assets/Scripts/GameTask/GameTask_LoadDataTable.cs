using System;
using Framework.Scheduler.Base;
using DataTable;

class GameTask_LoadDataTable : Task
{
    public override void OnAddTask()
    {

    }

    public override void OnAwake()
    {

    }

    public override void OnUpdate()
    {
        UserDataTable.Instance.LoadDataTable();
        ResourceDataTable.Instance.LoadDataTable();
        ItemDataTable.Instance.LoadDataTable();

        Complete(ETaskState.Success);
    }

    public override void OnComplete()
    {

    }
}