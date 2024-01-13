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
        AccountDataTable.Instance.LoadCommonAccountDataTable();
        ResourceDataTable.Instance.LoadCommonResourceDataTable();

        Complete(ETaskState.Success);
    }

    public override void OnComplete()
    {

    }
}