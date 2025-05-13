




using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine;
using UnityEngine.UI;

public class PurChaseOKUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public Text Text_ID;

	public Text Text_Receipt;

	public ScrollRectBase mScrollRect;

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	private LocalUnityObjctPool mPool;

	private PurChaseOKProxy.Transfer mTransfer;

	protected override void OnInit()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<Text>(Text_Receipt.gameObject);
		Text_Receipt.gameObject.SetActive(value: false);
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_PurchaseOK);
		};
		Button_Shadow.onClick = Button_Close.onClick;
	}

	protected override void OnOpen()
	{
		mPool.Collect<Text>();
		mTransfer = (Facade.Instance.RetrieveProxy("PurChaseOKProxy").Data as PurChaseOKProxy.Transfer);
		InitUI();
	}

	private void InitUI()
	{
		Text_ID.text = mTransfer.id;
		int num = 1000;
		string receipt = mTransfer.receipt;
		int num2 = mTransfer.receipt.Length / num + 1;
		float num3 = 0f;
		for (int i = 0; i < num2; i++)
		{
			int num4 = i * num;
			int value = (i + 1) * num;
			value = MathDxx.Clamp(value, 0, mTransfer.receipt.Length);
			if (value < num4)
			{
				num4 = value;
			}
			string text = "empty";
			if (num4 < value)
			{
				text = receipt.Substring(num4, value - num4);
			}
			Text text2 = mPool.DeQueue<Text>();
			text2.transform.SetParentNormal(mScrollRect.content);
			text2.text = text;
			(text2.transform as RectTransform).anchoredPosition = new Vector2(0f, 0f - num3);
			num3 += text2.preferredHeight;
			if (num4 == value)
			{
				break;
			}
		}
		RectTransform content = mScrollRect.content;
		Vector2 sizeDelta = mScrollRect.content.sizeDelta;
		content.sizeDelta = new Vector2(sizeDelta.x, num3);
		Text_Title.text = "读取消息流失败";
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
