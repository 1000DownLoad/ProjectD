using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Network;
using Protocol;
using UnityEditor;

class GUI_Command : GUIBase
{
    [MenuItem("CustomMenu/OpenCommandGUI")]
    private static void OpenCommandGUI()
    {
        GUIManager.Instance.OpenGUI<GUI_Command>(new GUI_Command.OpenParam());
    }

    [SerializeField] private TMP_InputField m_command_input;
    [SerializeField] private Button m_send_button;

    private string m_command;

    public class OpenParam : IGUIOpenParam
    {
        public OpenParam()
        {

        }
    }

    public override void Init()
    {
        base.Init();

        m_command_input.onValueChanged.AddListener(OnInputChange);
        m_send_button.onClick.AddListener(OnSendButtonClick);
    }

    public override void Open(IGUIOpenParam in_param)
    {
        base.Open(in_param);

        var param_data = in_param as OpenParam;
        if (param_data != null)
        {

        }
    }

    override public void Close()
    {
        base.Close();
    }

    private void OnInputChange(string in_string)
    {
        m_command = in_string;
    }

    private void OnSendButtonClick()
    {
        var req = new GS_USER_COMMAND_REQ();
        req.UserID = UserManager.Instance.GetUser().user_id;
        req.Command = m_command;

        WebSocketClient.Instance.Send<GS_USER_COMMAND_REQ>(PROTOCOL.GS_USER_COMMAND_REQ, req);
    }
}
