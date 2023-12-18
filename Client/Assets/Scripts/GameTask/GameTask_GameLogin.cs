using System;
using Framework.Scheduler.Base;
using DataTable;

class GameTask_GameLogin : Task
{
    public override void OnAddTask()
    {

    }

    public override void OnAwake()
    {
        Action callback = ()=>
        {
            AccountDataTable.LoadAccountDataTable();
            ResourceDataTable.LoadResourceDataTable();

            GUIManager.Instance.CloseGUI<GUI_Login>();
            GUIManager.Instance.OpenGUI<GUI_Loading>(new GUI_Loading.OpenParam("LobbyScene"));

            Complete(ETaskState.Success);
        };

        GUIManager.Instance.OpenGUI<GUI_Login>(new GUI_Login.OpenParam(callback));
    }

    public override void OnUpdate()
    {

    }

    public override void OnComplete()
    {

    }
}