using System;
using Framework.Scheduler.Base;
using Network;
using Protocol;

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
        };

        GUIManager.Instance.OpenGUI<GUI_Login>(new GUI_Login.OpenParam(button_action));
    }

    public override void OnUpdate()
    {
        //if (FirebaseManager.Instance.IsUserLogin() == false)
        //    return;

        //string account_id = FirebaseManager.Instance.GetAccountID();
        //if (account_id == string.Empty)
        //    return;

        //if (WebSocketClient.Instance.GetSocketState() == System.Net.WebSockets.WebSocketState.None || WebSocketClient.Instance.GetSocketState() == System.Net.WebSockets.WebSocketState.Closed)
        //{
        //    WebSocketClient.Instance.Connect("ws://localhost:8080", account_id);
        //}

        //if (WebSocketClient.Instance.GetSocketState() != System.Net.WebSockets.WebSocketState.Open)
        //    return;

        //ProtocolBinder.Instance.Initialize();

        Complete(ETaskState.Success);
    }

    public override void OnComplete()
    {

    }
}