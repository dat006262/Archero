




using PureMVC.Interfaces;
using UnityEngine;

public class MainUIPage4 : UIBase
{
	private GameObject child;

	private MediatorCtrlBase ctrl;

	private ButtonCtrl mButtonStart;

	public MainUIPage4(Transform parent)
		: base(parent)
	{
	}

	protected override void OnInitBefore()
	{
		child = new GameObject("4_setting");
		child.AddComponent<RectTransform>();
		child.transform.SetParentNormal(mParent);
	}

	protected override void OnInit()
	{
		mView = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/MainUI/4"));
		mView.SetParentNormal((!(child == null)) ? child.transform : mParent);
		ctrl = mView.GetComponentInChildren<MediatorCtrlBase>();
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
