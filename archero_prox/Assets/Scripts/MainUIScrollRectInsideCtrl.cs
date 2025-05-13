




using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainUIScrollRectInsideCtrl : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler, IEventSystemHandler
{
	public ScrollRectBase anotherScrollRect;

	public bool thisIsUpAndDown = true;

	public Action Event_OnClick;

	private ScrollRectBase thisScrollRect;

	private bool bFirstDrag = true;

	private void Awake()
	{
		thisScrollRect = GetComponent<ScrollRectBase>();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if (!(anotherScrollRect == null))
		{
			anotherScrollRect.enabled = false;
		}
	}

	public void OnDrag(PointerEventData eventData)
	{
		if (anotherScrollRect == null)
		{
			return;
		}
		if (bFirstDrag)
		{
			float num = Vector2.Angle(eventData.delta, Vector2.up);
			if (num > 45f && num < 135f)
			{
				thisScrollRect.enabled = !thisIsUpAndDown;
				anotherScrollRect.enabled = thisIsUpAndDown;
			}
			else
			{
				anotherScrollRect.enabled = !thisIsUpAndDown;
				thisScrollRect.enabled = thisIsUpAndDown;
			}
		}
		if (thisScrollRect.enabled)
		{
			if (bFirstDrag)
			{
				thisScrollRect.OnBeginDragInternal(eventData);
				bFirstDrag = false;
			}
			thisScrollRect.OnDragInternal(eventData);
		}
		else
		{
			if (bFirstDrag)
			{
				anotherScrollRect.OnBeginDragInternal(eventData);
				bFirstDrag = false;
			}
			anotherScrollRect.OnDragInternal(eventData);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		if (!(anotherScrollRect == null))
		{
			if (thisScrollRect.enabled)
			{
				thisScrollRect.OnEndDragInternal(eventData);
			}
			else
			{
				anotherScrollRect.OnEndDragInternal(eventData);
			}
			anotherScrollRect.enabled = true;
			thisScrollRect.enabled = true;
			bFirstDrag = true;
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (Event_OnClick != null)
		{
			Event_OnClick();
		}
	}
}
