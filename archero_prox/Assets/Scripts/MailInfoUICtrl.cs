




using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class MailInfoUICtrl : MediatorCtrlBase
{
	public class RewardData
	{
		public int type;

		public int id;

		public int count;
	}

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	public Text Text_TitleTitle;

	public Text Text_Title;

	public Text Text_Time;

	public Text Text_Content;

	public GameObject rewardparent;

	public GameObject rewardchildparent;

	public Text Text_RewardContent;

	public ScrollRectBase mScrollRect;

	public RectTransform scrolltransform;

	public ButtonCtrl Button_Get;

	private const float RewardWidth = 130f;

	private const float RewardHeight = 80f;

	private const float OneWidth = 100f;

	private bool bHaveReward = true;

	private List<RewardData> mList = new List<RewardData>();

	private LocalUnityObjctPool mPool;

	private MailInfoProxy.Transfer mTranfer;

	private Vector3 mCoinPos;

	private Vector3 mDiamondPos;

	protected override void OnInit()
	{
		Button_Close.onClick = OnClickClose;
		Button_Shadow.onClick = OnClickClose;
		Button_Get.onClick = OnClickGet;
		GameObject gameObject = CInstance<UIResourceCreator>.Instance.GetPropOneEquip(base.transform).gameObject;
		gameObject.SetActive(value: false);
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<PropOneEquip>(gameObject);
	}

	protected override void OnOpen()
	{
		mCoinPos = new Vector3((float)GameLogic.Width / 2f, (float)GameLogic.Height / 2f, 0f);
		mDiamondPos = new Vector3((float)GameLogic.Width / 2f, (float)GameLogic.Height / 2f, 0f);
		mPool.Collect<PropOneEquip>();
		IProxy proxy = Facade.Instance.RetrieveProxy("MailInfoProxy");
		if (proxy == null)
		{
			SdkManager.Bugly_Report("MailInfoUICtrl", "MailInfoProxy is null.");
			OnClickClose();
			return;
		}
		if (proxy.Data == null)
		{
			SdkManager.Bugly_Report("MailInfoUICtrl", "MailInfoProxy.Data is null.");
			OnClickClose();
			return;
		}
		mTranfer = (proxy.Data as MailInfoProxy.Transfer);
		if (mTranfer == null)
		{
			SdkManager.Bugly_Report("MailInfoUICtrl", "MailInfoProxy.Data is not a [MailInfoProxy.Transfer] type.");
			OnClickClose();
		}
		else
		{
			InitUI();
		}
	}

	private void InitUI()
	{
		Text_Content.text = mTranfer.data.m_strContent;
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("邮件详情");
		Text_Time.text = Utils.GetTimeGo(mTranfer.data.m_i64PubTime);
		Text_TitleTitle.text = mTranfer.data.m_strTitle;
		RectTransform content = mScrollRect.content;
		Vector2 sizeDelta = mScrollRect.content.sizeDelta;
		content.sizeDelta = new Vector2(sizeDelta.x, Text_Content.preferredHeight);
		LocalSave.Instance.Mail.MailReaded(mTranfer.data);
		InitGet();
	}

	private void InitGet()
	{
		mList.Clear();
		if (mTranfer.data.m_nDiamond > 0)
		{
			mList.Add(new RewardData
			{
				type = 1,
				id = 2,
				count = mTranfer.data.m_nDiamond
			});
		}
		if (mTranfer.data.m_nCoins > 0)
		{
			mList.Add(new RewardData
			{
				type = 1,
				id = 1,
				count = mTranfer.data.m_nCoins
			});
		}
		bHaveReward = (mList.Count > 0 && !mTranfer.data.IsGot);
		rewardparent.SetActive(bHaveReward);
		for (int i = 0; i < mList.Count; i++)
		{
			int num = i;
			RewardData rewardData = mList[i];
			PropOneEquip propOneEquip = mPool.DeQueue<PropOneEquip>();
			propOneEquip.transform.SetLeft();
			propOneEquip.gameObject.SetParentNormal(rewardchildparent);
			propOneEquip.transform.localScale = Vector3.one * 0.6f;
			propOneEquip.transform.localPosition = new Vector3((float)(num % 5) * 100f, (float)(num / 5) * -100f);
			switch (rewardData.type)
			{
			case 1:
				propOneEquip.InitCurrency(rewardData.id, rewardData.count);
				if (rewardData.id == 1)
				{
					mCoinPos = propOneEquip.GetMiddlePosition();
				}
				else if (rewardData.id == 2)
				{
					mDiamondPos = propOneEquip.GetMiddlePosition();
				}
				break;
			case 3:
				propOneEquip.InitEquip(rewardData.id, rewardData.count);
				break;
			}
		}
		RefreshGot();
	}

	private void RefreshGot()
	{
		bool flag = (mTranfer.data.IsHaveReward && mTranfer.data.IsGot) || !mTranfer.data.IsHaveReward;
		rewardparent.SetActive(!flag);
		Button_Get.gameObject.SetActive(!flag);
	}

	private void OnClickGet()
	{
		List<Drop_DropModel.DropData> list = new List<Drop_DropModel.DropData>();
		if (mTranfer.data.m_nCoins > 0)
		{
			list.Add(new Drop_DropModel.DropData
			{
				type = PropType.eCurrency,
				id = 1,
				count = mTranfer.data.m_nCoins
			});
		}
		if (mTranfer.data.m_nDiamond > 0)
		{
			list.Add(new Drop_DropModel.DropData
			{
				type = PropType.eCurrency,
				id = 2,
				count = mTranfer.data.m_nDiamond
			});
		}
		CReqItemPacket itemPacket = NetManager.GetItemPacket(list);
		itemPacket.m_nPacketType = 6;
		itemPacket.m_nExtraInfo = mTranfer.data.m_nMailID;
		NetManager.SendInternal(itemPacket, SendType.eForceOnce, delegate(NetResponse response)
		{
#if ENABLE_NET_MANAGER
			if (response.IsSuccess)
#endif
			{
				LocalSave.Instance.Mail.MailGot(mTranfer.data);
				if (mTranfer.data.m_nCoins > 0)
				{
					LocalSave.Instance.Modify_Gold((int)mTranfer.data.m_nCoins, updateui: false);
					CurrencyFlyCtrl.PlayGet(CurrencyType.Gold, (int)mTranfer.data.m_nCoins, mCoinPos);
				}
				if (mTranfer.data.m_nDiamond > 0)
				{
					LocalSave.Instance.Modify_Diamond((int)mTranfer.data.m_nDiamond, updateui: false);
					CurrencyFlyCtrl.PlayGet(CurrencyType.Diamond, (int)mTranfer.data.m_nDiamond, mDiamondPos);
				}
				if (mTranfer.ctrl != null)
				{
					mTranfer.ctrl.UpdateMail();
				}
				OnClickClose();
			}
#if ENABLE_NET_MANAGER
			else if (response.error != null)
			{
				CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode, 1);
			}
#endif
		});
	}

	private void OnClickClose()
	{
		WindowUI.CloseWindow(WindowID.WindowID_MailInfo);
	}

	protected override void OnClose()
	{
		if (mTranfer.poptype == MailInfoProxy.EMailPopType.eMain)
		{
			LocalSave.Instance.Mail.CheckMainPop();
		}
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
