using System;
using System.Collections.Generic;
using UnityEngine;
using Framework;

class GUIManager : TMonoSingleton<GUIManager>
{
    private GameObject                  m_gui_root;
    private Transform                   m_root_transform;
    private GUIBase                     m_cur_open_gui;
    private Dictionary<string, GUIBase> m_gui_resource = new Dictionary<string, GUIBase>();
    private List<GUIBase>               m_open_gui_stack = new List<GUIBase>();

    protected override void Awake()
    {
        if (m_gui_root != null)
            return;

        string GUI_RROOT_PATH = "UI/Prefabs/Base/GUIRoot";
        var load_root = Resources.Load<GameObject>(GUI_RROOT_PATH);
        if (load_root == null)
            return;

        var root = GameObject.Instantiate<GameObject>(load_root);
        if (root == null)
            return;

        var rect_transform = root.GetComponentInChildren<RectTransform>();
        if (rect_transform == null)
            return;

        m_gui_root = root;
        m_gui_root.transform.parent = this.transform;
        m_root_transform = rect_transform.transform;
    }

    public T OpenGUI<T>(IGUIOpenParam in_param) 
        where T : GUIBase
    {
        string GUI_NAME = typeof(T).Name;

        if(m_cur_open_gui != null)
        {
            if (m_cur_open_gui.m_gui_name == GUI_NAME)
            {
                return (T)m_cur_open_gui;
            }
        }

        if (m_gui_resource.TryGetValue(GUI_NAME, out var out_gui) && out_gui != null)
        {
            out_gui.Open(in_param);

            if(out_gui.m_use_stack)
            {
                m_open_gui_stack.Remove(out_gui);
                m_open_gui_stack.Add(out_gui);
            }

            NewOpenGUI(out_gui);

            return (T)out_gui;
        }

        string DEFAULT_GUI_PATH = "UI/Prefabs/";
        string GUI_PATH = DEFAULT_GUI_PATH + GUI_NAME;
        var load_gui = Resources.Load<T>(GUI_PATH);
        if (load_gui == null)
            return null;

        var go = GameObject.Instantiate<T>(load_gui);
        if (go == null)
            return null;

        var go_rect_transform = go.GetComponentInChildren<RectTransform>();
        if (go_rect_transform == null)
            return null;

        go_rect_transform.SetParent(m_root_transform);
        go_rect_transform.localPosition = Vector3.zero;
        go_rect_transform.anchoredPosition = Vector2.zero;
        go_rect_transform.localScale = Vector3.one;
        go_rect_transform.sizeDelta = Vector2.zero;

        go.m_gui_name = GUI_NAME;
        go.Open(in_param);

        m_gui_resource.Remove(go.m_gui_name);
        m_gui_resource.Add(go.m_gui_name, go);

        if(go.m_use_stack)
            m_open_gui_stack.Add(go);

        if (load_gui.m_gui_type == EGUIType.Popup)
        {
            OpenPopupGUI(go);
        }
        else
        {
            NewOpenGUI(go);
        }

        return go;
    }

    public T FindGUI<T>()
        where T : GUIBase
    {
        string GUI_NAME = typeof(T).Name;

        if (m_gui_resource.TryGetValue(GUI_NAME, out var out_gui))
        {
            return (T)out_gui;
        }

        return null;
    }

    public void CloseGUI<T>()
        where T : GUIBase
    {
        string GUI_NAME = typeof(T).Name;

        var find_gui = FindGUI<T>();
        if(find_gui != null)
        {
            m_gui_resource.Remove(GUI_NAME);
            m_open_gui_stack.Remove(find_gui);
            find_gui.Close();

            OpenPreviousGUI();
        }
    }

    public void OpenPreviousGUI()
    {
        if (m_cur_open_gui != null)
        {
            m_open_gui_stack.Remove(m_cur_open_gui);

            GUIBase previous_gui = null;
            if (m_open_gui_stack.Count > 0)
                previous_gui = m_open_gui_stack[m_open_gui_stack.Count - 1];

            NewOpenGUI(previous_gui);
        }
    }

    public void OpenRootGUI()
    {
        GUIStackClear();
        OpenGUI<GUI_Lobby>(new GUI_Lobby.OpenParam());
    }

    public void GUIStackClear()
    {
        foreach(var gui in m_open_gui_stack)
            gui.gameObject.SetActive(false);

        m_open_gui_stack.Clear();
        m_cur_open_gui = null;
    }

    private void NewOpenGUI(GUIBase in_new_gui)
    {
        if (in_new_gui == null)
            return;

        if (m_cur_open_gui != null)
            m_cur_open_gui.gameObject.SetActive(false);

        m_cur_open_gui = in_new_gui;
        m_cur_open_gui.gameObject.SetActive(true);
        m_cur_open_gui.transform.SetAsLastSibling();
    }

    public void OpenPopupGUI(GUIBase in_new_gui)
    {
        if (in_new_gui == null)
            return;

        //if (m_cur_open_gui != null)
        //    m_cur_open_gui.gameObject.SetActive(false);

        m_cur_open_gui = in_new_gui;
        m_cur_open_gui.gameObject.SetActive(true);
        m_cur_open_gui.transform.SetAsLastSibling();
    }
}
