using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

class GUI_Login : GUIBase
{
    [SerializeField] private TMP_InputField m_id_input;
    [SerializeField] private TMP_InputField m_password_input;
    [SerializeField] private Button m_login_button;

    private string m_id;
    private string m_password;
    private Action m_login_button_callback;

    public class OpenParam : IGUIOpenParam
    {
        public Action m_login_button_callback;
        public OpenParam(Action in_login_button_callback)
        {
            m_login_button_callback = in_login_button_callback;
        }
    }
    private void Awake()
    {
        m_id_input.onValueChanged.AddListener(OnIDInputChange);
        m_password_input.onValueChanged.AddListener(OnPasswordInputChange);
        m_login_button.onClick.AddListener(OnLoginButtonClick);
    }

    override public void Open(IGUIOpenParam in_param)
    {
        base.Open(in_param);

        var param_data = in_param as OpenParam;
        if (param_data != null)
        {
            m_login_button_callback = param_data.m_login_button_callback;
        }
    }

    override public void Close()
    {
        base.Close();
    }

    private void OnIDInputChange(string in_string)
    {
        m_id = in_string;
    }

    private void OnPasswordInputChange(string in_string)
    {
        m_password = in_string;
    }

    private void OnLoginButtonClick()
    {
        if (m_login_button_callback != null)
            m_login_button_callback.Invoke();
    }
}
