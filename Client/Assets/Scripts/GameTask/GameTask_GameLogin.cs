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
        Action button_action = () =>
        {
            //FirebaseManager.Instance.LoginAnonymous();
            //FirebaseManager.Instance.LoginWithGoogle();

            GUIManager.Instance.CloseGUI<GUI_Login>();
            GUIManager.Instance.OpenGUI<GUI_Loading>(new GUI_Loading.OpenParam("LobbyScene"));

            Complete(ETaskState.Success);
        };

        GUIManager.Instance.OpenGUI<GUI_Login>(new GUI_Login.OpenParam(button_action));
    }

    public override void OnUpdate()
    {
        //if (FirebaseManager.Instance.m_firebase_auth == null)
        //    return;

        //if (FirebaseManager.Instance.m_firebase_auth.CurrentUser == null)
        //    return;
    }

    public override void OnComplete()
    {

    }
}