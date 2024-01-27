using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Account;
using Framework.Event;
using Network;
using Protocol;
using User;

class GUI_Lobby : GUIBase
{
    [SerializeField] private TMP_Text m_account_level_text;
    [SerializeField] private TMP_Text m_account_exp_text;
    [SerializeField] private TMP_Text m_resource_gem_text;
    [SerializeField] private TMP_Text m_resource_gold_text;
    [SerializeField] private TMP_Text m_resource_energy_text;

    [SerializeField] private Slider m_account_exp_slider;

    [SerializeField] private Button m_inventory_button;
    [SerializeField] private Button m_stage_button;
    [SerializeField] private Button m_battle_button;

    public class OpenParam : IGUIOpenParam
    {
        public OpenParam()
        {
        }
    }

    public override void OnEventHandle(EventData in_data)
    {
        var user_data = in_data as EVENT_USER_DATA_UPDATE;
        if (user_data != null)
        {
            RefreshText();
        }
    }

    private void Awake()
    {
        GUIManager.Instance.SubscribeEvnet(typeof(EVENT_USER_DATA_UPDATE));

        m_inventory_button.onClick.AddListener(OnInventoryButtonClick);
        m_stage_button.onClick.AddListener(OnStageButtonClick);
        m_battle_button.onClick.AddListener(OnBattleButtonClick);
    }

    public override void Open(IGUIOpenParam in_param)
    {
        base.Open(in_param);

        var param_data = in_param as OpenParam;
        if (param_data != null)
        {

        }

        RefreshText();
    }

    private void RefreshText()
    {
        var account_info = AccountManager.Instance.GetAccountInfo();
        if (account_info == null)
            return;

        m_account_level_text.SetText(account_info.level.ToString());

        var account_data = DataTable.AccountDataTable.Instance.GetAccountTableData(account_info.level);
        if(account_data != null)
        {
            m_account_exp_text.SetText(string.Format("{0}/{1}", Util.UI.SeparatorConvert(account_info.cur_exp), Util.UI.SeparatorConvert(account_data.max_exp)));

            var energy_data = ResourceManager.Instance.GetResourceData(ResourceType.ENERGY);
            if (energy_data != null)
                m_resource_energy_text.SetText(string.Format("{0}/{1}", Util.UI.SeparatorConvert(energy_data.count), Util.UI.SeparatorConvert(account_data.max_energy)));
            else
                m_resource_energy_text.SetText(string.Format("0"));

            RefreshSlider(account_info.cur_exp / (float)account_data.max_exp);
        }

        var gem_data = ResourceManager.Instance.GetResourceData(ResourceType.GEM);
        if (gem_data != null)
            m_resource_gem_text.SetText(Util.UI.SeparatorConvert(gem_data.count));
        else
            m_resource_gem_text.SetText(string.Format("0"));

        var gold_data = ResourceManager.Instance.GetResourceData(ResourceType.GOLD);
        if(gold_data != null)
            m_resource_gold_text.SetText(Util.UI.SeparatorConvert(gold_data.count));
        else
            m_resource_gold_text.SetText(string.Format("0"));
    }

    private void RefreshSlider(float in_value)
    {
        m_account_exp_slider.value = in_value;
    }

    private void OnInventoryButtonClick()
    {
        GUIManager.Instance.OpenGUI<GUI_Inventory>(new GUI_Inventory.OpenParam());
    }

    private void OnStageButtonClick()
    {
        GUIManager.Instance.OpenGUI<GUI_Loading>(new GUI_Loading.OpenParam("StageScene"));
    }

    private void OnBattleButtonClick()
    {
        var user_login_req = new GS_USER_LOGIN_REQ();
        user_login_req.UserID = UserManager.Instance.GetUser().user_id;
        WebSocketClient.Instance.Send<GS_USER_LOGIN_REQ>(PROTOCOL.GS_USER_LOGIN_REQ, user_login_req);
    }
}
