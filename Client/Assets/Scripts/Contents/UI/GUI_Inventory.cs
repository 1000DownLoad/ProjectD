using System;
using UnityEngine;
using UnityEngine.UI;




class GUI_Inventory : GUIBase
{

    [SerializeField] private SUB_Item baseItem;
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

    public override void Init()
    {
        base.Init();

        weaponToggle.onValueChanged.AddListener(OnClickWeaponTab);
        shieldToggle.onValueChanged.AddListener(OnClickShieldTab);
        shoesToggle.onValueChanged.AddListener(OnClickShoesTab);
        ringsToggle.onValueChanged.AddListener(OnClickRingsTab);
    }

    public override void Open(IGUIOpenParam in_param)
    {
        base.Open(in_param);

        var param_data = in_param as OpenParam;
        if (param_data != null)
        {

        }
    }

    private void OnClickRingsTab(bool isOn)
    {
        if (isOn)
        {
            ClearItems();

            Debug.Log("반지 클릭");

            foreach (var item_data in UserItemManager.Instance.m_item_data)
            {
                var ringItem = Instantiate(baseItem, itemParent);
                ringItem.GetComponent<SUB_Item>().SetItemInfo((int)item_data.Key, (int)item_data.Value);
            }
        }
    }

    private void OnClickShieldTab(bool isOn)
    {
        if(isOn)
        {
            ClearItems();


            //Debug.Log("방패 클릭");
            //var shieldItem = Instantiate(baseItem, itemParent);
            //shieldItem.GetComponent<SUB_Item>().SetItemInfo(ItemType.Armor);
        }
    }

    private void OnClickShoesTab(bool isOn)
    {
        if(isOn)
        {
            ClearItems();

            //Debug.Log("신발 클릭");
            //var shoesItem = Instantiate(baseItem, itemParent);
            //shoesItem.GetComponent<SUB_Item>().SetItemInfo(ItemType.Shoes);
        }
    }

    private void OnClickWeaponTab(bool isOn)
    {
        if(isOn)
        {
            ClearItems();

            //Debug.Log("무기 클릭");
            //var weaponItem = Instantiate(baseItem, itemParent);
            //weaponItem.GetComponent<SUB_Item>().SetItemInfo(ItemType.Weapon);
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
