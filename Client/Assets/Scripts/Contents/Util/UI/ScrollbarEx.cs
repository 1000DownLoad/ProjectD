#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.UI;
#endif
using UniRx;
using System.Collections.Generic;


namespace UnityEngine.UI
{
	public class ScrollbarEx : Scrollbar
	{
        [SerializeField] Toggle m_ToggleBase;
        [SerializeField] Transform poolRoot;

        ObjectPool<Toggle> pool;
        List<Toggle> listToggle = new List<Toggle>();
        LoopScrollRect scroll;
        readonly CompositeDisposable disposable = new CompositeDisposable();

        protected override void OnDestroy()
        {
            disposable.Dispose();
            scroll = null;
            listToggle.Clear();
            if (null != pool)
                pool.Clear();
        }

        public void SetToggle(LoopScrollRect scroll)
        {
            if (null == pool)
                pool = new ObjectPool<Toggle>(m_ToggleBase, poolRoot, transform);
            pool.ReturnAllList();
            listToggle.Clear();
            disposable.Clear();

            this.scroll = scroll;

            for (int i = 0; i < this.scroll.totalCount; i++)
            {
                var item = pool.Rent();
                item.gameObject.name = i.ToString();
                listToggle.Add(item);
            }

            foreach (var item in listToggle)
            {
                item.OnValueChangedAsObservable().DistinctUntilChanged().Where(isOn => isOn).Subscribe((isOn) =>
                {
                    this.scroll.ScrollToCell(int.Parse(item.gameObject.name), false);

                }).AddTo(disposable);
            }

            if (0 != listToggle.Count)
            {
                listToggle[0].group.SetAllTogglesOff();
                listToggle[0].isOn = true;
            }

            numberOfSteps = this.scroll.totalCount;
        }

        public void UpdateScrollbars(float normalizedPosition)
        {
            if (0 != listToggle.Count)
            {
                var val = Mathf.Round(normalizedPosition * (numberOfSteps - 1));
                listToggle[(int)val].isOn = true;
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(ScrollbarEx), true)]
    [CanEditMultipleObjects]
    public class ScrollbarExEditor : ScrollbarEditor
    {
        SerializedProperty m_ToggleBase;
        SerializedProperty poolRoot;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_ToggleBase = serializedObject.FindProperty("m_ToggleBase");
            poolRoot = serializedObject.FindProperty("poolRoot");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();

            // EditorGUILayout.PropertyField(m_ToggleBase);
            EditorGUI.BeginChangeCheck();
            Toggle newToggle = EditorGUILayout.ObjectField("Toggle", m_ToggleBase.objectReferenceValue, typeof(Toggle), true) as Toggle;
            if (EditorGUI.EndChangeCheck())
                m_ToggleBase.objectReferenceValue = newToggle;

            EditorGUI.BeginChangeCheck();
            Transform newpoolRoot = EditorGUILayout.ObjectField("poolRoot", poolRoot.objectReferenceValue, typeof(Transform), true) as Transform;
            if (EditorGUI.EndChangeCheck())
                poolRoot.objectReferenceValue = newpoolRoot;

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}