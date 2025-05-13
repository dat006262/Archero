




using Dxx.Util;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ScrollRectBase : ScrollRect, IPointerClickHandler, IEventSystemHandler
{
	public Action<PointerEventData> BeginDrag;

	public Action<PointerEventData> Drag;

	public Action<PointerEventData> EndDrag;

	public Action OnClick;

	public Action<int> EndDragItem;

	public Action<Vector2> ValueChanged;

	private float scrollpercent;

	public bool UseWhole;

	private bool _usegrag = true;

	public bool DragDisableForce;

	public bool bUseScrollEvent = true;

	public float SpeedCritical = 20f;

	public float Whole_PerOne;

	public int Whole_Count;

	public float AllWidth;

	private bool _dragging;

	private bool _sendfinish;

	private bool bUpdateEnd;

	private float speed;

	private int currentPage;

	private float scrollendpos;

	private Action mPageAniFinish;

	private bool[] mLocks;

	private bool bGotoStart;

	private float mGotoValue;

	private float mGotoTemp;

	public bool UseDrag
	{
		get
		{
			if (DragDisableForce)
			{
				return false;
			}
			return _usegrag;
		}
		set
		{
			_usegrag = value;
			if (!value)
			{
				bDragging = false;
			}
		}
	}

	private bool bDragging
	{
		get
		{
			return _dragging;
		}
		set
		{
			_dragging = value;
			if (_dragging)
			{
				bSendFinish = false;
			}
		}
	}

	private bool bSendFinish
	{
		get
		{
			return _sendfinish;
		}
		set
		{
			_sendfinish = value;
		}
	}

	protected override void Awake()
	{
		base.Awake();
		base.onValueChanged = new ScrollRectEvent();
		base.onValueChanged.AddListener(OnValueChanged);
	}

	public void RemoveAllListeners()
	{
		if (base.onValueChanged != null)
		{
			base.onValueChanged.RemoveAllListeners();
		}
	}

	protected override void OnEnable()
	{
		OnEnableWhole();
	}

	protected override void OnDisable()
	{
		OnDisableWhole();
	}

	private void Update()
	{
		if (UseWhole)
		{
			OnUpdateWhole();
		}
		OnUpdateGoto();
	}

	public void SetLocks(bool[] locks)
	{
		mLocks = locks;
	}

	private int GetNextUnlock(int currentindex, bool left)
	{
		if (mLocks == null)
		{
			if (left)
			{
				return currentindex - 1;
			}
			return currentindex + 1;
		}
		int result = currentindex;
		int num = (!left) ? 1 : (-1);
		currentindex += num;
		while (currentindex >= 0 && currentindex < mLocks.Length)
		{
			if (mLocks[currentindex])
			{
				currentindex += num;
				continue;
			}
			return currentindex;
		}
		return result;
	}

	private void OnUpdateGoto()
	{
		if (bGotoStart)
		{
			if (MathDxx.Abs(mGotoTemp - mGotoValue) < 3f)
			{
				bGotoStart = false;
			}
			if (base.horizontal)
			{
				Vector2 anchoredPosition = base.content.anchoredPosition;
				mGotoTemp = Mathf.Lerp(anchoredPosition.x, mGotoValue, 0.2f);
				RectTransform content = base.content;
				float x = mGotoTemp;
				Vector2 anchoredPosition2 = base.content.anchoredPosition;
				content.anchoredPosition = new Vector2(x, anchoredPosition2.y);
			}
			else
			{
				Vector2 anchoredPosition3 = base.content.anchoredPosition;
				mGotoTemp = Mathf.Lerp(anchoredPosition3.y, mGotoValue, 0.2f);
				RectTransform content2 = base.content;
				Vector2 anchoredPosition4 = base.content.anchoredPosition;
				content2.anchoredPosition = new Vector2(anchoredPosition4.x, mGotoTemp);
			}
		}
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		if (bUseScrollEvent && UseDrag)
		{
			OnBeginDragInternal(eventData);
		}
	}

	public void OnBeginDragInternal(PointerEventData eventData)
	{
		base.OnBeginDrag(eventData);
		if (UseWhole)
		{
			OnBeginDragWhole(eventData);
		}
		if (BeginDrag != null)
		{
			BeginDrag(eventData);
		}
	}

	public override void OnDrag(PointerEventData eventData)
	{
		if (bUseScrollEvent && UseDrag)
		{
			OnDragInternal(eventData);
		}
	}

	public void OnDragInternal(PointerEventData eventData)
	{
		base.OnDrag(eventData);
		if (UseWhole)
		{
			OnDragWhole(eventData);
		}
		if (Drag != null)
		{
			Drag(eventData);
		}
	}

	public override void OnEndDrag(PointerEventData eventData)
	{
		if (bUseScrollEvent && UseDrag)
		{
			OnEndDragInternal(eventData);
		}
	}

	public void OnEndDragInternal(PointerEventData eventData)
	{
		base.OnEndDrag(eventData);
		if (UseWhole)
		{
			OnEndDragWhole(eventData);
		}
		if (EndDrag != null)
		{
			EndDrag(eventData);
		}
	}

	private void OnValueChanged(Vector2 value)
	{
		scrollpercent = value.x;
		if (ValueChanged != null)
		{
			ValueChanged(value);
		}
	}

	public void Goto(float value, bool playanimation = false)
	{
		if (!playanimation)
		{
			if (base.horizontal)
			{
				RectTransform content = base.content;
				Vector2 anchoredPosition = base.content.anchoredPosition;
				content.anchoredPosition = new Vector2(value, anchoredPosition.y);
				Vector2 anchoredPosition2 = base.content.anchoredPosition;
				mGotoTemp = anchoredPosition2.x;
			}
			else
			{
				RectTransform content2 = base.content;
				Vector2 anchoredPosition3 = base.content.anchoredPosition;
				content2.anchoredPosition = new Vector2(anchoredPosition3.x, value);
				Vector2 anchoredPosition4 = base.content.anchoredPosition;
				mGotoTemp = anchoredPosition4.y;
			}
		}
		else
		{
			if (base.horizontal)
			{
				Vector2 anchoredPosition5 = base.content.anchoredPosition;
				mGotoTemp = anchoredPosition5.x;
			}
			else
			{
				Vector2 anchoredPosition6 = base.content.anchoredPosition;
				mGotoTemp = anchoredPosition6.y;
			}
			mGotoValue = value;
			bGotoStart = true;
		}
	}

	private void OnEnableWhole()
	{
	}

	private void OnDisableWhole()
	{
	}

	public void SetWhole(GridLayoutGroup grid, int count)
	{
		UseWhole = true;
		if (base.horizontal)
		{
			Vector2 cellSize = grid.cellSize;
			Whole_PerOne = cellSize.x;
		}
		else
		{
			Vector2 cellSize2 = grid.cellSize;
			Whole_PerOne = cellSize2.y;
		}
		Whole_Count = count;
		AllWidth = Whole_PerOne * (float)(Whole_Count - 1);
		base.content.sizeDelta = new Vector2(AllWidth, 0f);
	}

	public void SetPage(int page, bool animate, Action onFinish = null)
	{
		mPageAniFinish = onFinish;
		currentPage = page;
		if (!animate)
		{
			if (base.horizontal)
			{
				UpdateScrollEndPos();
				base.horizontalNormalizedPosition = scrollendpos;
			}
			if (base.vertical)
			{
				UpdateScrollEndPos();
				base.verticalNormalizedPosition = scrollendpos;
			}
		}
		else
		{
			bSendFinish = false;
			bUpdateEnd = false;
		}
		UpdateScrollEndPos();
	}

	private void OnBeginDragWhole(PointerEventData eventData)
	{
		bDragging = true;
	}

	private void OnDragWhole(PointerEventData eventData)
	{
		Vector2 delta = eventData.delta;
		speed = delta.x;
	}

	private void OnEndDragWhole(PointerEventData eventData)
	{
		int num = currentPage;
		if (Mathf.Abs(speed) < SpeedCritical)
		{
			int page = GetPage();
			if ((mLocks != null && page >= 0 && page < mLocks.Length && !mLocks[page]) || mLocks == null)
			{
				currentPage = page;
			}
		}
		else
		{
			if (speed > 0f)
			{
				currentPage = GetNextUnlock(currentPage, left: true);
			}
			else
			{
				currentPage = GetNextUnlock(currentPage, left: false);
			}
			currentPage = Mathf.Clamp(currentPage, 0, Whole_Count - 1);
		}
		bUpdateEnd = false;
		bDragging = false;
		speed = 0f;
		UpdateScrollEndPos();
		if (EndDragItem != null)
		{
			EndDragItem(currentPage);
		}
	}

	private void UpdateScrollEndPos()
	{
		scrollendpos = Whole_PerOne * (float)currentPage / AllWidth;
	}

	private void OnUpdateWhole()
	{
		if (bDragging || bSendFinish)
		{
			return;
		}
		if (base.horizontal)
		{
			base.horizontalNormalizedPosition = Mathf.Lerp(base.horizontalNormalizedPosition, scrollendpos, 7f * Updater.delta);
			float num = Mathf.Abs(base.horizontalNormalizedPosition - scrollendpos);
			Vector2 sizeDelta = base.content.sizeDelta;
			if (num * sizeDelta.x < 2f)
			{
				base.horizontalNormalizedPosition = scrollendpos;
				bUpdateEnd = true;
			}
		}
		if (base.vertical)
		{
			base.verticalNormalizedPosition = Mathf.Lerp(base.verticalNormalizedPosition, scrollendpos, 7f * Updater.delta);
			float num2 = Mathf.Abs(base.verticalNormalizedPosition - scrollendpos);
			Vector2 sizeDelta2 = base.content.sizeDelta;
			if (num2 * sizeDelta2.y < 2f)
			{
				base.verticalNormalizedPosition = scrollendpos;
				bUpdateEnd = true;
			}
		}
		if (bUpdateEnd)
		{
			if (mPageAniFinish != null)
			{
				mPageAniFinish();
			}
			bSendFinish = true;
		}
	}

	private int GetPage()
	{
		int value = (int)(scrollpercent * (float)Whole_Count);
		return Mathf.Clamp(value, 0, Whole_Count - 1);
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (OnClick != null)
		{
			OnClick();
		}
	}
}
