using UnityEngine;
using System.Collections;

namespace UnityEngine.UI
{
    [AddComponentMenu("UI/Loop Horizontal Scroll Rect", 50)]
    [DisallowMultipleComponent]
    public class LoopHorizontalScrollRect : LoopScrollRect
    {
        protected override float GetSize(RectTransform item)
        {
            float size = contentSpacing;
            if (m_GridLayout != null)
            {
                size += m_GridLayout.cellSize.x;
            }
            else
            {
                var add = item.rect.width;
                if (0 == add)
                {
                    if (0 == item.childCount)
                    {
                        add = LayoutUtility.GetPreferredWidth(item);
                    }
                    else
                    {
                        for (int i = 0; i < item.childCount; i++)
                        {
                            var child = item.GetChild(i);
                            if (false == child.gameObject.activeSelf)
                                continue;

                            add = (child as RectTransform).rect.width;
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
                m_GridLayout = content.GetComponent<GridLayoutGroup>();
                if (m_GridLayout != null)
                {
                    if (m_GridLayout.constraint != GridLayoutGroup.Constraint.Flexible)
                        return m_GridLayout.constraintCount;
                    else
                    {
                        float size = m_GridLayout.spacing.y + m_GridLayout.cellSize.y;

                        var rect = (RectTransform)m_GridLayout.transform;

                        return Mathf.FloorToInt(rect.rect.height / size);
                    }
                }
            }

            return 1;
        }


        protected override float GetDimension(Vector2 vector)
        {
            return -vector.x;
        }

        protected override Vector2 GetVector(float value)
        {
            return new Vector2(-value, 0);
        }

        protected override void Awake()
        {
            base.Awake();
            directionSign = 1;
        }

        public override void UpdatePollTotalCount(int _totalCount)
        {
            base.UpdatePollTotalCount(_totalCount);
            directionSign = 1; // Awake 가 skip 되는 경우가 있다. // prefab, scene 생성할 때 gameObject 가 active false 인 경우 호출되지 않는다 // https://answers.unity.com/questions/932269/skip-awakestart.html
        }

        protected override bool UpdateItems(Bounds viewBounds, Bounds contentBounds)
        {
            bool changed = false;
            if (viewBounds.max.x > contentBounds.max.x)
            {
                float size = NewItemAtEnd(), totalSize = size;
                while (size > 0 && viewBounds.max.x > contentBounds.max.x + totalSize)
                {
                    size = NewItemAtEnd();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }
            else if (viewBounds.max.x < contentBounds.max.x - threshold)
            {
                float size = DeleteItemAtEnd(), totalSize = size;
                while (size > 0 && viewBounds.max.x < contentBounds.max.x - threshold - totalSize)
                {
                    size = DeleteItemAtEnd();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }

            if (viewBounds.min.x < contentBounds.min.x)
            {
                float size = NewItemAtStart(), totalSize = size;
                while (size > 0 && viewBounds.min.x < contentBounds.min.x - totalSize)
                {
                    size = NewItemAtStart();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }
            else if (viewBounds.min.x > contentBounds.min.x + threshold)
            {
                float size = DeleteItemAtStart(), totalSize = size;
                while (size > 0 && viewBounds.min.x > contentBounds.min.x + threshold + totalSize)
                {
                    size = DeleteItemAtStart();
                    totalSize += size;
                }
                if (totalSize > 0)
                    changed = true;
            }
            return changed;
        }
    }
}