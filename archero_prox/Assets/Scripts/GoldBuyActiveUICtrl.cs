




using Dxx.Net;
using GameProtocol;
using PureMVC.Interfaces;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class GoldBuyActiveUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public Text Text_Content;

	public GoldTextCtrl mDiamondCtrl;

	public ButtonCtrl Button_Buy;

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	private long gold;

	private long needdiamond;

	protected override void OnInit()
	{
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_GoldBuyActive);
		};
		Button_Shadow.onClick = Button_Close.onClick;
		Button_Buy.SetDepondNet(value: true);
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
	{
		needdiamond = 50L;
		gold = needdiamond * LocalModelManager.Instance.Shop_Gold.GetDiamond2Gold();
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("金币购买标题");
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("金币购买描述", gold);
		mDiamondCtrl.SetCurrencyType(CurrencyType.Diamond);
		mDiamondCtrl.UseTextRed();
		mDiamondCtrl.SetValue((int)needdiamond);
		Button_Buy.onClick = delegate
		{
			long diamond = LocalSave.Instance.GetDiamond();
			if (diamond < needdiamond)
			{
				PurchaseManager.Instance.SetOpenSource(ShopOpenSource.EACTIVE);
				WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond);
			}
			else
			{
				CDiamondToCoin cDiamondToCoin = new CDiamondToCoin
				{
					m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
					m_nCoins = (uint)gold,
					m_nDiamonds = (uint)needdiamond
				};
				Debug.Log("Send DiamondToCoin Request " + cDiamondToCoin.m_nCoins + " transid " + cDiamondToCoin.m_nTransID);
				NetManager.SendInternal(cDiamondToCoin, SendType.eForceOnce, delegate(NetResponse response)
				{
					if (response.IsSuccess && response.data != null && response.data is CRespDimaonToCoin)
					{
						CRespDimaonToCoin cRespDimaonToCoin = response.data as CRespDimaonToCoin;
						LocalSave.Instance.UserInfo_SetDiamond((int)cRespDimaonToCoin.m_nDiamonds);
						long num = LocalSave.Instance.GetGold();
						long num2 = cRespDimaonToCoin.m_nCoins;
						if (num < num2)
						{
							long allcount = num2 - num;
							LocalSave.Instance.UserInfo_SetGold((int)num);
							LocalSave.Instance.Modify_Gold(allcount, updateui: false);
							CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, allcount);
						}
						else
						{
							LocalSave.Instance.UserInfo_SetGold((int)cRespDimaonToCoin.m_nCoins);
						}
						Button_Close.onClick();
					}
					else if (response.error != null)
					{
						CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
					}
				});
			}
		};
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
