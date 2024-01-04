using System;
using Framework.Scheduler.Base;
using DataTable;

class GameTask_InitFireBase : Task
{
    public override void OnAddTask()
    {

    }

    public override void OnAwake()
    {
        FirebaseManager.Instance.InitFirebase();
    }

    public override void OnUpdate()
    {
        if (FirebaseManager.Instance.m_firebase_auth == null)
            return;

        Complete(ETaskState.Success);
    }

    public override void OnComplete()
    {

    }
}