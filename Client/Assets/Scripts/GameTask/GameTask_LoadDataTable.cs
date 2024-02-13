using System;
using Framework.Scheduler.Base;
using DataTable;
using UnityEngine;

class GameTask_LoadDataTable : Task
{
    public override void OnAwake()
    {
        Debug.Log("GameTask_LoadDataTable OnAwake");
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
        Debug.Log("GameTask_LoadDataTable OnComplete");
    }
}