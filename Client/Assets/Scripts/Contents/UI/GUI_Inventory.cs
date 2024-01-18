using System;
using UnityEngine;
using UnityEngine.UI;
using UniRx;


public enum ItemType
{
    NONE,
    Weapon,
    Armor,
    Shoes,
    Accessories, 
}

class GUI_Inventory : GUIBase
{

    [SerializeField] private UI_Item baseItem;
    [SerializeField] private Transform itemParent;
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
    }

    override public void Open(IGUIOpenParam in_param)
    {
        base.Open(in_param);

        var param_data = in_param as OpenParam;
        if (param_data != null)
        {

        }

        Init();

    }

    private void Init()
    {
        weaponToggle.onValueChanged.AddListener(OnClickWeaponTab);
        shieldToggle.onValueChanged.AddListener(OnClickShieldTab);
        shoesToggle.onValueChanged.AddListener(OnClickShoesTab);
        ringsToggle.onValueChanged.AddListener(OnClickRingsTab);

    }

    private void OnClickRingsTab(bool isOn)
    {
        if (isOn)
        {
            ClearItems();

            Debug.Log("반지 클릭");
            var ringItem = Instantiate(baseItem, itemParent);
            ringItem.GetComponent<UI_Item>().SetItemInfo(ItemType.Accessories);
        }
    }

    private void OnClickShieldTab(bool isOn)
    {
        if(isOn)
        {
            ClearItems();


            Debug.Log("방패 클릭");
            var shieldItem = Instantiate(baseItem, itemParent);
            shieldItem.GetComponent<UI_Item>().SetItemInfo(ItemType.Armor);
        }
    }

    private void OnClickShoesTab(bool isOn)
    {
        if(isOn)
        {
            ClearItems();

            Debug.Log("신발 클릭");
            var shoesItem = Instantiate(baseItem, itemParent);
            shoesItem.GetComponent<UI_Item>().SetItemInfo(ItemType.Shoes);
        }
    }

    private void OnClickWeaponTab(bool isOn)
    {
        if(isOn)
        {
            ClearItems();

            Debug.Log("무기 클릭");
            var weaponItem = Instantiate(baseItem, itemParent);
            weaponItem.GetComponent<UI_Item>().SetItemInfo(ItemType.Weapon);
        }
    }

    void ClearItems()
    {
        if (itemParent.childCount > 0)
        {
            for (int i = 0; i < itemParent.childCount; i++)
            {
                Destroy(itemParent.GetChild(i).gameObject);
            }
        }
    }
}
