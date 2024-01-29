using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

class GUI_RewardPopup : GUIBase
{
    [SerializeField] private Button m_button_ok;
    [SerializeField] private TextMeshProUGUI m_reward_value;


    public class OpenParam : IGUIOpenParam
    {
        public int rewardCoin = 0;

        public OpenParam(int in_reward)
        {
            rewardCoin = in_reward;
        }
    }

    public override void Init()
    {
        base.Init();

        m_button_ok.onClick.AddListener(OnClickOkButton);
    }

    public override void Open(IGUIOpenParam in_param)
    {
        base.Open(in_param);

        var param_data = in_param as OpenParam;
        if (param_data != null)
        {
            m_reward_value.text = param_data.rewardCoin.ToString("#,##0");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // ������ ���� 
    }


    private void OnClickOkButton()
    {
        //������ ���� ó��

        GUIManager.Instance.CloseGUI<GUI_RewardPopup>();
    }
}
