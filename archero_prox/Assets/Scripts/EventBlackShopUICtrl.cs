




using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class EventBlackShopUICtrl : MediatorCtrlBase
{
	private const float width = 200f;

	private const float height = 240f;

	private const int LineCount = 4;

	public Text Text_Title;

	public Text Text_Content;

	public Text Text_Close;

	public ButtonCtrl Button_Close;

	public GameObject items;

	private GameObject _itemone;

	private List<BlackItemOnectrl> mList = new List<BlackItemOnectrl>();

	private LocalUnityObjctPool mPool;

	private List<Shop_MysticShop> mDataList = new List<Shop_MysticShop>();

	private List<int> buys = new List<int>();

	private int diamondforcoin;

	private List<int> shows = new List<int>();

	private int shoptype;

	private GameObject itemone
	{
		get
		{
			if (_itemone == null)
			{
				_itemone = CInstance<UIResourceCreator>.Instance.GetBlackShopOne(base.transform).gameObject;
				_itemone.SetActive(value: false);
			}
			return _itemone;
		}
	}

	protected override void OnInit()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<BlackItemOnectrl>(itemone);
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_EventBlackShop);
		};
	}

	protected override void OnOpen()
	{
		GameLogic.SetPause(pause: true);
		WindowUI.ShowCurrency(WindowID.WindowID_Currency);
		mPool.Collect<BlackItemOnectrl>();
		InitUI();
	}

	private void InitUI()
	{
		buys.Clear();
		shoptype = LocalModelManager.Instance.Shop_MysticShop.GetRandomShopType();
		mDataList = LocalModelManager.Instance.Shop_MysticShop.GetListByStage(GameLogic.Hold.BattleData.Level_CurrentStage, shoptype);
		mList.Clear();
		int count = mDataList.Count;
		int num = MathDxx.Clamp(count, 0, 4);
		float num2 = (float)(-(num - 1)) * 200f / 2f;
		for (int i = 0; i < count; i++)
		{
			BlackItemOnectrl blackItemOnectrl = mPool.DeQueue<BlackItemOnectrl>();
			blackItemOnectrl.Init(i, mDataList[i]);
			blackItemOnectrl.OnClickButton = OnClickBuy;
			RectTransform rectTransform = blackItemOnectrl.transform as RectTransform;
			rectTransform.SetParentNormal(items);
			rectTransform.anchoredPosition = new Vector2(num2 + (float)(i % 4) * 200f, 120f + (float)(i / 4) * -240f);
			mList.Add(blackItemOnectrl);
		}
		shows.Clear();
		int j = 0;
		for (int count2 = mDataList.Count; j < count2; j++)
		{
			shows.Add(mDataList[j].ProductId);
		}
		for (int k = shows.Count; k < 8; k++)
		{
			shows.Add(0);
		}
		if (shows.Count >= 8)
		{
			SdkManager.send_event_mysteries("SHOW", shoptype, 0, 0, shows[0], shows[1], shows[2], shows[3], shows[4], shows[5], shows[6], shows[7], 0, 0, string.Empty, string.Empty);
		}
	}

	private void OnClickBuy(BlackItemOnectrl one)
	{
		int coins = (one.mData.PriceType == 1) ? one.mData.Price : 0;
		int gems = (one.mData.PriceType == 2) ? one.mData.Price : 0;
		SdkManager.send_event_mysteries("CLICK", shoptype, one.mIndex, one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, coins, gems, string.Empty, string.Empty);
		LocalSave.EquipOne equipOne = new LocalSave.EquipOne();
		equipOne.EquipID = one.mData.ProductId;
		equipOne.bNew = false;
		equipOne.Count = one.mData.ProductNum;
		equipOne.Level = 1;
		equipOne.WearIndex = -1;
		LocalSave.EquipOne one2 = equipOne;
		EquipInfoModuleProxy.Transfer transfer = new EquipInfoModuleProxy.Transfer();
		transfer.one = one2;
		transfer.type = EquipInfoModuleProxy.InfoType.eBuy;
		transfer.buy_itemone = one;
		transfer.buy_callback = OnClickBuyInternal;
		EquipInfoModuleProxy proxy = new EquipInfoModuleProxy(transfer);
		Facade.Instance.RegisterProxy(proxy);
		WindowUI.ShowWindow(WindowID.WindowID_EquipInfo);
	}

	private void OnClickBuyInternal(BlackItemOnectrl one)
	{
		switch (one.mData.PriceType)
		{
		case 1:
		{
			long gold = LocalSave.Instance.GetGold();
			int needgold = one.mData.Price;
			if (gold < needgold)
			{
				PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EBLACK_SHOP);
				WindowUI.ShowGoldBuy(CoinExchangeSource.EBLACK_SHOP, needgold - gold, delegate(int diamond)
				{
					diamondforcoin = diamond;
					OnClickBuyInternal(one);
				});
			}
			else
			{
				SdkManager.send_event_mysteries("CLICK_PURCHASE", shoptype, one.mIndex, one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, needgold, 0, string.Empty, string.Empty);
				CEquipTrans equipTrans = GetEquipTrans(one.mData, needgold, 0);
				NetManager.SendInternal(equipTrans, SendType.eForceOnce, delegate(NetResponse response)
				{
#if ENABLE_NET_MANAGER
					if (response.IsSuccess)
#endif
					{
						add_equipexp(equipTrans.m_stEquipItem);
						LocalSave.Instance.Modify_Gold(-one.mData.Price);
						one.Buy();
						CInstance<TipsUIManager>.Instance.Show(ETips.Tips_BlackShopBuy);
						diamondforcoin = 0;
						buys.Add(one.mData.ProductId);
						SdkManager.send_event_equipment("GET", one.mData.ProductId, one.mData.ProductNum, 1, EquipSource.EBlack_shop, 0);
						SdkManager.send_event_mysteries("FINISH", shoptype, one.mIndex, one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, needgold, 0, "SUCCESS", string.Empty);
					}
#if ENABLE_NET_MANAGER
					else if (response.error != null)
					{
						CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 1);
						SdkManager.send_event_mysteries("FINISH", shoptype, one.mIndex, one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, needgold, 0, "FAIL", "CURRENCY_NOT_ENOUGH");
					}
					else
					{
						SdkManager.send_event_mysteries("FINISH", shoptype, one.mIndex, one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, needgold, 0, "FAIL", "RESPONSE_ERROR_NULL");
					}
#endif
				});
			}
			break;
		}
		case 2:
		{
			long diamond2 = LocalSave.Instance.GetDiamond();
			int needdiamond = one.mData.Price;
			if (diamond2 < needdiamond)
			{
				PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EBLACK_SHOP);
				WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond);
			}
			else
			{
				SdkManager.send_event_mysteries("CLICK_PURCHASE", shoptype, one.mIndex, one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, 0, needdiamond, string.Empty, string.Empty);
				CEquipTrans equipTrans2 = GetEquipTrans(one.mData, 0, needdiamond);
				NetManager.SendInternal(equipTrans2, SendType.eForceOnce, delegate(NetResponse response)
				{
#if ENABLE_NET_MANAGER
					if (response.IsSuccess)
#endif
					{
						add_equipexp(equipTrans2.m_stEquipItem);
						LocalSave.Instance.Modify_Diamond(-one.mData.Price);
						one.Buy();
						CInstance<TipsUIManager>.Instance.Show(ETips.Tips_BlackShopBuy);
						diamondforcoin = 0;
						buys.Add(one.mData.ProductId);
						SdkManager.send_event_equipment("GET", one.mData.ProductId, one.mData.ProductNum, 1, EquipSource.EBlack_shop, 0);
						SdkManager.send_event_mysteries("FINISH", shoptype, one.mIndex, one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, 0, needdiamond, "SUCCESS", string.Empty);
					}
#if ENABLE_NET_MANAGER
					else if (response.error != null)
					{
						CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
						SdkManager.send_event_mysteries("FINISH", shoptype, one.mIndex, one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, 0, needdiamond, "FAIL", "DIAMOND_NOT_ENOUGH");
					}
					else
					{
						CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
						SdkManager.send_event_mysteries("FINISH", shoptype, one.mIndex, one.mData.ProductId, 0, 0, 0, 0, 0, 0, 0, 0, 0, needdiamond, "FAIL", "RESPONSE_ERROR_NULL");
					}
#endif
				});
			}
			break;
		}
		}
	}

	private CEquipTrans GetEquipTrans(Shop_MysticShop data, int gold, int diamond)
	{
		CEquipTrans cEquipTrans = new CEquipTrans();
		cEquipTrans.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
		cEquipTrans.m_nType = 1;
		cEquipTrans.m_nCoins = (ushort)gold;
		cEquipTrans.m_nDiamonds = (ushort)diamond;
		cEquipTrans.m_stEquipItem = new CEquipmentItem();
		cEquipTrans.m_stEquipItem.m_nUniqueID = Utils.GenerateUUID();
		cEquipTrans.m_stEquipItem.m_nEquipID = (uint)data.ProductId;
		cEquipTrans.m_stEquipItem.m_nLevel = 1u;
		cEquipTrans.m_stEquipItem.m_nFragment = (uint)data.ProductNum;
		return cEquipTrans;
	}

	private void add_equipexp(CEquipmentItem item)
	{
		if (item != null)
		{
			LocalSave.EquipOne equipOne = new LocalSave.EquipOne();
			equipOne.UniqueID = item.m_nUniqueID;
			equipOne.EquipID = (int)item.m_nEquipID;
			equipOne.Level = 1;
			equipOne.Count = (int)item.m_nFragment;
			if (equipOne.Overlying)
			{
				LocalSave.Instance.AddProp(item);
			}
		}
	}

	protected override void OnClose()
	{
		GameLogic.SetPause(pause: false);
		WindowUI.CloseCurrency();
	}

	private void UpdateCurrency()
	{
		int i = 0;
		for (int count = mList.Count; i < count; i++)
		{
			mList[i].UpdateCurrency();
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
		if (name != null && name == "PUB_UI_UPDATE_CURRENCY")
		{
			UpdateCurrency();
		}
	}

	public override void OnLanguageChange()
	{
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("神秘商店标题");
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("神秘商店描述");
		Text_Close.text = GameLogic.Hold.Language.GetLanguageByTID("神秘商店关闭");
	}
}
