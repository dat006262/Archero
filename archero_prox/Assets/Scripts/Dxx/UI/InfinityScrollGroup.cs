




using Dxx.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Dxx.UI
{
	public class InfinityScrollGroup : UIBehaviour, ILayoutElement
	{
		protected interface ComponentAction
		{
			void Invoke(int index, Component component);
		}

		protected class ChildComponentAction<T> : ComponentAction where T : Component
		{
			public Action<int, T> callBack;

			public ChildComponentAction(Action<int, T> callBack)
			{
				this.callBack = callBack;
			}

			public void Invoke(int index, Component component)
			{
				if (callBack != null)
				{
					callBack(index, component as T);
				}
			}
		}

		public enum Axis
		{
			Horizontal,
			Vertical
		}

		[Header("<Infinity>")]
		[AliasName("子物体", true)]
		[Tooltip("复制用子物体.")]
		public GameObject copyItemChild;

		[AliasName("数据个数", true)]
		[Tooltip("数据个数.必须大于0")]
		public int itemCount;

		public ScrollRect scrollRect;

		[Header("<Layout>")]
		public RectOffset padding = new RectOffset();

		public Vector2 cellSize = new Vector2(100f, 100f);

		public Vector2 spacing = Vector2.zero;

		public Axis sortAxis;

		[Tooltip("必须大于1")]
		public int constraintCount = 1;

		[Header("滚动最小长度，0为不限制")]
		public float MinScrollLength;

		protected Action<int, GameObject> updateChildCallBack;

		protected Dictionary<Type, ComponentAction> updateChildComponentCallBack = new Dictionary<Type, ComponentAction>();

		public Action<Vector2> onSizeChange;

		protected List<RectTransform> rectChildren = new List<RectTransform>();

		protected Dictionary<GameObject, Component[]> objToCompnent = new Dictionary<GameObject, Component[]>();

		protected DrivenRectTransformTracker m_Tracker;

		private int lastRowIndex;

		[NonSerialized]
		private RectTransform m_Rect;

		private List<GameObject> childCache = new List<GameObject>();

		private Vector2 m_TotalMinSize = Vector2.zero;

		private Vector2 m_TotalPreferredSize = Vector2.zero;

		private Vector2 m_TotalFlexibleSize = Vector2.zero;

		public int displayItemCount => rectChildren.Count;

		public int displayMaxRow => (displayItemCount / constraintCount + displayItemCount % constraintCount > 0) ? 1 : 0;

		public int itemMaxRow => itemCount / constraintCount + ((itemCount % constraintCount > 0) ? 1 : 0);

		public RectTransform rectTransform
		{
			get
			{
				if (m_Rect == null)
				{
					m_Rect = GetComponent<RectTransform>();
				}
				return m_Rect;
			}
		}

		public virtual float minWidth => GetTotalMinSize(0);

		public virtual float preferredWidth => GetTotalPreferredSize(0);

		public virtual float flexibleWidth => GetTotalFlexibleSize(0);

		public virtual float minHeight => GetTotalMinSize(1);

		public virtual float preferredHeight => GetTotalPreferredSize(1);

		public virtual float flexibleHeight => GetTotalFlexibleSize(1);

		public virtual int layoutPriority => 0;

		protected override void Awake()
		{
			scrollRect.onValueChanged.AddListener(delegate(Vector2 edata)
			{
				Scroll(edata);
			});
			base.Awake();
		}

		public void Init(int displayCount, int itemCount, GameObject copyItemChild = null)
		{
			while (rectChildren.Count > 0)
			{
				UnityEngine.Object.Destroy(rectChildren[0].gameObject);
				rectChildren.RemoveAt(0);
			}
			rectChildren.Clear();
			if (copyItemChild != null)
			{
				this.copyItemChild = copyItemChild;
			}
			SetDisplayCount(displayCount);
			SetItemCount(itemCount);
			UpdateLayout();
		}

		public void RefreshAll()
		{
			UpdateLayoutChildren(callUpdate: false, callUpdateAlways: true);
		}

		private void UpdateChildListCallback(int index, GameObject obj)
		{
			if (updateChildCallBack != null)
			{
				updateChildCallBack(index, obj);
			}
			if (objToCompnent[obj] == null)
			{
				return;
			}
			for (int i = 0; i < objToCompnent[obj].Length; i++)
			{
				Component component = objToCompnent[obj][i];
				Type type = component.GetType();
				if (updateChildComponentCallBack.ContainsKey(type))
				{
					updateChildComponentCallBack[type].Invoke(index, component);
				}
			}
		}

		public void RegUpdateCallback(Action<int, GameObject> callBack)
		{
			updateChildCallBack = (Action<int, GameObject>)Delegate.Combine(updateChildCallBack, callBack);
		}

		public void UnRegUpdateCallback(Action<int, GameObject> callBack)
		{
			updateChildCallBack = (Action<int, GameObject>)Delegate.Remove(updateChildCallBack, callBack);
		}

		public void RegUpdateCallback<T>(Action<int, T> callBack) where T : Component
		{
			Type typeFromHandle = typeof(T);
			if (!updateChildComponentCallBack.ContainsKey(typeFromHandle))
			{
				updateChildComponentCallBack.Add(typeFromHandle, new ChildComponentAction<T>(callBack));
				return;
			}
			ChildComponentAction<T> obj = updateChildComponentCallBack[typeFromHandle] as ChildComponentAction<T>;
			obj.callBack = (Action<int, T>)Delegate.Combine(obj.callBack, callBack);
		}

		public void UnRegUpdateCallback<T>(Action<int, T> callBack) where T : Component
		{
			Type typeFromHandle = typeof(T);
			if (updateChildComponentCallBack.ContainsKey(typeFromHandle))
			{
				ChildComponentAction<T> obj = updateChildComponentCallBack[typeFromHandle] as ChildComponentAction<T>;
				obj.callBack = (Action<int, T>)Delegate.Remove(obj.callBack, callBack);
			}
		}

		private void UpdateLayout()
		{
			m_Tracker.Clear();
			for (int i = 0; i < rectChildren.Count; i++)
			{
				m_Tracker.Add(this, rectChildren[i], DrivenTransformProperties.AnchoredPositionX | DrivenTransformProperties.AnchoredPositionY | DrivenTransformProperties.AnchorMinX | DrivenTransformProperties.AnchorMinY | DrivenTransformProperties.AnchorMaxX | DrivenTransformProperties.AnchorMaxY | DrivenTransformProperties.SizeDeltaX | DrivenTransformProperties.SizeDeltaY | DrivenTransformProperties.PivotX | DrivenTransformProperties.PivotY);
				rectChildren[i].anchorMin = Vector2.up;
				rectChildren[i].anchorMax = Vector2.up;
				rectChildren[i].sizeDelta = cellSize;
				rectChildren[i].pivot = new Vector2(0f, 1f);
			}
			m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDelta);
		}

		private void UpdateLayoutChildren(bool callUpdate = true, bool callUpdateAlways = false)
		{
			Vector2 vector;
			if (sortAxis == Axis.Horizontal)
			{
				int num = lastRowIndex;
				vector = default(Vector2);
				vector.x = padding.left;
				vector.y = (float)(-padding.top) - (cellSize.y + spacing.y) * (float)num;
				Vector2 vector2 = vector;
				for (int i = 0; i < rectChildren.Count; i++)
				{
					int num2 = i % constraintCount;
					int num3 = i / constraintCount;
					float x = vector2.x + (float)num2 * (cellSize.x + spacing.x);
					float y = vector2.y - (float)num3 * (cellSize.y + spacing.y);
					rectChildren[i].anchoredPosition = new Vector2(x, y);
					rectChildren[i].SetSiblingIndex(i);
					int num4 = num * constraintCount + i;
					rectChildren[i].gameObject.name = Utils.FormatString("Item_({0})", num4);
					if (num4 < itemCount && num4 >= 0)
					{
						if (!rectChildren[i].gameObject.activeSelf)
						{
							rectChildren[i].gameObject.SetActive(value: true);
							UpdateChildListCallback(num4, rectChildren[i].gameObject);
						}
						if (callUpdateAlways)
						{
							UpdateChildListCallback(num4, rectChildren[i].gameObject);
						}
					}
					else
					{
						rectChildren[i].gameObject.SetActive(value: false);
					}
				}
			}
			else if (sortAxis == Axis.Vertical)
			{
				int num5 = lastRowIndex;
				vector = default(Vector2);
				vector.x = (float)padding.left + (cellSize.x + spacing.x) * (float)num5;
				vector.y = -padding.top;
				Vector2 vector3 = vector;
				for (int j = 0; j < rectChildren.Count; j++)
				{
					int num6 = j / constraintCount;
					int num7 = j % constraintCount;
					float x2 = vector3.x + (float)num6 * (cellSize.x + spacing.x);
					float y2 = vector3.y - (float)num7 * (cellSize.y + spacing.y);
					rectChildren[j].anchoredPosition = new Vector2(x2, y2);
					rectChildren[j].SetSiblingIndex(j);
					int num8 = num5 * constraintCount + j;
					rectChildren[j].gameObject.name = Utils.FormatString("Item_({0})", num8);
					if (num8 < itemCount && num8 >= 0)
					{
						if (!rectChildren[j].gameObject.activeSelf)
						{
							rectChildren[j].gameObject.SetActive(value: true);
							UpdateChildListCallback(num8, rectChildren[j].gameObject);
						}
						if (callUpdateAlways)
						{
							UpdateChildListCallback(num8, rectChildren[j].gameObject);
						}
					}
					else
					{
						rectChildren[j].gameObject.SetActive(value: false);
					}
				}
			}
			UpdateLayoutContent();
		}

		private void UpdateLayoutContent()
		{
			if (sortAxis == Axis.Horizontal)
			{
				float num = (float)padding.horizontal + (float)constraintCount * (cellSize.x + spacing.x) - spacing.x;
				float num2 = (float)padding.vertical + (float)itemMaxRow * (cellSize.y + spacing.y) - spacing.y;
				if (MinScrollLength > 0f && num2 < MinScrollLength)
				{
					num2 = MinScrollLength;
				}
				SetLayoutInputForAxis(num, num, -1f, 0);
				SetLayoutInputForAxis(num2, num2, -1f, 1);
			}
			else if (sortAxis == Axis.Vertical)
			{
				float num3 = (float)padding.horizontal + (float)itemMaxRow * (cellSize.x + spacing.x) - spacing.x;
				float num4 = (float)padding.vertical + (float)constraintCount * (cellSize.y + spacing.y) - spacing.y;
				if (MinScrollLength > 0f && num3 < MinScrollLength)
				{
					num3 = MinScrollLength;
				}
				SetLayoutInputForAxis(num3, num3, -1f, 0);
				SetLayoutInputForAxis(num4, num4, -1f, 1);
			}
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, LayoutUtility.GetPreferredSize(rectTransform, 0));
			rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, LayoutUtility.GetPreferredSize(rectTransform, 1));
			if (onSizeChange != null)
			{
				onSizeChange(new Vector2(preferredWidth, preferredHeight));
			}
		}

		private int PositionToRealIndex(Vector2 pos)
		{
			Vector2Int vector2Int = PositionToGrid(pos);
			if (sortAxis == Axis.Horizontal)
			{
				return vector2Int.y * constraintCount + vector2Int.x;
			}
			if (sortAxis == Axis.Vertical)
			{
				return vector2Int.x * constraintCount + vector2Int.y;
			}
			return 0;
		}

		private Vector2Int PositionToGrid(Vector2 pos)
		{
			int x = (int)((pos.x - (float)padding.left) / (cellSize.x + spacing.x));
			int y = (int)((0f - pos.y - (float)padding.top) / (cellSize.y + spacing.y));
			return new Vector2Int(x, y);
		}

		private float ContentPositionToRowIndex(Vector2 pos)
		{
			Vector2 vector = default(Vector2);
			vector.x = 0f - pos.x - (float)padding.left;
			vector.y = pos.y - (float)padding.top;
			Vector2 vector2 = vector;
			float result = vector2.x / (cellSize.x + spacing.x);
			float result2 = vector2.y / (cellSize.y + spacing.y);
			if (sortAxis == Axis.Horizontal)
			{
				return result2;
			}
			if (sortAxis == Axis.Vertical)
			{
				return result;
			}
			return 0f;
		}

		public void ScrollToItem(int itemIndex)
		{
			if (itemIndex < 0 || itemIndex >= itemCount)
			{
				return;
			}
			if (sortAxis == Axis.Horizontal)
			{
				int num = itemIndex / constraintCount;
				float num2 = (float)padding.top + (cellSize.y + spacing.y) * (float)num;
				float height = scrollRect.viewport.rect.height;
				Vector2 anchoredPosition = scrollRect.content.anchoredPosition;
				if (num2 > anchoredPosition.y || num2 < anchoredPosition.y + height)
				{
					scrollRect.content.anchoredPosition = new Vector2(anchoredPosition.x, num2);
					StartCoroutine(ScrollToItemImpl());
				}
			}
			else if (sortAxis == Axis.Vertical)
			{
				int num3 = itemIndex / constraintCount;
				float num4 = (float)(-padding.left) - (cellSize.x + spacing.x) * (float)num3;
				float width = scrollRect.viewport.rect.width;
				Vector2 anchoredPosition2 = scrollRect.content.anchoredPosition;
				if (num4 < anchoredPosition2.x || num4 > anchoredPosition2.x - width)
				{
					scrollRect.content.anchoredPosition = new Vector2(num4, anchoredPosition2.y);
					StartCoroutine(ScrollToItemImpl());
				}
			}
		}

		private IEnumerator ScrollToItemImpl()
		{
			yield return new WaitForEndOfFrame();
			UpdateLayoutChildren(callUpdate: false, callUpdateAlways: true);
		}

		private void ScrollChild(int indexCount)
		{
			if (indexCount > 0)
			{
				while (indexCount-- > 0)
				{
					for (int i = 0; i < constraintCount; i++)
					{
						RectTransform rectTransform = rectChildren[0];
						rectChildren.RemoveAt(0);
						rectChildren.Add(rectTransform);
						int num2 = PositionToRealIndex(rectTransform.anchoredPosition);
						int num3 = num2 + displayItemCount;
						if (num3 >= 0 && num3 < itemCount)
						{
							UpdateChildListCallback(num3, rectTransform.gameObject);
						}
					}
				}
			}
			else
			{
				if (indexCount >= 0)
				{
					return;
				}
				while (indexCount++ < 0)
				{
					for (int j = 0; j < constraintCount; j++)
					{
						RectTransform rectTransform2 = rectChildren[rectChildren.Count - 1];
						rectChildren.RemoveAt(rectChildren.Count - 1);
						rectChildren.Insert(0, rectTransform2);
						int num5 = PositionToRealIndex(rectTransform2.anchoredPosition);
						int num6 = num5 - displayItemCount;
						if (num6 >= 0 && num6 < itemCount)
						{
							UpdateChildListCallback(num6, rectTransform2.gameObject);
						}
					}
				}
			}
		}

		protected void Scroll(Vector2 value)
		{
			int a = (int)ContentPositionToRowIndex(scrollRect.content.anchoredPosition);
			a = Mathf.Min(Mathf.Max(a, 0), itemMaxRow - displayMaxRow);
			if (a != lastRowIndex)
			{
				ScrollChild(a - lastRowIndex);
				lastRowIndex = a;
				UpdateLayoutChildren();
			}
		}

		private void DestroyChild(RectTransform child)
		{
			rectChildren.Remove(child);
			child.gameObject.SetActive(value: false);
			childCache.Add(child.gameObject);
		}

		private void CreateNewDisplayChild()
		{
			GameObject gameObject;
			if (childCache.Count > 0)
			{
				gameObject = childCache[0];
				childCache.RemoveAt(0);
			}
			else
			{
				gameObject = UnityEngine.Object.Instantiate(copyItemChild);
				objToCompnent.Add(gameObject, gameObject.GetComponents<Component>());
				gameObject.SetActive(value: false);
			}
			if (gameObject.transform is RectTransform)
			{
				gameObject.transform.SetParent(rectTransform, worldPositionStays: false);
				rectChildren.Add(gameObject.transform as RectTransform);
				return;
			}
			throw new Exception($"copyItem({copyItemChild.name}) is not a RectTransform type");
		}

		private void SetDisplayCount(int newCount)
		{
			if (displayItemCount < newCount)
			{
				while (displayItemCount < newCount)
				{
					CreateNewDisplayChild();
				}
			}
			else if (displayItemCount > newCount)
			{
				while (displayItemCount > newCount)
				{
					DestroyChild(rectChildren[0]);
				}
			}
		}

		public void SetItemCount(int newCount, bool callUpdate = true)
		{
			itemCount = newCount;
			UpdateLayoutChildren(callUpdate);
		}

		public virtual void CalculateLayoutInputHorizontal()
		{
		}

		public virtual void CalculateLayoutInputVertical()
		{
		}

		protected float GetTotalMinSize(int axis)
		{
			return m_TotalMinSize[axis];
		}

		protected float GetTotalPreferredSize(int axis)
		{
			return m_TotalPreferredSize[axis];
		}

		protected float GetTotalFlexibleSize(int axis)
		{
			return m_TotalFlexibleSize[axis];
		}

		protected void SetLayoutInputForAxis(float totalMin, float totalPreferred, float totalFlexible, int axis)
		{
			m_TotalMinSize[axis] = totalMin;
			m_TotalPreferredSize[axis] = totalPreferred;
			m_TotalFlexibleSize[axis] = totalFlexible;
		}
	}
}
