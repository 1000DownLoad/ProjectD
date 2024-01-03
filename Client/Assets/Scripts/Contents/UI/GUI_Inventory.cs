using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

class GUI_Inventory : GUIBase
{

    [SerializeField] private LoopVerticalScrollRect loopRect;
    [SerializeField] private UI_Item baseItem;
    [SerializeField] private Toggle weaponToggle;
    [SerializeField] private Toggle shieldToggle;
    [SerializeField] private Toggle shoesToggle;
    [SerializeField] private Toggle ringsToggle;

    public class OpenParam : IGUIOpenParam
    {
        public OpenParam()
        {
        }
    }

    private void Awake()
    {
        m_back_button.onClick.AddListener(OnBackButtonClick);
        BindUI();
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

    void BindUI()
    {
        weaponToggle.OnValueChangedAsObservable().Subscribe(isOn =>
        {
            Debug.Log(isOn ? "무기" : "무기꺼짐");
        });

        shieldToggle.OnValueChangedAsObservable().Subscribe(isOn =>
        {
            Debug.Log(isOn ? "방패" : "방패꺼짐");
        });

        shoesToggle.OnValueChangedAsObservable().Subscribe(isOn =>
        {
            Debug.Log(isOn ? "신발" : "신발꺼짐");
        });

        ringsToggle.OnValueChangedAsObservable().Subscribe(isOn =>
        {
            Debug.Log(isOn ? "반지" : "반지꺼짐");
        });
    }

    //TODO: 스크롤 컨텐츠 오브젝트 풀링.. 


}
