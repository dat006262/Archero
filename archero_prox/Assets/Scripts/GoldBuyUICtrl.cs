




using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using UnityEngine.UI;

public class GoldBuyUICtrl : MediatorCtrlBase
{
	private static CoinExchangeSource mSource;

	public Text Text_Title;

	public Text Text_Content;

	public GoldTextCtrl mDiamondCtrl;

	public ButtonCtrl Button_Buy;

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	private GoldBuyModuleProxy.Transfer mTransfer;

	private long needdiamond;

	public static void SetSource(CoinExchangeSource source)
	{
		mSource = source;
	}

	public static CoinExchangeSource GetSource()
	{
		return mSource;
	}

	protected override void OnInit()
	{
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_GoldBuy);
		};
		Button_Shadow.onClick = Button_Close.onClick;
		Button_Buy.SetDepondNet(value: true);
	}

	protected override void OnOpen()
	{
		GameLogic.Hold.Sound.PlayUI(SoundUIType.ePopUI);
		InitUI();
	}

	private void InitUI()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("GoldBuy");
		mTransfer = (proxy.Data as GoldBuyModuleProxy.Transfer);
		needdiamond = Formula.GetNeedDiamond(mTransfer.gold);
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("金币不足标题");
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("金币不足描述");
		mDiamondCtrl.SetCurrencyType(CurrencyType.Diamond);
		mDiamondCtrl.UseTextRed();
		mDiamondCtrl.SetValue((int)needdiamond);
		Button_Buy.onClick = delegate
		{
			long diamond = LocalSave.Instance.GetDiamond();
			if (diamond < needdiamond)
			{
				WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond);
			}
			else
			{
				GoldBuyUICtrl goldBuyUICtrl = this;
				CDiamondToCoin diamondToCoin = new CDiamondToCoin();
				diamondToCoin.m_nTransID = LocalSave.Instance.SaveExtra.GetTransID();
				diamondToCoin.m_nCoins = (uint)mTransfer.gold;
				diamondToCoin.m_nDiamonds = (uint)needdiamond;
				Debugger.Log("Send DiamondToCoin Request " + diamondToCoin.m_nCoins + " transid " + diamondToCoin.m_nTransID);
				NetManager.SendInternal(diamondToCoin, SendType.eForceOnce, delegate(NetResponse response)
				{
					if (response.IsSuccess && response.data != null && response.data is CRespDimaonToCoin)
					{
						SdkManager.send_event_exchange(GetSource(), (int)diamondToCoin.m_nCoins, (int)goldBuyUICtrl.needdiamond);
						CRespDimaonToCoin cRespDimaonToCoin = response.data as CRespDimaonToCoin;
						LocalSave.Instance.UserInfo_SetDiamond((int)cRespDimaonToCoin.m_nDiamonds);
						LocalSave.Instance.UserInfo_SetGold((int)cRespDimaonToCoin.m_nCoins);
						if (goldBuyUICtrl.mTransfer.callback != null)
						{
							goldBuyUICtrl.mTransfer.callback((int)goldBuyUICtrl.needdiamond);
						}
						goldBuyUICtrl.Button_Close.onClick();
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
