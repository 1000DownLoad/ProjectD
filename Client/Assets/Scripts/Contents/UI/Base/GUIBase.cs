using Framework.Event;
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

    public virtual void Open(IGUIOpenParam in_param)
    {

    }

    public virtual void OpenPopup(IGUIOpenParam in_param)
    {
        
    }

    public virtual void OnEventHandle(EventData in_data)
    {

    }

    public virtual void Close()
    {
        Destroy(this.gameObject);
    }

    public virtual void OnBackButtonClick()
    {
        GUIManager.Instance.OpenPreviousGUI();
    }

    public virtual void OnHomeButtonClick()
    {
        GUIManager.Instance.OpenRootGUI();
    }
}
