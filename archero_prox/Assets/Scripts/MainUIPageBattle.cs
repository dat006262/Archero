




using PureMVC.Interfaces;
using UnityEngine;

public class MainUIPageBattle : UIBase
{
	private GameObject child;

	private MediatorCtrlBase mCtrl;

	public MainUIPageBattle(Transform parent)
		: base(parent)
	{
	}

	protected override void OnInitBefore()
	{
		child = new GameObject("2_battle");
		child.AddComponent<RectTransform>();
		child.transform.SetParentNormal(mParent);
	}

	protected override void OnInit()
	{
		mView = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/MainUI/2Battle"));
		mView.SetParentNormal((!(child == null)) ? child.transform : mParent);
		mCtrl = mView.transform.Find("offset").GetComponent<MediatorCtrlBase>();
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

	public override object OnGetEvent(string eventName)
	{
		if (mCtrl != null)
		{
			return mCtrl.OnGetEvent(eventName);
		}
		return null;
	}
}
