using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;
using TMPro;

class GUI_Loading : GUIBase
{
    [SerializeField] private TMP_Text m_loading_text;
    [SerializeField] private Slider m_loading_slider;

    public class OpenParam : IGUIOpenParam
    {
        public OpenParam(string in_next_scene_name) 
        {
            next_scene_name = in_next_scene_name;
        }

        public string next_scene_name;
    }

    override public void Open(IGUIOpenParam in_param)
    {
        base.Open(in_param);

        var param_data = in_param as OpenParam;
        if(param_data != null)
        {
            StartCoroutine(LoadSceneAsync(param_data.next_scene_name));
        }
    }

    override public void Close()
    {
        base.Close();
    }

    IEnumerator LoadSceneAsync(string in_next_scene_name)
    {
        var async_operation = SceneManager.LoadSceneAsync(in_next_scene_name);
        int percent = 0;

        while (async_operation.isDone == false)
        {
            m_loading_slider.value = async_operation.progress;
            percent = (int)(async_operation.progress * 100);
            m_loading_text.SetText(string.Format("Loading...{0}%", percent));

            yield return null;
        }

        m_loading_slider.value = 1f;
        m_loading_text.SetText(string.Format("Loading...{0}%", 100));

        Close();

        if (in_next_scene_name == "LobbyScene")
        {
            GUIManager.Instance.OpenGUI<GUI_Lobby>(new GUI_Lobby.OpenParam());
        }
        else if (in_next_scene_name == "StageScene")
        {
            GUIManager.Instance.OpenGUI<GUI_Stage>(new GUI_Stage.OpenParam());
        }
    }
}
