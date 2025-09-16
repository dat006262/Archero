




using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ShopItemDiamondBoxNormal : ShopItemDiamondBoxBase, AdsRequestHelper.AdsCallback
{
	public Image Image_Ad;

	private Box_SilverNormalBox mData;

	private bool bWatchEnd;

	protected override void OnAwake()
	{
		mTransfer.source = EquipSource.EDiamond_box_normal;
		mTransfer.boxtype = LocalSave.TimeBoxType.BoxChoose_DiamondNormal;
		mTransfer.retry_callback = delegate
		{
			onClickButtonInternal(mTransfer.count);
		};
	}

	protected override void OnInit()
	{
		bWatchEnd = false;
		LocalSave.Instance.mGuideData.CheckDiamondBox(NotFreeParent.transform as RectTransform, 1);
		mData = LocalModelManager.Instance.Box_SilverNormalBox.GetBeanById(LocalSave.Instance.Stage_GetStage());
		mTransfer.diamonds = mData.Price1;
		PerTime = mData.Time * 60;
		mGoldCtrl.SetValue(get_price(0));
		FreeShow(value: false);
		UpdateBox();
		if (LocalSave.Instance.IsAdFree())
		{
			Image_Ad.enabled = false;
			RectTransform rectTransform = Text_Free.rectTransform;
			Vector2 anchoredPosition = Text_Free.rectTransform.anchoredPosition;
			rectTransform.anchoredPosition = new Vector2(0f, anchoredPosition.y);
		}
		else
		{
			Image_Ad.enabled = true;
			RectTransform rectTransform2 = Text_Free.rectTransform;
			float text_FreeX = Text_FreeX;
			Vector2 anchoredPosition2 = Text_Free.rectTransform.anchoredPosition;
			rectTransform2.anchoredPosition = new Vector2(text_FreeX, anchoredPosition2.y);
		}
		AdsRequestHelper.getRewardedAdapter().RemoveCallback(this);
		AdsRequestHelper.getRewardedAdapter().AddCallback(this);
	}

	protected override void OnDeinit()
	{
		AdsRequestHelper.getRewardedAdapter().RemoveCallback(this);
	}

	private int get_price(int opencount)
	{
		if (opencount < mData.Price1.Length)
		{
			return mData.Price1[opencount];
		}
		return mData.Price1[mData.Price1.Length - 1];
	}

	protected override void OnClickButton()
	{
		LocalSave.Instance.mGuideData.SetIndex(2);
		mTransfer.ResetCount();
		onClickButtonInternal(mTransfer.count);
	}

	private void onClickButtonInternal(int count)
	{
		bool flag = false;
		bool flag2 = false;
		if (LocalSave.Instance.GetDiamondBoxFreeCount(mBoxType) > 0)
		{
			flag = true;
			flag2 = true;
		}
		else
		{
			count = MathDxx.Clamp(count, 0, mTransfer.diamonds.Length - 1);
			if (CheckCanOpen(2, get_price(count)))
			{
				flag2 = true;
			}
		}
		if (!flag2)
		{
			return;
		}
		if (LocalSave.Instance.GetTimeBoxCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal) > 0)
		{
			SdkManager.send_event_ad(ADSource.eDiamondNormal, "CLICK", 0, 0, string.Empty, string.Empty);
		}
		if (!NetManager.IsNetConnect)
		{
			CInstance<TipsUIManager>.Instance.Show(ETips.Tips_NetError);
			if (LocalSave.Instance.GetTimeBoxCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal) > 0)
			{
				SdkManager.send_event_ad(ADSource.eDiamondNormal, "IMPRESSION", 0, 0, "FAIL", "NET_ERROR");
			}
		}
		else if (LocalSave.Instance.GetTimeBoxCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal) > 0)
		{
			if (LocalSave.Instance.IsAdFree())
			{
				send_get_box();
			}
			else if (!AdsRequestHelper.getRewardedAdapter().isLoaded())
			{
				SdkManager.send_event_ad(ADSource.eDiamondNormal, "IMPRESSION", 0, 0, "FAIL", "AD_NOT_READY");
				WindowUI.ShowAdInsideUI(AdInsideProxy.EnterSource.eGameTurn, delegate
				{
					SdkManager.send_event_ad(ADSource.eDiamondNormal, "REWARD", 0, 0, "INSIDE", string.Empty);
					send_get_box();
				});
			}
			else
			{
				AdsRequestHelper.getRewardedAdapter().Show(this);
				SdkManager.send_event_ad(ADSource.eDiamondNormal, "IMPRESSION", 0, 0, "SUCCESS", string.Empty);
			}
		}
		else
		{
			send_get_box();
		}
	}

	private void send_get_box()
	{
		bool free = false;
		if (LocalSave.Instance.GetDiamondBoxFreeCount(mBoxType) > 0)
		{
			free = true;
		}
		List<Drop_DropModel.DropData> list = LocalModelManager.Instance.Drop_Drop.GetDiamondBoxNormal();
        Debug.Log("@LOG ShopItemDiamondBoxNormal.send_get_box list:" + list.Count);
        foreach (var item in list)
        {
            UnityEngine.Debug.Log("@LOG ShopItemDiamondBoxNormal.send_get_box item.id:" + item.id + "|item.uniqueid:" + item.uniqueid);
        }
        //@TODO ADD Equipment Item
#if ENABLE_NET_MANAGER
        CReqItemPacket itemPacket = NetManager.GetItemPacket(list);
#else
        CReqItemPacket itemPacket = NetManager.GetItemPacket(list, true);
#endif

        itemPacket.m_nPacketType = 2;
		int diamondup = get_price(mTransfer.count);
		itemPacket.m_nDiamondAmount = (uint)diamondup;
		NetManager.SendInternal(itemPacket, SendType.eForceOnce, delegate(NetResponse response)
		{
#if ENABLE_NET_MANAGER
			if (response.IsSuccess)
#endif
			{
				if (LocalSave.Instance.GetTimeBoxCount(mBoxType) > 0)
				{
					LocalSave.Instance.Modify_TimeBoxCount(mBoxType, -1);
					UpdateBox();
				}
				else if (LocalSave.Instance.GetDiamondExtraCount(mBoxType) > 0)
				{
					LocalSave.Instance.Modify_DiamondExtraCount(mBoxType, -1);
				}
				mTransfer.data = list[0];
				if (!free)
				{
					mTransfer.AddCount();
				}
                UnityEngine.Debug.Log("@LOG send_get_box equipment:" + mTransfer.data.id);
				update_red();
                Facade.Instance.RegisterProxy(new BoxOpenSingleProxy(mTransfer));
				WindowUI.CloseWindow(WindowID.WindowID_BoxOpenSingle);
				WindowUI.ShowWindow(WindowID.WindowID_BoxOpenSingle);
				string purchase = (!free) ? "normalpurchasegems" : "normalpurchasefree";
				int num = (!free) ? diamondup : 0;
				LocalSave.Instance.Modify_Diamond(-num);
				SdkManager.send_event_shop(purchase, 0, num, mTransfer.data.id, mTransfer.count);
			}
#if ENABLE_NET_MANAGER
			else if (response.error != null)
			{
				CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 2);
			}
#endif
		});
	}

	protected override void OnLanguageChange()
	{
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_diamondbox_title_normal");
		Text_BoxContent.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_diamondbox_content_normal");
		Text_Free.text = GameLogic.Hold.Language.GetLanguageByTID("商店_免费抽取");
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("商店_抽一次");
	}

	public void onRequest(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("DiamondNormal onRequest");
	}

	public void onLoad(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("DiamondNormal onLoad");
	}

	public void onFail(AdsRequestHelper.AdsDriver sender, string msg)
	{
		AdsRequestHelper.DebugLog("DiamondNormal onFail");
	}

	public void onOpen(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("DiamondNormal onOpen");
	}

	public void onClose(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("DiamondNormal onClose");
		DOTween.Sequence().AppendInterval(0.1f).AppendCallback(delegate
		{
			if (!bWatchEnd)
			{
				SdkManager.send_event_ad(ADSource.eDiamondNormal, "CLOSE_BEFORE", 0, 0, string.Empty, string.Empty);
			}
		});
	}

	public void onClick(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		AdsRequestHelper.DebugLog("DiamondNormal onClick");
	}

	public void onReward(AdsRequestHelper.AdsDriver sender, string networkName)
	{
		bWatchEnd = true;
		SdkManager.send_event_ad(ADSource.eDiamondNormal, "REWARD", 0, 0, "BUSINESS", string.Empty);
		send_get_box();
	}
}
