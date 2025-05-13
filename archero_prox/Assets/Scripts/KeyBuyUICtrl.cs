




using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class KeyBuyUICtrl : MediatorCtrlBase, AdsRequestHelper.AdsCallback
{
	private static KeyBuySource mSource;

	public Text Text_Title;

	public Text Text_Content;

	public Text Text_DiamondCount;

	public Text Text_AdCount;

	public Text Text_AdLast;

	public Text Text_AdGet;

	public Image Image_Ad;

	public GameObject freeparent;

	public GameObject notfreeparent;

	public Text Text_NotFreeCount;

	public GoldTextCtrl mAdCtrl;

	public GoldTextCtrl mDiamondCtrl;

	public GoldTextCtrl mNotFreeDiamondCtrl;

	public ButtonCtrl Button_Buy;

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	public ButtonCtrl Button_Ad;

	public ButtonCtrl Button_BuyNotFree;

	private float Text_AdGetX;

	private int KeyCount;

	private int adCount;

	private long needdiamond;

	private bool bAdReward;

	public static void SetSource(KeyBuySource source)
	{
		mSource = source;
	}

	public static KeyBuySource GetSource()
	{
		return mSource;
	}

	protected override void OnInit()
	{
		Vector2 anchoredPosition = Text_AdGet.rectTransform.anchoredPosition;
		Text_AdGetX = anchoredPosition.x;
		KeyCount = GameConfig.GetMaxKeyCount();
		adCount = GameConfig.GetAdKeyCount();
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_KeyBuy);
		};
		Button_Ad.SetDepondNet(value: true);
		Button_Ad.onClick = delegate
		{
			bAdReward = false;
			SdkManager.send_event_ad(ADSource.eKey, "CLICK", 0, 0, string.Empty, string.Empty);
			if (LocalSave.Instance.UserInfo_GetAdKeyCount() > 0)
			{
				if (!NetManager.IsNetConnect)
				{
					CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
					SdkManager.send_event_ad(ADSource.eKey, "IMPRESSION", 0, 0, "FAIL", "NET_ERROR");
				}
				else if (LocalSave.Instance.IsAdFree())
				{
					onReward(null, null);
				}
				else if (!AdsRequestHelper.getRewardedAdapter().isLoaded())
				{
					SdkManager.send_event_ad(ADSource.eKey, "IMPRESSION", 0, 0, "FAIL", "AD_NOT_READY");
					CInstance<TipsUIManager>.Instance.Show(ETips.Tips_AdNotReady);
				}
				else
				{
					AdsRequestHelper.getRewardedAdapter().Show(this);
					SdkManager.send_event_ad(ADSource.eKey, "IMPRESSION", 0, 0, "SUCCESS", string.Empty);
				}
			}
		};
		Button_Shadow.onClick = Button_Close.onClick;
	}

	protected override void OnOpen()
	{
		AdsRequestHelper.getRewardedAdapter().AddCallback(this);
		SdkManager.send_event_strength("CLICK", GetSource(), string.Empty, string.Empty, 0);
		GameLogic.Hold.Sound.PlayUI(SoundUIType.ePopUI);
		InitUI();
	}

	private void InitUI()
	{
		needdiamond = GameConfig.GetKeyBuyDiamond();
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("key_buy_content", KeyCount.ToString());
		Text_DiamondCount.text = Utils.FormatString("x{0}", KeyCount);
		Text_NotFreeCount.text = Utils.FormatString("x{0}", KeyCount);
		Text_AdCount.text = Utils.FormatString("x{0}", adCount);
		update_ad_count();
		mDiamondCtrl.SetCurrencyType(CurrencyType.Diamond);
		mDiamondCtrl.UseTextRed();
		mDiamondCtrl.SetValue((int)needdiamond);
		mNotFreeDiamondCtrl.SetCurrencyType(CurrencyType.Diamond);
		mNotFreeDiamondCtrl.UseTextRed();
		mNotFreeDiamondCtrl.SetValue((int)needdiamond);
		Button_Buy.onClick = delegate
		{
			long diamond = LocalSave.Instance.GetDiamond();
			if (diamond < needdiamond)
			{
				WindowUI.ShowShopSingle(ShopSingleProxy.SingleType.eDiamond);
			}
			else
			{
				SdkManager.send_event_strength("PURCHASE_CLICK", GetSource(), string.Empty, string.Empty, 0);
				CLifeTransPacket packet = new CLifeTransPacket
				{
					m_nTransID = LocalSave.Instance.SaveExtra.GetTransID(),
					m_nType = 2,
					m_nMaterial = (ushort)needdiamond
				};
				Debug.LogError("buy key needdiamond = " + needdiamond);
                //@TODO
#if !ENABLE_BEST_HTTP
                LocalSave.Instance.Modify_Diamond(-needdiamond);
                CurrencyFlyCtrl.PlayGet(CurrencyType.Key, KeyCount);
                Button_Close.onClick();
                SdkManager.send_event_strength("FINISH", GetSource(), "SUCCESS", string.Empty, (int)needdiamond);
#else
                NetManager.SendInternal(packet, SendType.eForceOnce, delegate(NetResponse response)
				{
					if (response.IsSuccess)
					{
						LocalSave.Instance.Modify_Diamond(-needdiamond);
						CurrencyFlyCtrl.PlayGet(CurrencyType.Key, KeyCount);
						Button_Close.onClick();
						SdkManager.send_event_strength("FINISH", GetSource(), "SUCCESS", string.Empty, (int)needdiamond);
					}
					else if (response.error != null)
					{
						SdkManager.send_event_strength("FINISH", GetSource(), "FAIL", "DIAMOND_NOT_ENOUGH", 0);
						CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
					}
					else
					{
						SdkManager.send_event_strength("FINISH", GetSource(), "FAIL", "RESPONSE_ERROR_NULL", 0);
					}
				});
#endif
            }
		};
		Button_BuyNotFree.onClick = Button_Buy.onClick;
	}

	private void update_ad_count()
	{
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("key_ad_count");
		int num = LocalSave.Instance.UserInfo_GetAdKeyCount();
		Text_AdLast.text = Utils.FormatString("{0}: {1}", languageByTID, num);
		string languageByTID2 = GameLogic.Hold.Language.GetLanguageByTID("key_ad_get");
		if (!LocalSave.Instance.IsAdFree())
		{
			Image_Ad.enabled = true;
			mAdCtrl.Interval = 10f;
			mAdCtrl.SetValue(languageByTID2);
		}
		else
		{
			Image_Ad.enabled = false;
			RectTransform rectTransform = Text_AdGet.rectTransform;
			Vector2 anchoredPosition = Text_AdGet.rectTransform.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(0f, anchoredPosition.y);
		}
		if (num > 0)
		{
			freeparent.SetActive(value: true);
			notfreeparent.SetActive(value: false);
		}
		else
		{
			freeparent.SetActive(value: false);
			notfreeparent.SetActive(value: true);
		}
	}

	protected override void OnClose()
	{
		AdsRequestHelper.getRewardedAdapter().RemoveCallback(this);
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
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("key_buy_title");
	}

	public void onRequest(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("Key onRequest");
	}

	public void onLoad(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("Key onLoad");
	}

	public void onFail(AdsRequestHelper.AdsDriver sender, string msg)
	{
		AdsRequestHelper.DebugLog("Key onFail");
	}

	public void onOpen(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("Key onOpen");
	}

	public void onClose(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("Key onClose");
		DOTween.Sequence().AppendInterval(0.1f).AppendCallback(delegate
		{
			if (!bAdReward)
			{
				SdkManager.send_event_ad(ADSource.eKey, "CLOSE_BEFORE", 0, 0, string.Empty, string.Empty);
			}
		});
	}

	public void onClick(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("Key onClick");
	}

	public void onReward(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		bAdReward = true;
		if (LocalSave.Instance.UserInfo_GetAdKeyCount() > 0)
		{
			LocalSave.Instance.UserInfo_UseAdKeyCount();
			update_ad_count();
		}
		AdsRequestHelper.DebugLog(Utils.FormatString("Key Reward adCount : {0}", adCount));
		List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
		list.Add(new Drop_DropModel.DropData
		{
			type = PropType.eCurrency,
			id = 3,
			count = adCount
		});
		CReqItemPacket itemPacket = NetManager.GetItemPacket(list);
		itemPacket.m_nPacketType = 18;
		NetManager.SendInternal(itemPacket, SendType.eUDP, delegate(NetResponse response)
		{
#if ENABLE_NET_MANAGER
			if (response.IsSuccess)
#endif
			{
				SdkManager.send_event_ad(ADSource.eKey, "REWARD", 0, 0, string.Empty, string.Empty);
				AdsRequestHelper.DebugLog(Utils.FormatString("Key Reward adCount : {0} success", adCount));
			}
		});
		CurrencyFlyCtrl.PlayGet(CurrencyType.Key, adCount);
		Button_Close.onClick();
	}
}
