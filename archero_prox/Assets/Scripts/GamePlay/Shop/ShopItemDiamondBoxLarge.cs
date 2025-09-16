




using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;

public class ShopItemDiamondBoxLarge : ShopItemDiamondBoxBase
{
	private Box_SilverBox mData;

	protected override void OnAwake()
	{
		mTransfer.source = EquipSource.EDiamond_box_large;
		mTransfer.boxtype = LocalSave.TimeBoxType.BoxChoose_DiamondLarge;
		mTransfer.retry_callback = delegate
		{
			onClickButtonInternal(mTransfer.count);
		};
		mBoxType = LocalSave.TimeBoxType.BoxChoose_DiamondLarge;
	}

	protected override void OnInit()
	{
		mData = LocalModelManager.Instance.Box_SilverBox.GetBeanById(LocalSave.Instance.Stage_GetStage());
		mTransfer.diamonds = mData.Price1;
		PerTime = mData.Time * 60;
		mGoldCtrl.SetValue(get_price(0));
		FreeShow(value: false);
		UpdateBox();
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
		mTransfer.ResetCount();
		onClickButtonInternal(mTransfer.count);
	}

	private void onClickButtonInternal(int count)
	{
		bool flag = false;
		if (LocalSave.Instance.GetDiamondBoxFreeCount(mBoxType) > 0)
		{
			flag = true;
		}
		else
		{
			count = MathDxx.Clamp(count, 0, mTransfer.diamonds.Length - 1);
			if (CheckCanOpen(2, get_price(count)))
			{
				flag = true;
			}
		}
		if (flag)
		{
			send_get_box();
		}
	}

	protected override void OnLanguageChange()
	{
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_diamondbox_title_large");
		Text_BoxContent.text = GameLogic.Hold.Language.GetLanguageByTID("shopui_diamondbox_content_large");
		Text_Free.text = GameLogic.Hold.Language.GetLanguageByTID("商店_免费抽取");
		Text_Content.text = GameLogic.Hold.Language.GetLanguageByTID("商店_抽一次");
	}

	private void send_get_box()
	{
		bool free = false;
		if (LocalSave.Instance.GetDiamondBoxFreeCount(mBoxType) > 0)
		{
			free = true;
		}
		List<Drop_DropModel.DropData> list = LocalModelManager.Instance.Drop_Drop.GetDiamondBoxLarge();
        UnityEngine.Debug.Log("@LOG ShopItemDiamondBoxLarge.send_get_box list:" + list.Count);
        foreach(var item in list)
        {
            UnityEngine.Debug.Log("@LOG ShopItemDiamondBoxLarge.send_get_box item.id:" + item.id + "|item.uniqueid:" + item.uniqueid);
        }
        //@TODO ADD Equipment Item
#if ENABLE_NET_MANAGER
        CReqItemPacket itemPacket = NetManager.GetItemPacket(list);
#else
        CReqItemPacket itemPacket = NetManager.GetItemPacket(list, true);
#endif

        itemPacket.m_nPacketType = 5;
		ushort diamondup = (ushort)get_price(mTransfer.count);
		itemPacket.m_nDiamondAmount = diamondup;
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
				update_red();
				mTransfer.data = list[0];
				if (!free)
				{
					mTransfer.AddCount();
				}
                UnityEngine.Debug.Log("@LOG send_get_box equipment:" + mTransfer.data.id);
				Facade.Instance.RegisterProxy(new BoxOpenSingleProxy(mTransfer));
				WindowUI.CloseWindow(WindowID.WindowID_BoxOpenSingle);
				WindowUI.ShowWindow(WindowID.WindowID_BoxOpenSingle);
				string purchase = (!free) ? "largepurchasegems" : "largepurchasefree";
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
}
