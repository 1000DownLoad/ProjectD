﻿using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    private void Awake()
    {
        m_inventory_button.onClick.AddListener(OnInventoryButtonClick);
        m_stage_button.onClick.AddListener(OnStageButtonClick);
        m_battle_button.onClick.AddListener(OnBattleButtonClick);
    }

    override public void Open(IGUIOpenParam in_param)
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
        m_account_level_text.SetText(AccountManager.Instance.m_user_level.ToString());

        var account_data = DataTable.AccountDataTable.GetAccountData(AccountManager.Instance.m_user_level);
        if(account_data != null)
        {
            m_account_exp_text.SetText(string.Format("{0}/{1}", Util.UI.SeparatorConvert(AccountManager.Instance.m_user_exp), Util.UI.SeparatorConvert(account_data.need_exp)));

            var energy_data = ResourceManager.Instance.GetResourceData(ResourceType.ENERGY);
            if (energy_data != null)
                m_resource_energy_text.SetText(string.Format("{0}/{1}", Util.UI.SeparatorConvert(energy_data.count), Util.UI.SeparatorConvert(account_data.max_energy)));
            else
                m_resource_energy_text.SetText(string.Format("0"));

            RefreshSlider(AccountManager.Instance.m_user_exp / (float)account_data.need_exp);
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
        GUIManager.Instance.OpenGUI<GUI_RewardPopup>(new GUI_RewardPopup.OpenParam(300));
    }
}
