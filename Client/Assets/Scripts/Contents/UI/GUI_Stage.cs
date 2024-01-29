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

    public override void Init()
    {
        base.Init();
    }

    public override void Open(IGUIOpenParam in_param)
    {
        base.Open(in_param);

        var param_data = in_param as OpenParam;
        if (param_data != null)
        {

        }
    }

    public override void Close()
    {
        base.Close();
    }

    public override void OnBackButtonClick()
    {
        GUIManager.Instance.OpenGUI<GUI_Loading>(new GUI_Loading.OpenParam("LobbyScene"));
    }
}
