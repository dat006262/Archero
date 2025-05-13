




using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Patterns;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopOneGold : ShopOneBase
{
	public const float itemwidth = 235f;

	public Text Text_Title;

	public GameObject goldparent;

	private List<ShopItemGold> mList = new List<ShopItemGold>();

	private GameObject _itemgold;

	private LocalUnityObjctPool mPool;

	private GameObject itemgold
	{
		get
		{
			if (_itemgold == null)
			{
				_itemgold = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/ShopUI/ShopItemGoldOne"));
				_itemgold.SetParentNormal(goldparent);
			}
			return _itemgold;
		}
	}

	protected override void OnAwake()
	{
		if (mPool == null)
		{
			mPool = LocalUnityObjctPool.Create(base.gameObject);
			mPool.CreateCache<ShopItemGold>(itemgold);
			itemgold.SetActive(value: false);
		}
	}

	protected override void OnInit()
	{
		mPool.Collect<ShopItemGold>();
		mList.Clear();
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("商店_金币标题"));
		int num = 3;
		float num2 = (float)(num - 1) * 235f;
		for (int i = 0; i < num; i++)
		{
			ShopItemGold shopItemGold = mPool.DeQueue<ShopItemGold>();
			shopItemGold.gameObject.SetParentNormal(goldparent);
			RectTransform rectTransform = shopItemGold.transform as RectTransform;
			rectTransform.anchoredPosition = new Vector2((0f - num2) / 2f + 235f * (float)i, 0f);
			shopItemGold.Init(i);
			shopItemGold.OnClickButton = OnOpenWindowSure;
			mList.Add(shopItemGold);
		}
	}

	private void OnOpenWindowSure(int index, ShopItemGold item)
	{
		BuyGoldSureProxy.Transfer transfer = new BuyGoldSureProxy.Transfer();
		transfer.index = index;
		transfer.item = item;
		transfer.callback = OnClickGold;
		BuyGoldSureProxy proxy = new BuyGoldSureProxy(transfer);
		Facade.Instance.RegisterProxy(proxy);
		WindowUI.ShowWindow(WindowID.WindowID_BuyGoldSure);
	}

	private void OnClickGold(int index, ShopItemGold item)
	{
		int diamond = item.GetDiamond();
		if (LocalSave.Instance.GetDiamond() < diamond)
		{
			CInstance<TipsUIManager>.Instance.Show(ETips.Tips_DiamondNotEnoughBuy);
			Facade.Instance.SendNotification("MainUI_GotoShop", "ShopOneDiamond");
			return;
		}
		int gold = item.GetGold();
		CDiamondToCoin cDiamondToCoin = new CDiamondToCoin();
		cDiamondToCoin.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
		cDiamondToCoin.m_nCoins = (uint)gold;
		cDiamondToCoin.m_nDiamonds = (uint)diamond;
		NetManager.SendInternal(cDiamondToCoin, SendType.eForceOnce, delegate(NetResponse response)
		{
#if ENABLE_NET_MANAGER
            if (response.IsSuccess && response.data != null && response.data is CRespDimaonToCoin)
#endif
			{
#if ENABLE_NET_MANAGER
				CRespDimaonToCoin cRespDimaonToCoin = response.data as CRespDimaonToCoin;
#else
                CRespDimaonToCoin cRespDimaonToCoin = new CRespDimaonToCoin() 
                {
                    m_nCoins = (uint)(LocalSave.Instance.GetGold() + item.GetGold()),
                    m_nDiamonds = (uint)(LocalSave.Instance.GetDiamond() - item.GetDiamond())
                };
#endif
                if (cRespDimaonToCoin != null)
				{
					LocalSave.Instance.UserInfo_SetDiamond((int)cRespDimaonToCoin.m_nDiamonds);
					long gold2 = LocalSave.Instance.GetGold();
					long num = cRespDimaonToCoin.m_nCoins;
					if (gold2 < num)
					{
						long gold3 = num - gold2;
						LocalSave.Instance.UserInfo_SetGold((int)gold2);
						LocalSave.Instance.Modify_Gold(gold3, updateui: false);
						if (item != null)
						{
							CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, gold, item.transform.position);
						}
						else
						{
							CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, gold);
						}
					}
					else
					{
						LocalSave.Instance.UserInfo_SetGold((int)cRespDimaonToCoin.m_nCoins);
					}
					SdkManager.send_event_shop(Utils.FormatString("Gold{0}", index + 1), gold, diamond, 0, 0);
				}
			}
#if ENABLE_NET_MANAGER
			else if (response.error != null)
			{
				CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
			}
#endif
		});
	}

	public override void OnLanguageChange()
	{
		OnInit();
	}

	public override void UpdateNet()
	{
		int i = 0;
		for (int count = mList.Count; i < count; i++)
		{
			mList[i].UpdateNet();
		}
	}

	protected override void OnDeinit()
	{
	}
}
