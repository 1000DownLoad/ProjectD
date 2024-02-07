using System;
using Framework.Scheduler.Base;
using Network;
using Protocol;

class GameTask_GameLogin : Task
{
    private bool is_firebase_login = false;
    private bool is_check_network = false;
    private bool is_send_user_auth_token = false;
    private bool is_send_user_login = false;
    private bool is_send_user_base_info = false;

    public override void OnAddTask()
    {

    }

    public override void OnAwake()
    {
        Action button_action = () =>
        {
            GUIManager.Instance.CloseGUI<GUI_Login>();
            GUIManager.Instance.OpenGUI<GUI_Loading>(new GUI_Loading.OpenParam("LobbyScene"));
        };

        GUIManager.Instance.OpenGUI<GUI_Login>(new GUI_Login.OpenParam(button_action));
    }

    public override void OnUpdate()
    {
        // ���̾� ���̽� ���� Ȯ��
        if (FirebaseManager.Instance.IsUserLogin() == false)
        {
            if(is_firebase_login == false)
            {
                FirebaseManager.Instance.LoginAnonymous();
                is_firebase_login = true;
            }

            return;
        }

        string firebase_uid = FirebaseManager.Instance.GetUID();
        if (firebase_uid == string.Empty)
            return;

        if (is_check_network == false)
        {
            // ���� ���� ����
            WebSocketClient.Instance.Connect("ws://localhost:8080", firebase_uid);

            // �������� ���δ� �ʱ�ȭ
            ProtocolBinder.Instance.Initialize();

            is_check_network = true;
        }

        // ���� üũ
        if (WebSocketClient.Instance.GetSocketState() != System.Net.WebSockets.WebSocketState.Open)
            return;

        // ���� ��ū ��û
        if (is_send_user_auth_token == false)
        {
            var user_auth_token_req = new GS_USER_AUTH_TOKEN_REQ();
            user_auth_token_req.AccountID = FirebaseManager.Instance.GetUID();
            WebSocketClient.Instance.Send<GS_USER_AUTH_TOKEN_REQ>(PROTOCOL.GS_USER_AUTH_TOKEN_REQ, user_auth_token_req);

            is_send_user_auth_token = true;
        }

        // ���� ��ū Ȯ��
        if (UserManager.Instance.m_auth_token == string.Empty)
            return;

        // ���� �α��� ��û
        if (is_send_user_login == false)
        {
            var user_login_req = new GS_USER_LOGIN_REQ();
            user_login_req.AccountID = FirebaseManager.Instance.GetUID();
            WebSocketClient.Instance.Send<GS_USER_LOGIN_REQ>(PROTOCOL.GS_USER_LOGIN_REQ, user_login_req);

            is_send_user_login = true;
        }

        // ���� üũ
        var user = UserManager.Instance.GetUser();
        if (user == null)
            return;

        // ���� �⺻ ���� ��û
        if (is_send_user_base_info == false)
        {
            var user_req = new GS_USER_BASE_INFO_GET_REQ();
            user_req.UserID = user.user_id;
            WebSocketClient.Instance.Send<GS_USER_BASE_INFO_GET_REQ>(PROTOCOL.GS_USER_BASE_INFO_GET_REQ, user_req);

            is_send_user_base_info = true;
        }

        // ���� ���� �ʱ�ȭ üũ
        if (UserManager.Instance.m_is_init_data == false)
            return;

        Complete(ETaskState.Success);
    }

    public override void OnComplete()
    {

    }
}