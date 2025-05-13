




using PureMVC.Interfaces;
using UnityEngine;

public class MainUIPageChar : UIBase
{
	private GameObject child;

	private MediatorCtrlBase ctrl;

	private ScrollRectBase mMainScroll;

	public MainUIPageChar(Transform parent, ScrollRectBase scroll)
		: base(parent)
	{
		mMainScroll = scroll;
	}

	protected override void OnInitBefore()
	{
		child = new GameObject("1_char");
		child.AddComponent<RectTransform>();
		child.transform.SetParentNormal(mParent);
	}

	protected override void OnInit()
	{
		mView = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/MainUI/1Char"));
		mView.SetParentNormal((!(child == null)) ? child.transform : mParent);
		ctrl = mView.GetComponentInChildren<MediatorCtrlBase>();
		ctrl.SetArgs(mMainScroll);
		ctrl.Init();
	}

	protected override void OnDeInit()
	{
	}

	protected override void OnOpen()
	{
		if (ctrl != null)
		{
			ctrl.Open();
		}
	}

	protected override void OnClose()
	{
		if (ctrl != null)
		{
			ctrl.Close();
		}
	}

	protected override void OnHandleNotification(INotification notification)
	{
		if (ctrl != null)
		{
			ctrl.OnHandleNotification(notification);
		}
	}

	public override object OnGetEvent(string eventName)
	{
		if (ctrl != null)
		{
			return ctrl.OnGetEvent(eventName);
		}
		return null;
	}

	public override void OnLanguageChange()
	{
		if (ctrl != null)
		{
			ctrl.OnLanguageChange();
		}
	}
}
