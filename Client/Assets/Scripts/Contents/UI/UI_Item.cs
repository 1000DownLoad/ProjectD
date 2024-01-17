using System.Collections;
using System.Collections.Generic;
using DataTable;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class UI_Item : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image itemBg;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private GameObject selectedObj;
    private bool isSelected = false;

    void Awake()
    {
        button = GetComponent<Button>();
    }

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnClickItemButton);
    }

    private void OnClickItemButton()
    {
        selectedObj.SetActive(!isSelected);
        isSelected = !isSelected;
    }

    // 일단 switch문으로 해놓고 추후 수정
    public void SetItemInfo(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Weapon:
            {
                var item = ItemDataTable.Instance.GetCommonItemData(ItemType.Weapon);
                itemIcon.sprite = Resources.Load<Sprite>($"UI/Sprites/{item.sprite_name}");
            }
            break;
            
            case ItemType.Armor:
            {
                var item = ItemDataTable.Instance.GetCommonItemData(ItemType.Armor);
                itemIcon.sprite = Resources.Load<Sprite>($"UI/Sprites/{item.sprite_name}");
            }
            break;
            
            case ItemType.Shoes:
            {
                var item = ItemDataTable.Instance.GetCommonItemData(ItemType.Shoes);
                itemIcon.sprite = Resources.Load<Sprite>($"UI/Sprites/{item.sprite_name}");
            }
            break;
            
            case ItemType.Accessories:
            {
                var item = ItemDataTable.Instance.GetCommonItemData(ItemType.Accessories);
                itemIcon.sprite = Resources.Load<Sprite>($"UI/Sprites/{item.sprite_name}");
            }
            break;
        }
    }
}
