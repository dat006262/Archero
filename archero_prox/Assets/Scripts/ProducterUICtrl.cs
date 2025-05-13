




using PureMVC.Interfaces;
using System.Collections.Generic;

public class ProducterUICtrl : MediatorCtrlBase
{
	public ButtonCtrl Button_Close;

	public List<ProducterOneCtrl> mList;

	private static List<string> mProducters = new List<string>
	{
		"Sinan Zhu",
		"Han Zhang",
		"Jian Chen"
	};

	protected override void OnInit()
	{
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_Producer);
		};
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
	{
		int i = 0;
		for (int count = mList.Count; i < count; i++)
		{
			mList[i].Init(mProducters[i]);
		}
	}

	private void UpdateList()
	{
	}

	private void UpdateChildCallBack(int index, ProducterOneCtrl one)
	{
	}

	protected override void OnClose()
	{
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
	}

	public override void OnLanguageChange()
	{
	}
}
