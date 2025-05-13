




using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EventFirstShopUICtrl : MediatorCtrlBase
{
	public const int FirstShopCount = 2;

	public Text Text_Title;

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	public GameObject copyitems;

	public GameObject copyitem;

	public GameObject itemsparent;

	private List<bool> goodbuy;

	private LocalUnityObjctPool mPool;

	protected override void OnInit()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<FirstItemOnectrl>(copyitem);
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_EventFirstShop);
		};
		Button_Shadow.onClick = Button_Close.onClick;
		copyitems.SetActive(value: false);
	}

	protected override void OnOpen()
	{
		GameLogic.SetPause(pause: true);
		WindowUI.ShowCurrency(WindowID.WindowID_Currency);
		mPool.Collect<FirstItemOnectrl>();
		InitUI();
	}

	private void InitUI()
	{
		IList<Shop_ReadyShop> allBeans = LocalModelManager.Instance.Shop_ReadyShop.GetAllBeans();
		int count = allBeans.Count;
		count = MathDxx.Clamp(count, count, 2);
		goodbuy = LocalSave.Instance.BattleIn_GetFirstShop();
		for (int i = 0; i < count; i++)
		{
			FirstItemOnectrl firstItemOnectrl = mPool.DeQueue<FirstItemOnectrl>();
			firstItemOnectrl.Init(i, allBeans[i], GameLogic.Hold.BattleData.GetFirstShopBuy(i));
			firstItemOnectrl.OnClickButton = OnClickBuy;
			firstItemOnectrl.SetBuy(goodbuy[i]);
			RectTransform rectTransform = firstItemOnectrl.transform as RectTransform;
			rectTransform.SetParentNormal(itemsparent);
			rectTransform.anchoredPosition = new Vector2(0f, i * -170);
		}
	}

	private void OnClickBuy(FirstItemOnectrl one)
	{
		switch (one.mData.PriceType)
		{
		case 1:
		{
			long gold = LocalSave.Instance.GetGold();
			int price2 = one.mData.Price;
			if (gold < price2)
			{
				PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EFIRST_SHOP);
				WindowUI.ShowGoldBuy(CoinExchangeSource.EFIRST_SHOP, price2 - gold, delegate
				{
					OnClickBuy(one);
				});
				return;
			}
			CLifeTransPacket cLifeTransPacket2 = new CLifeTransPacket();
			cLifeTransPacket2.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
			cLifeTransPacket2.m_nType = 3;
			cLifeTransPacket2.m_nMaterial = (ushort)price2;
			NetManager.SendInternal(cLifeTransPacket2, SendType.eForceOnce, delegate(NetResponse response)
			{
#if ENABLE_NET_MANAGER
				if (response.IsSuccess)
#endif
				{
					LocalSave.Instance.Modify_Gold(-one.mData.Price);
					one.Buy();
				}
#if ENABLE_NET_MANAGER
				else if (response.error != null)
				{
					CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
				}
#endif
			});
			break;
		}
		case 2:
		{
			long diamond2 = LocalSave.Instance.GetDiamond();
			int price = one.mData.Price;
			if (diamond2 < price)
			{
				PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EFIRST_SHOP);
				WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond);
				return;
			}
			CLifeTransPacket cLifeTransPacket = new CLifeTransPacket();
			cLifeTransPacket.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
			cLifeTransPacket.m_nType = 4;
			cLifeTransPacket.m_nMaterial = (ushort)price;
			NetManager.SendInternal(cLifeTransPacket, SendType.eForceOnce, delegate(NetResponse response)
			{
#if ENABLE_NET_MANAGER
				if (response.IsSuccess)
#endif
				{
					LocalSave.Instance.Modify_Diamond(-one.mData.Price);
					one.Buy();
				}
#if ENABLE_NET_MANAGER
				else if (response.error != null)
				{
					CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
				}
#endif
			});
			break;
		}
		}
		goodbuy[one.mIndex] = true;
		LocalSave.Instance.BattleIn_UpdateFirstShop(goodbuy);
	}

	protected override void OnClose()
	{
		GameLogic.SetPause(pause: false);
		WindowUI.CloseCurrency();
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
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("初始商店标题");
	}
}
