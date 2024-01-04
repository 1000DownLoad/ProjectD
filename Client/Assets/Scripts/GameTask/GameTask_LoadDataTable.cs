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
        //AccountDataTable.LoadAccountDataTable();
        //ResourceDataTable.LoadResourceDataTable();

        Complete(ETaskState.Success);
    }

    public override void OnComplete()
    {

    }
}