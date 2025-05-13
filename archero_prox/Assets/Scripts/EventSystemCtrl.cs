




using UnityEngine;
using UnityEngine.EventSystems;

public class EventSystemCtrl : MonoBehaviour
{
	public static EventSystemCtrl Instance;

	private EventSystem mEventSystem;

	private int defaultDragThreshold;

	private bool bEnable = true;

	private void Start()
	{
		mEventSystem = EventSystem.current;
		mEventSystem.pixelDragThreshold = (int)((float)mEventSystem.pixelDragThreshold * GameLogic.WidthScale);
		Instance = this;
		defaultDragThreshold = mEventSystem.pixelDragThreshold;
	}

	public void SetDragEnable(bool value)
	{
		if (bEnable != value)
		{
			bEnable = value;
			if (value)
			{
				mEventSystem.pixelDragThreshold = defaultDragThreshold;
			}
			else
			{
				mEventSystem.pixelDragThreshold = int.MaxValue;
			}
		}
	}
}
