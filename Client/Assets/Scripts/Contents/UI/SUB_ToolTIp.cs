using System.Collections;
using System.Collections.Generic;
using DataTable;
using TMPro;
using UnityEngine;

public class SUB_ToolTIp : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemDescription;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void SetTooltipText(int in_index)
    {
        var item_data = ItemDataTable.Instance.GetCommonItemData(in_index);
        itemDescription.text = item_data.description;
    }
}
