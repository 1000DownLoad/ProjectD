using System;
using UnityEngine;
using UnityEngine.UI;

class GUI_Inventory : GUIBase
{

    [SerializeField] private LoopVerticalScrollRect loopRect;
    [SerializeField] private UI_Item baseItem;

    public class OpenParam : IGUIOpenParam
    {
        public OpenParam()
        {
        }
    }

    private void Awake()
    {
        m_back_button.onClick.AddListener(OnBackButtonClick);
    }

    override public void Open(IGUIOpenParam in_param)
    {
        base.Open(in_param);

        var param_data = in_param as OpenParam;
        if (param_data != null)
        {

        }

        loopRect.Initialize(baseItem.gameObject, (itemTransform, index) =>
        {
            if (false == itemTransform.TryGetComponent<UI_Item>(out var item)) 
                return;

            item.SetItemInfo();
        });

        loopRect.UpdatePollTotalCount(64);
        loopRect.RefreshScrollRect();
    }

    //TODO: 스크롤 컨텐츠 오브젝트 풀링.. 


}
