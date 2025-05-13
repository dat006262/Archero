




using DG.Tweening;
using Dxx.Util;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollIntCtrl<T> : ScrollRectBase where T : Component
{
	public class ScrollData
	{
		public float maxScale = 1.5f;

		public float minScale = 1f;

		public T one;

		public RectTransform transform;

		public int index;

		public float normalize;

		public float normalize_range;

		private float scale;

		private float scalex;

		public ScrollData(int index, T one)
		{
			Refresh(index, one);
		}

		public void Refresh(int index, T one)
		{
			this.index = index;
			this.one = one;
			if ((bool)(UnityEngine.Object)one)
			{
				transform = (one.transform as RectTransform);
			}
		}

		public void Miss()
		{
			one = (T)null;
			transform = null;
		}

		public float UpdateScale(float normalizepos)
		{
			if (!(UnityEngine.Object)one || !transform)
			{
				return 0f;
			}
			if (normalize_range > 0f)
			{
				scale = Mathf.Abs(normalize - normalizepos) / normalize_range;
				scale = Mathf.Clamp01(scale);
			}
			else
			{
				scale = 0f;
			}
			scalex = maxScale - scale * (maxScale - minScale);
			Vector3 localScale = transform.localScale;
			if (localScale.x == scalex)
			{
				return scalex;
			}
			transform.localScale = Vector3.one * scalex;
			return scalex;
		}

		public void SetFront()
		{
			transform.SetAsLastSibling();
		}
	}

	public GameObject copyItem;

	public Transform mScrollChild;

	[Header("滚动加速系数")]
	public float Speed = 3f;

	public Action<int, T> OnUpdateOne;

	public Action<int, T> OnUpdateSize;

	public Action<int, T> OnScrollEnd;

	public Action OnBeginDragEvent;

	public float maxScale = 1.5f;

	public float minScale = 1f;

	private bool bInit;

	private int showCount = 10;

	private int count = 40;

	private float allWidth;

	private float itemWidth;

	private float offsetx = 360f;

	private float lastscrollpos;

	private float lastspeed;

	private int mCurrentIndex;

	private LocalUnityObjctPool mObjPool;

	private List<ScrollData> mList = new List<ScrollData>();

	private Sequence seq;

	private int mGotoIntIndex;

	protected override void Awake()
	{
		base.Awake();
		Vector2 sizeDelta = (base.transform as RectTransform).sizeDelta;
		offsetx = sizeDelta.x / 2f;
		BeginDrag = OnDragBegin;
		Drag = OnDrags;
		EndDrag = OnDragEnd;
	}

	public void InitOnce()
	{
		mObjPool = LocalUnityObjctPool.Create(base.gameObject);
		mObjPool.CreateCache<T>(copyItem);
		Vector2 sizeDelta = (copyItem.transform as RectTransform).sizeDelta;
		itemWidth = sizeDelta.x;
	}

	public void SetScale(float min, float max)
	{
		minScale = min;
		maxScale = max;
	}

	public void Init(int count)
	{
		if (!bInit)
		{
			InitOnce();
			bInit = true;
		}
		mObjPool.Collect<T>();
		base.horizontalNormalizedPosition = 0f;
		base.velocity = Vector2.zero;
		lastscrollpos = -1f;
		mCurrentIndex = 0;
		lastspeed = 0f;
		mList.Clear();
		this.count = count;
		allWidth = (float)(count - 1) * itemWidth;
		RectTransform content = base.content;
		float x = allWidth + offsetx * 2f;
		Vector2 sizeDelta = base.content.sizeDelta;
		content.sizeDelta = new Vector2(x, sizeDelta.y);
		for (int i = 0; i < count; i++)
		{
			ScrollData scrollData = new ScrollData(i, (T)null);
			scrollData.maxScale = maxScale;
			scrollData.minScale = minScale;
			mList.Add(scrollData);
			if (i < showCount)
			{
				T val = mObjPool.DeQueue<T>();
				if (OnUpdateOne != null)
				{
					OnUpdateOne(i, val);
				}
				UpdateOne(i, val);
			}
			else
			{
				UpdateOne(i, (T)null);
			}
		}
		UpdateSize();
		if (mCurrentIndex < mList.Count && OnScrollEnd != null)
		{
			OnScrollEnd(mCurrentIndex, mList[mCurrentIndex].one);
		}
	}

	private void UpdateOne(int i, T one)
	{
		ScrollData scrollData = mList[i];
		scrollData.Refresh(i, one);
		if ((UnityEngine.Object)one != (UnityEngine.Object)null)
		{
			one.transform.SetParentNormal(mScrollChild);
			RectTransform rectTransform = one.transform as RectTransform;
			rectTransform.anchoredPosition = new Vector2((float)i * itemWidth + offsetx, 0f);
		}
		if (count > 1)
		{
			scrollData.normalize = (float)i / ((float)count - 1f);
			scrollData.normalize_range = itemWidth / allWidth;
		}
		else if (count == 1)
		{
			scrollData.normalize = 0f;
			scrollData.normalize_range = 0f;
		}
	}

	public void DeInit()
	{
		if (mObjPool != null)
		{
			mObjPool.Collect<T>();
		}
		Updater.RemoveUpdateUI(OnUpdate);
		if (seq != null)
		{
			seq.Kill();
		}
	}

	private void OnDragBegin(PointerEventData eventData)
	{
		if (OnBeginDragEvent != null)
		{
			OnBeginDragEvent();
		}
		Updater.RemoveUpdateUI(OnUpdate);
	}

	private void OnDrags(PointerEventData eventData)
	{
		UpdateSize();
	}

	private void OnDragEnd(PointerEventData eventData)
	{
		base.velocity *= Speed;
		Updater.RemoveUpdateUI(OnUpdate);
		Updater.AddUpdateUI(OnUpdate);
	}

	private void OnUpdate(float delta)
	{
		UpdateScroll();
		Vector2 velocity = base.velocity;
		lastspeed = velocity.x;
	}

	private void UpdateScroll()
	{
		UpdateSize();
		UpdateInfinity();
	}

	private void UpdateSize()
	{
		if (lastscrollpos == base.horizontalNormalizedPosition)
		{
			return;
		}
		lastscrollpos = base.horizontalNormalizedPosition;
		lastscrollpos = Mathf.Clamp01(lastscrollpos);
		float num = 0f;
		int i = 0;
		for (int num2 = mList.Count; i < num2; i++)
		{
			ScrollData scrollData = mList[i];
			float num3 = Mathf.Abs((lastscrollpos - scrollData.normalize) * allWidth);
			if (num3 > 600f)
			{
				if ((UnityEngine.Object)scrollData.one != (UnityEngine.Object)null)
				{
					mObjPool.EnQueue<T>(scrollData.one.gameObject);
					scrollData.Miss();
				}
			}
			else if ((UnityEngine.Object)scrollData.one == (UnityEngine.Object)null)
			{
				UpdateOne(i, mObjPool.DeQueue<T>());
				if (OnUpdateOne != null)
				{
					OnUpdateOne(i, scrollData.one);
				}
			}
			float num4 = scrollData.UpdateScale(lastscrollpos);
			if (num < num4)
			{
				num = num4;
				mCurrentIndex = i;
			}
		}
		if (mCurrentIndex < mList.Count)
		{
			mList[mCurrentIndex].SetFront();
			if (OnUpdateSize != null)
			{
				OnUpdateSize(mCurrentIndex, mList[mCurrentIndex].one);
			}
		}
	}

	private void UpdateInfinity()
	{
		Vector2 velocity = base.velocity;
		if (Mathf.Abs(velocity.x - lastspeed) < 50f && mCurrentIndex < mList.Count)
		{
			float horizontalNormalizedPosition = base.horizontalNormalizedPosition;
			base.horizontalNormalizedPosition = mList[mCurrentIndex].normalize;
			float num = (base.horizontalNormalizedPosition - horizontalNormalizedPosition) * allWidth;
			mScrollChild.transform.localPosition = new Vector3(num, 0f, 0f);
			Updater.RemoveUpdateUI(OnUpdate);
			Tweener t = mScrollChild.DOLocalMoveX((!(num > 0f)) ? 10 : (-10), 0.2f).OnUpdate(UpdateSize);
			Tweener t2 = mScrollChild.DOLocalMoveX(0f, 0.2f).OnUpdate(UpdateSize);
			seq = DOTween.Sequence().Append(t).Append(t2);
			if (OnScrollEnd != null)
			{
				OnScrollEnd(mCurrentIndex, mList[mCurrentIndex].one);
			}
		}
	}

	public void GotoInt(int index, bool playanimation = false)
	{
		if (index >= mList.Count || index < 0)
		{
			return;
		}
		if (!playanimation)
		{
			float x = (0f - mList[index].normalize) * allWidth;
			base.content.localPosition = new Vector3(x, 0f, 0f);
			mCurrentIndex = index;
			UpdateSize();
			if (OnScrollEnd != null)
			{
				OnScrollEnd(index, mList[index].one);
			}
		}
		else
		{
			mGotoIntIndex = index;
			float posx = (0f - mList[mCurrentIndex].normalize) * allWidth;
			base.content.localPosition = new Vector3(posx, 0f, 0f);
			float normalize = mList[mGotoIntIndex].normalize;
			float nextxx = (0f - normalize) * allWidth;
			Vector3 localPosition = base.content.localPosition;
			float x2 = localPosition.x;
			float starth = mList[mCurrentIndex].normalize;
			float endh = mList[mGotoIntIndex].normalize;
			base.content.DOLocalMoveX(nextxx, 0.5f).OnUpdate(delegate
			{
				lastscrollpos = -1f;
				Vector3 localPosition2 = base.content.localPosition;
				float num = MathDxx.Abs((localPosition2.x - posx) / (nextxx - posx));
				base.horizontalNormalizedPosition = (endh - starth) * num + starth;
				UpdateSize();
			}).SetEase(Ease.OutQuad)
				.OnComplete(delegate
				{
					mCurrentIndex = mGotoIntIndex;
					if (OnScrollEnd != null)
					{
						OnScrollEnd(mCurrentIndex, mList[mCurrentIndex].one);
					}
				});
		}
	}
}
