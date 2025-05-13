




using Dxx.Util;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSingleUICtrl : MediatorCtrlBase
{
	public ButtonCtrl Button_Close;

	public ScrollRectBase mScrollRect;

	public RectTransform window;

	private ShopSingleProxy.Transfer mTransfer;

	private List<string> shops = new List<string>();

	private List<ShopOneBase> mShopItemList = new List<ShopOneBase>();

	private Dictionary<string, Func<bool>> mOpenCondition = new Dictionary<string, Func<bool>>();

	private Action onUIClose;

	protected override void OnInit()
	{
		mOpenCondition.Clear();
		mOpenCondition.Add("ShopOneStageDiscount", () => (!PurchaseManager.Instance.IsValid()) ? false : ShopOneStageDiscount.IsValid());
		RectTransform rectTransform = base.transform as RectTransform;
		RectTransform rectTransform2 = window;
		Vector2 sizeDelta = window.sizeDelta;
		float x = sizeDelta.x;
		Vector2 sizeDelta2 = rectTransform.sizeDelta;
		float y = sizeDelta2.y;
		Vector2 anchoredPosition = window.anchoredPosition;
		rectTransform2.sizeDelta = new Vector2(x, y + anchoredPosition.y);
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_ShopSingle);
		};
	}

	protected override void OnOpen()
	{
		SdkManager.send_event_iap("SHOW", PurchaseManager.Instance.GetOpenSource(), string.Empty, string.Empty, string.Empty);
		WindowUI.ShowCurrency(WindowID.WindowID_Currency);
		IProxy proxy = Facade.Instance.RetrieveProxy("ShopSingleProxy");
		if (proxy == null || proxy.Data == null)
		{
			SdkManager.Bugly_Report("ShopSingleUICtrl.cs", Utils.FormatString("OnOpen ShopSingleProxy is error."));
			Button_Close.onClick();
		}
		else
		{
			mTransfer = (proxy.Data as ShopSingleProxy.Transfer);
			onUIClose = proxy.Event_Para0;
			InitUI();
		}
	}

	private void InitUI()
	{
		shops.Clear();
		int i = 0;
		for (int count = mShopItemList.Count; i < count; i++)
		{
			if (mShopItemList[i] != null)
			{
				UnityEngine.Object.Destroy(mShopItemList[i].gameObject);
			}
		}
		mShopItemList.Clear();
		if (mTransfer.type == ShopSingleProxy.SingleType.eDiamond)
		{
			shops.Add("ShopOneStageDiscount");
			shops.Add("ShopOneDiamond");
		}
		float num = 100f;
		int j = 0;
		for (int count2 = shops.Count; j < count2; j++)
		{
			Func<bool> value = null;
			if (!mOpenCondition.TryGetValue(shops[j], out value) || value == null || value())
			{
				ShopOneBase shop = GetShop(shops[j]);
				shop.Init();
				shop.mRectTransform.anchoredPosition = new Vector2(0f, 0f - num);
				float num2 = num;
				Vector2 sizeDelta = shop.mRectTransform.sizeDelta;
				num = num2 + sizeDelta.y;
			}
		}
		float num3 = num;
		Vector2 sizeDelta2 = window.sizeDelta;
		if (num3 > sizeDelta2.y)
		{
			num += 200f;
			mScrollRect.movementType = ScrollRect.MovementType.Elastic;
		}
		else
		{
			mScrollRect.movementType = ScrollRect.MovementType.Clamped;
		}
		RectTransform content = mScrollRect.content;
		Vector2 sizeDelta3 = mScrollRect.content.sizeDelta;
		content.sizeDelta = new Vector2(sizeDelta3.x, num);
	}

	private void UpdateList()
	{
	}

	private ShopOneBase GetShop(string path)
	{
		string path2 = Utils.FormatString("UIPanel/ShopUI/{0}", path);
		GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>(path2));
		gameObject.SetParentNormal(mScrollRect.content.transform);
		ShopOneBase component = gameObject.GetComponent<ShopOneBase>();
		mShopItemList.Add(component);
		return component;
	}

	protected override void OnClose()
	{
		WindowUI.CloseCurrency();
		if (onUIClose != null)
		{
			onUIClose();
		}
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnHandleNotification(INotification notification)
	{
		string name = notification.Name;
		object body = notification.Body;
		if (name != null && name == "ShopUI_Update")
		{
			OnOpen();
		}
	}

	public override void OnLanguageChange()
	{
	}
}
