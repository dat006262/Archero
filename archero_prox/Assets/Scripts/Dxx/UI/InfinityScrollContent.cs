




using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Dxx.UI
{
	public class InfinityScrollContent : UIBehaviour
	{
		public InfinityScrollGroup mGroup;

		protected DrivenRectTransformTracker m_Tracker;

		private RectTransform m_rectTransform;

		public RectTransform rectTransform
		{
			get
			{
				if (m_rectTransform == null)
				{
					m_rectTransform = (base.transform as RectTransform);
				}
				return m_rectTransform;
			}
		}

		protected override void Awake()
		{
			if (mGroup == null)
			{
				mGroup = GetComponentInChildren<InfinityScrollGroup>();
			}
			InfinityScrollGroup infinityScrollGroup = mGroup;
			infinityScrollGroup.onSizeChange = (Action<Vector2>)Delegate.Combine(infinityScrollGroup.onSizeChange, new Action<Vector2>(FitContent));
			m_Tracker.Clear();
			m_Tracker.Add(this, rectTransform, DrivenTransformProperties.SizeDelta);
			base.Awake();
		}

		public void FitContent(Vector2 size)
		{
			if (mGroup.sortAxis == InfinityScrollGroup.Axis.Horizontal)
			{
				RectTransform rectTransform = this.rectTransform;
				Vector2 sizeDelta = this.rectTransform.sizeDelta;
				rectTransform.sizeDelta = new Vector2(sizeDelta.x, size.y);
			}
			else
			{
				RectTransform rectTransform2 = this.rectTransform;
				float x = size.x;
				Vector2 sizeDelta2 = this.rectTransform.sizeDelta;
				rectTransform2.sizeDelta = new Vector2(x, sizeDelta2.y);
			}
		}
	}
}
