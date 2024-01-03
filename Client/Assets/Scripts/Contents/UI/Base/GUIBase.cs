using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

interface IGUIOpenParam
{

};

enum EGUIType
{
    Screen,
    Popup,
}

class GUIBase : MonoBehaviour
{
    [NonSerialized]  public string   m_gui_name;
    [SerializeField] public EGUIType m_gui_type;
    [SerializeField] public bool     m_use_stack;

    [SerializeField] public Button   m_back_button;
    [SerializeField] public Button   m_home_button;

    virtual public void Open(IGUIOpenParam in_param)
    {

    }

    virtual public void OpenPopup(IGUIOpenParam in_param)
    {
        
    }

    virtual public void Close()
    {
        Destroy(this.gameObject);
    }

    virtual public void OnBackButtonClick()
    {
        GUIManager.Instance.OpenPreviousGUI();
    }

    virtual public void OnHomeButtonClick()
    {
        GUIManager.Instance.OpenRootGUI();
    }
}
