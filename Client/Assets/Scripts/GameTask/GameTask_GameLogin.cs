using System;
using Framework.Scheduler.Base;
using Network;
using Protocol;
using Account;
using User;

class GameTask_GameLogin : Task
{
    private bool is_check_network = false;
    private bool is_send_account = false;
    private bool is_send_user_login = false;
    private bool is_send_user_base_info = false;

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
        Complete(ETaskState.Success);
        return;

        // ���̾� ���̽� ���� Ȯ��
        if (FirebaseManager.Instance.IsUserLogin() == false)
            return;

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

        // ���� ���� ��û
        if (is_send_account == false)
        {
            var account_req = new GS_ACCOUNT_GET_REQ();
            account_req.AccountID = FirebaseManager.Instance.GetUID();
            WebSocketClient.Instance.Send<GS_ACCOUNT_GET_REQ>(PROTOCOL.GS_ACCOUNT_GET_REQ, account_req);

            is_send_account = true;
        }

        // ���� ���� �ʱ�ȭ üũ
        var account_info = AccountManager.Instance.GetAccountInfo();
        if (account_info.account_id == null)
            return;

        // ���� �α��� ��û
        if (is_send_user_login == false)
        {
            var user_login_req = new GS_USER_LOGIN_REQ();
            user_login_req.UserID = account_info.user_id;
            WebSocketClient.Instance.Send<GS_USER_LOGIN_REQ>(PROTOCOL.GS_USER_LOGIN_REQ, user_login_req);

            is_send_user_login = true;
        }

        // ���� ID ���� üũ
        var user = UserManager.Instance.GetUser();
        if (user.user_id == 0)
            return;

        // ���� �⺻ ���� ��û
        if (is_send_user_base_info == false)
        {
            var user_req = new GS_USER_BASE_INFO_GET_REQ();
            user_req.UserID = account_info.user_id;
            WebSocketClient.Instance.Send<GS_USER_BASE_INFO_GET_REQ>(PROTOCOL.GS_USER_BASE_INFO_GET_REQ, user_req);

            is_send_user_base_info = true;
        }

        // ���� ���� �ʱ�ȭ üũ
        if (user.is_init_data == false)
            return;

        Complete(ETaskState.Success);
    }

    public override void OnComplete()
    {

    }
}