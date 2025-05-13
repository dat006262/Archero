




using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{
	public delegate void VoidDelegate(GameObject go);

	public VoidDelegate onClick;

	public VoidDelegate onDown;

	public VoidDelegate onEnter;

	public VoidDelegate onExit;

	public VoidDelegate onUp;

	public VoidDelegate onSelect;

	public VoidDelegate onUpdateSelect;

	public static EventTriggerListener Get(GameObject go)
	{
		EventTriggerListener eventTriggerListener = go.GetComponent<EventTriggerListener>();
		if (eventTriggerListener == null)
		{
			eventTriggerListener = go.AddComponent<EventTriggerListener>();
		}
		return eventTriggerListener;
	}

	public override void OnPointerClick(PointerEventData eventData)
	{
		if (onClick != null)
		{
			onClick(base.gameObject);
		}
	}

	public override void OnPointerDown(PointerEventData eventData)
	{
		if (onDown != null)
		{
			onDown(base.gameObject);
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		if (onEnter != null)
		{
			onEnter(base.gameObject);
		}
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		if (onExit != null)
		{
			onExit(base.gameObject);
		}
	}

	public override void OnPointerUp(PointerEventData eventData)
	{
		if (onUp != null)
		{
			onUp(base.gameObject);
		}
	}

	public override void OnSelect(BaseEventData eventData)
	{
		if (onSelect != null)
		{
			onSelect(base.gameObject);
		}
	}

	public override void OnUpdateSelected(BaseEventData eventData)
	{
		if (onUpdateSelect != null)
		{
			onUpdateSelect(base.gameObject);
		}
	}
}
