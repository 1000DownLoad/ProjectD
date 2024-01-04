#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif


namespace UnityEngine.UI
{
	public class ToggleEx : Toggle
	{
        [SerializeField] GameObject reddot;
        public ToggleEvent onValueChangedInverse = new ToggleEvent();

        bool showReddotBackup = false;

        protected ToggleEx()
        {
            onValueChanged.AddListener((on) => { this.onValueChangedInverse.Invoke(!on); });
        }

        public void ShowReddot(bool show)
        {
            if (null != reddot)
                reddot.SetActive(show);
        }

        public void HideAll()
        {
            if (null != reddot)
            {
                showReddotBackup = reddot.activeSelf;
                reddot.SetActive(false);
            }
        }

        public void RollBackAll()
        {
            if (null != reddot)
                reddot.SetActive(showReddotBackup);
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ToggleEx), true)]
    [CanEditMultipleObjects]
    public class ToggleExEditor : ToggleEditor
    {
        SerializedProperty m_OnValueChangedInverseProperty;
        SerializedProperty m_reddotProperty;


        protected override void OnEnable()
        {
            base.OnEnable();

            m_OnValueChangedInverseProperty = serializedObject.FindProperty("onValueChangedInverse");
            m_reddotProperty = serializedObject.FindProperty("reddot");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.Space();

            // Draw the event notification options
            EditorGUILayout.PropertyField(m_OnValueChangedInverseProperty);
            EditorGUILayout.PropertyField(m_reddotProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}