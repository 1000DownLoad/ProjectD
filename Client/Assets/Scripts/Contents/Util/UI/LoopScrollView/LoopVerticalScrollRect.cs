using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.EventSystems;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Loop Vertical Scroll Rect", 51)]
    [DisallowMultipleComponent]
    public class LoopVerticalScrollRect : LoopScrollRect
    {
        public Action<int, int, Bounds, Bounds> Callback;

        protected override float GetSize(RectTransform item)
        {
            float size = contentSpacing;
            if (m_GridLayout != null)
            {
                size += m_GridLayout.cellSize.y;
            }
            else
            {
                var add = item.rect.height;
                if (0 == add)
                {
                    if (0 == item.childCount)
                    {
                        add = LayoutUtility.GetPreferredHeight(item);
                    }
                    else
                    {
                        for (int i = 0; i < item.childCount; i++)
                        {
                            var child = item.GetChild(i);
                            if (false == child.gameObject.activeSelf)
                                continue;

                            add = (child as RectTransform).rect.height;
                            break;
                        }
                    }
                }

                size += add;
            }
            return size;
        }
        protected override int GetConstraintCount()
        {
            if (content != null)
            {
                if (content.TryGetComponent(out m_GridLayout))
                {
                    if (m_GridLayout.constraint != GridLayoutGroup.Constraint.Flexible)
                        return m_GridLayout.constraintCount;
                    else
                    {
                        float size = m_GridLayout.spacing.x + m_GridLayout.cellSize.x;

                        var rect = (RectTransform)m_GridLayout.transform;

                        return Mathf.FloorToInt(rect.rect.width / size);
                    }
                }
            }

            return 1;
        }
        protected override float GetDimension(Vector2 vector)
        {
            return vector.y;
        }

        protected override Vector2 GetVector(float value)
        {
            return new Vector2(0, value);
        }

        protected override void Awake()
        {
            base.Awake();
            directionSign = -1;

            GridLayoutGroup layout = content.GetComponent<GridLayoutGroup>();
            if (layout != null && layout.constraint != GridLayoutGroup.Constraint.FixedColumnCount)
            {
                //Debug.LogError("[LoopHorizontalScrollRect] unsupported GridLayoutGroup constraint");
            }
        }

        public override void UpdatePollTotalCount(int _totalCount)
        {
            base.UpdatePollTotalCount(_totalCount);
            directionSign = -1; // Awake 가 skip 되는 경우가 있다. // prefab, scene 생성할 때 gameObject 가 active false 인 경우 호출되지 않는다 // https://answers.unity.com/questions/932269/skip-awakestart.html
        }


        protected override bool UpdateItems(Bounds viewBounds, Bounds contentBounds)
        {
            bool changed = false;
            if (viewBounds.min.y < contentBounds.min.y)
            {
                float size = NewItemAtEnd(), totalSize = size;
                while(size > 0 && viewBounds.min.y < contentBounds.min.y - totalSize)
                {
                    size = NewItemAtEnd();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }
            else if (viewBounds.min.y > contentBounds.min.y + threshold)
            {
                float size = DeleteItemAtEnd(), totalSize = size;
                while (size > 0 && viewBounds.min.y > contentBounds.min.y + threshold + totalSize)
                {
                    size = DeleteItemAtEnd();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }

            if (viewBounds.max.y > contentBounds.max.y)
            {
                float size = NewItemAtStart(), totalSize = size;
                while (size > 0 && viewBounds.max.y > contentBounds.max.y + totalSize)
                {
                    size = NewItemAtStart();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }
            else if (viewBounds.max.y < contentBounds.max.y - threshold)
            {
                float size = DeleteItemAtStart(), totalSize = size;
                while (size > 0 && viewBounds.max.y < contentBounds.max.y - threshold - totalSize)
                {
                    size = DeleteItemAtStart();
                    totalSize += size;
                }

                if (totalSize > 0)
                    changed = true;
            }

            Callback?.Invoke(itemTypeStart, itemTypeEnd, viewBounds, contentBounds);

            return changed;
        }
    }
}