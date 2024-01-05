#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif


namespace UnityEngine.UI
{
	public class ButtonEx : Button
	{
        [SerializeField] GameObject enable;
        [SerializeField] GameObject disable;
        [SerializeField] GameObject reddot;
        [SerializeField] GameObject[] listEnable;
        [SerializeField] GameObject[] listDisable;
        [SerializeField] GameObject[] effect;

        bool showEnableBackup = false;
        bool showDisableBackup = false;
        bool showReddotBackup = false;
        bool[] showlistEnableBackup;
        bool[] showlistDisableBackup;
        bool[] showEffectBackup;

        protected override void Start()
        {
            if (null != listEnable)
            {
                showlistEnableBackup = new bool[listEnable.Length];
                for (int i = 0; i < showlistEnableBackup.Length; i++)
                    showlistEnableBackup[i] = false;
            }
            if (null != listDisable)
            {
                showlistDisableBackup = new bool[listDisable.Length];
                for (int i = 0; i < showlistDisableBackup.Length; i++)
                    showlistDisableBackup[i] = false;
            }
            if (null != effect)
            { 
                showEffectBackup = new bool[effect.Length];
                for (int i = 0; i < showEffectBackup.Length; i++)
                    showEffectBackup[i] = false;
            }
        }

        protected override void OnDestroy()
        {
            showlistEnableBackup = null;
            showlistDisableBackup = null;
            showEffectBackup = null;
        }

        protected ButtonEx()
        {
            if (null != enable)
                enable.SetActive(true);
        }

        public void SetEnable()
        {
            if (null != enable)
                enable.SetActive(true);
            if (null != disable)
                disable.SetActive(false);
        }

        public void SetDisable()
        {
            if (null != enable)
                enable.SetActive(false);
            if (null != disable)
                disable.SetActive(true);
        }

        public void ShowDisable(bool show)
        {
            if (null != disable)
                disable.SetActive(show);
        }

        public void ShowReddot(bool show)
        {
            if (null != reddot)
                reddot.SetActive(show);
        }

        public void ShowEnable(int index, bool show)
        {
            if (null != listEnable)
            {
                for (int i = 0; i < listEnable.Length; i++)
                {
                    if (index == i)
                        listEnable[i].SetActive(show);
                    else
                        listEnable[i].SetActive(false);
                }
            }
        }

        public void ShowDisable(int index, bool show)
        {
            if (null != listDisable)
            {
                for (int i = 0; i < listDisable.Length; i++)
                {
                    if (index == i)
                        listDisable[i].SetActive(show);
                    else
                        listDisable[i].SetActive(false);
                }
            }
        }

        public void ShowEffect(int index, bool show)
        {
            if (null != effect)
            {
                for (int i = 0; i < effect.Length; i++)
                {
                    if (index == i)
                        effect[i].SetActive(show);
                    else
                        effect[i].SetActive(false);
                }
            }   
        }

        public void HideAll()
        {
            interactable = false;

            if (null != enable)
            {
                showEnableBackup = enable.activeSelf;
                enable.SetActive(false);
            }
            if (null != disable)
            {
                showDisableBackup = disable.activeSelf;
                disable.SetActive(false);
            }
            if (null != reddot)
            {
                showReddotBackup = reddot.activeSelf;
                reddot.SetActive(false);
            }
            if (null != listEnable)
            {
                for (int i = 0; i < listEnable.Length; i++)
                {
                    showlistEnableBackup[i] = listEnable[i].activeSelf;
                    listEnable[i].SetActive(false);
                }
            }
            if (null != listDisable)
            {
                for (int i = 0; i < listDisable.Length; i++)
                {
                    showlistDisableBackup[i] = listDisable[i].activeSelf;
                    listDisable[i].SetActive(false);
                }
            }
            if (null != effect)
            {
                for (int i = 0; i < effect.Length; i++)
                {
                    showEffectBackup[i] = effect[i].activeSelf;
                    effect[i].SetActive(false);
                }
            }
        }

        public void RollBackAll()
        {
            interactable = true;

            if (null != enable)
                enable.SetActive(showEnableBackup);
            if (null != disable)
                disable.SetActive(showDisableBackup);
            if (null != reddot)
                reddot.SetActive(showReddotBackup);

            if (null != listEnable)
            {
                for (int i = 0; i < listEnable.Length; i++)
                    listEnable[i].SetActive(showlistEnableBackup[i]);
            }
            if (null != listDisable)
            {
                for (int i = 0; i < listDisable.Length; i++)
                    listDisable[i].SetActive(showlistDisableBackup[i]);
            }
            if (null != effect)
            {
                for (int i = 0; i < effect.Length; i++)
                    effect[i].SetActive(showEffectBackup[i]);
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ButtonEx), true)]
    [CanEditMultipleObjects]
    public class ButtonExEditor : ButtonEditor
    {
        SerializedProperty m_enableProperty;
        SerializedProperty m_disableProperty;
        SerializedProperty m_reddotProperty;
        SerializedProperty m_listEnableProperty;
        SerializedProperty m_listDisableProperty;
        SerializedProperty m_effectProperty;


        protected override void OnEnable()
        {
            base.OnEnable();

            m_enableProperty = serializedObject.FindProperty("enable");
            m_disableProperty = serializedObject.FindProperty("disable");
            m_reddotProperty = serializedObject.FindProperty("reddot");
            m_listEnableProperty = serializedObject.FindProperty("listEnable");
            m_listDisableProperty = serializedObject.FindProperty("listDisable");
            m_effectProperty = serializedObject.FindProperty("effect");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            // Draw the event notification options
            EditorGUILayout.PropertyField(m_enableProperty);
            EditorGUILayout.PropertyField(m_disableProperty);
            EditorGUILayout.PropertyField(m_reddotProperty);
            EditorGUILayout.PropertyField(m_listEnableProperty);
            EditorGUILayout.PropertyField(m_listDisableProperty);
            EditorGUILayout.PropertyField(m_effectProperty);           

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}