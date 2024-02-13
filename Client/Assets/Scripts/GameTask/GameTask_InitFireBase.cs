using System;
using Framework.Scheduler.Base;
using DataTable;
using UnityEngine;

class GameTask_InitFireBase : Task
{
    public override void OnAwake()
    {
        Debug.Log("GameTask_InitFireBase OnAwake");

        FirebaseManager.Instance.InitFirebase();
    }

    public override void OnUpdate()
    {
        if (FirebaseManager.Instance.IsFireBaseLogin() == false)
            return;

        Complete(ETaskState.Success);
    }

    public override void OnComplete()
    {
        Debug.Log("GameTask_InitFireBase OnComplete");
    }
}