using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetItemInfo()
    {

    }
}
