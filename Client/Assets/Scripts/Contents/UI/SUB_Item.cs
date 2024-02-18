using System.Collections;
using System.Collections.Generic;
using DataTable;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SUB_Item : MonoBehaviour
{
    [SerializeField] private Button button;
    [SerializeField] private Image itemBg;
    [SerializeField] private Image itemIcon;
    [SerializeField] private TextMeshProUGUI itemCount;
    [SerializeField] private GameObject selectedObj;
    [SerializeField] private GameObject tooltipObj;
    private bool isSelected = false;
    private int _itemIndex = 0;

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
        tooltipObj.SetActive(!isSelected);
        tooltipObj.GetComponent<SUB_ToolTIp>().SetTooltipText(_itemIndex);
        isSelected = !isSelected;
    }

    public void SetItemInfo(int in_item_index, int in_item_count)
    {
        var item = ItemDataTable.Instance.GetCommonItemData(in_item_index);
        itemIcon.sprite = Resources.Load<Sprite>($"UI/Sprites/{item.sprite_name}");
        itemCount.text = in_item_count.ToString();
        _itemIndex = in_item_index;
    }
}
