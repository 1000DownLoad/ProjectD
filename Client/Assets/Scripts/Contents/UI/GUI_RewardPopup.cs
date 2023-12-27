using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class GUI_RewardPopup : GUIBase
{
    public class OpenParam : IGUIOpenParam
    {
        public OpenParam()
        {

        }
    }

    void Awake()
    {
        if (m_back_button != null)
            m_back_button.onClick.AddListener(OnBackButtonClick);
    }

    public override void Open(IGUIOpenParam in_param)
    {
        base.Open(in_param);

        var param_data = in_param as OpenParam;
        if (param_data != null)
        {

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
