using System;
using UnityEngine;
using UnityEngine.UI;

class GUI_Stage : GUIBase
{
    public class OpenParam : IGUIOpenParam
    {
        public OpenParam()
        {

        }
    }
    private void Awake()
    {
        if (m_back_button != null)
            m_back_button.onClick.AddListener(OnBackButtonClick);
    }

    override public void Open(IGUIOpenParam in_param)
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

    override public void OnBackButtonClick()
    {
        GUIManager.Instance.OpenGUI<GUI_Loading>(new GUI_Loading.OpenParam("LobbyScene"));
    }
}
