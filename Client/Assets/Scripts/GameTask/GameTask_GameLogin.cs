using System;
using Framework.Scheduler.Base;
using Network;
using DataTable;
using UnityEngine;

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
        //if (FirebaseManager.Instance.m_firebase_auth == null)
        //    return;

        //if (FirebaseManager.Instance.m_firebase_auth.CurrentUser == null)
        //    return;

        //if(WebSocketClient.Instance.GetSocketState() == System.Net.WebSockets.WebSocketState.None 
        //    || WebSocketClient.Instance.GetSocketState() == System.Net.WebSockets.WebSocketState.Closed)
        //{
        //    WebSocketClient.Instance.Connect();
        //}

        //if (WebSocketClient.Instance.GetSocketState() != System.Net.WebSockets.WebSocketState.Open)
        //    return;

        //ProtocolBinder.Instance.InitRegisterHandles();

        Complete(ETaskState.Success);
    }

    public override void OnComplete()
    {

    }
}