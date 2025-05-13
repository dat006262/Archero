




using PureMVC.Interfaces;
using UnityEngine;

public class MainUIPageShop : UIBase
{
	private GameObject child;

	private MediatorCtrlBase mCtrl;

	private ScrollRectBase mMainScroll;

	public MainUIPageShop(Transform parent, ScrollRectBase scroll)
		: base(parent)
	{
		mMainScroll = scroll;
	}

	protected override void OnInitBefore()
	{
		child = new GameObject("0_shop");
		child.AddComponent<RectTransform>();
		child.transform.SetParentNormal(mParent);
	}

	protected override void OnInit()
	{
		mView = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/MainUI/0Shop"));
		mView.SetParentNormal((!(child == null)) ? child.transform : mParent);
		mCtrl = mView.GetComponentInChildren<MediatorCtrlBase>();
		mCtrl.SetArgs(mMainScroll);
		mCtrl.Init();
	}

	protected override void OnDeInit()
	{
	}

	protected override void OnOpen()
	{
		if (mCtrl != null)
		{
			mCtrl.Open();
		}
	}

	protected override void OnClose()
	{
		if (mCtrl != null)
		{
			mCtrl.Close();
		}
	}

	protected override void OnHandleNotification(INotification notification)
	{
		if (mCtrl != null)
		{
			mCtrl.OnHandleNotification(notification);
		}
	}

	public override void OnLanguageChange()
	{
		if (mCtrl != null)
		{
			mCtrl.OnLanguageChange();
		}
	}
}
