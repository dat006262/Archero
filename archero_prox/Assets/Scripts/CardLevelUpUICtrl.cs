




using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardLevelUpUICtrl : MediatorCtrlBase
{
	public Text Text_CardName;

	public TapToCloseCtrl mCloseCtrl;

	public Transform CardParent;

	public Transform AttributeParent;

	private CardOneCtrl _cardctrl;

	private List<CardLevelUpAttCtrl> mAttList = new List<CardLevelUpAttCtrl>();

	private List<CardLevelUpAtt2Ctrl> mAtt2List = new List<CardLevelUpAtt2Ctrl>();

	private LocalSave.CardOne mData;

	private Action onEventClose;

	private CardOneCtrl mCardCtrl
	{
		get
		{
			if (_cardctrl == null)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/CardUI/CardOne"));
				_cardctrl = gameObject.GetComponent<CardOneCtrl>();
				Transform transform = gameObject.transform;
				transform.SetParent(CardParent);
				transform.localPosition = Vector3.zero;
				transform.localScale = Vector3.one;
				transform.localRotation = Quaternion.identity;
			}
			return _cardctrl;
		}
	}

	protected override void OnInit()
	{
	}

	protected override void OnOpen()
	{
		IProxy proxy = Facade.Instance.RetrieveProxy("CardLevelUpProxy");
		if (proxy != null)
		{
			onEventClose = proxy.Event_Para0;
			mData = (proxy.Data as LocalSave.CardOne);
			Text_CardName.text = GameLogic.Hold.Language.GetLanguageByTID(Utils.FormatString("宝物名称{0}", mData.CardID));
			Text_CardName.transform.localPosition = Vector3.zero;
			mCloseCtrl.OnClose = OnClickClose;
			if (mData.CardID == LocalSave.Instance.Card_GetHarvestID() && LocalSave.Instance.Card_GetHarvestLevel() == 1)
			{
				LocalSave.Instance.mHarvest.Unlock();
				CReqItemPacket itemPacket = NetManager.GetItemPacket(null);
				itemPacket.m_nPacketType = 17;
				NetManager.SendInternal(itemPacket, SendType.eCache, delegate(NetResponse response)
				{
#if ENABLE_NET_MANAGER
					if (response.IsSuccess)
#endif
					{
						LocalSave.Instance.mHarvest.init_last_time(Utils.GetTimeStamp());
						SdkManager.send_event_harvest("unlock", string.Empty, string.Empty, 0, 0);
					}
				});
			}
			mCardCtrl.InitCard(mData);
			mCardCtrl.SetNameShow(value: false);
			mCardCtrl.transform.localPosition = Vector3.zero;
			mCardCtrl.SetAlpha(0f);
			UpdateUI();
		}
	}

	private void UpdateUI()
	{
		mCloseCtrl.Show(value: false);
		for (int i = 0; i < mAttList.Count; i++)
		{
			mAttList[i].gameObject.SetActive(value: false);
		}
		for (int j = 0; j < mAtt2List.Count; j++)
		{
			mAtt2List[j].gameObject.SetActive(value: false);
		}
		Sequence s = DOTween.Sequence();
		Tweener t = Text_CardName.transform.DOLocalMoveY(100f, 0.3f);
		s.Append(t);
		s.Append(mCardCtrl.PlayCanvas(0f, 1f, 0.3f));
		s.Join(mCardCtrl.transform.DOLocalMoveY(100f, 0.3f));
		s.Append(UpdateAttribute());
		s.AppendCallback(delegate
		{
			mCloseCtrl.Show(value: true);
		});
	}

	private Sequence UpdateAttribute()
	{
		Sequence sequence = DOTween.Sequence();
		if (mData.level == 1)
		{
			int num = mData.data.BaseAttributes.Length;
			for (int i = 0; i < num; i++)
			{
				CardLevelUpAtt2Ctrl att2One = GetAtt2One(i);
				att2One.transform.localPosition = new Vector3(0f, i * -150, 0f);
				att2One.transform.localScale = Vector3.zero;
				att2One.UpdateUI(mData, i);
				sequence.Append(att2One.GetTweener());
			}
			for (int j = num; j < mAtt2List.Count; j++)
			{
				mAtt2List[j].gameObject.SetActive(value: false);
			}
		}
		else
		{
			int num2 = mData.data.BaseAttributes.Length;
			for (int k = 0; k < num2; k++)
			{
				CardLevelUpAttCtrl attOne = GetAttOne(k);
				attOne.transform.localPosition = new Vector3(0f, k * -150, 0f);
				attOne.transform.localScale = Vector3.zero;
				attOne.UpdateUI(mData, k);
				sequence.Append(attOne.GetTweener());
			}
			for (int l = num2; l < mAttList.Count; l++)
			{
				mAttList[l].gameObject.SetActive(value: false);
			}
		}
		return sequence;
	}

	private CardLevelUpAttCtrl GetAttOne(int index)
	{
		if (mAttList.Count > index)
		{
			mAttList[index].gameObject.SetActive(value: true);
			return mAttList[index];
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/CardLevelUpUI/AttributeOne"));
		Transform transform = gameObject.transform;
		transform.SetParent(AttributeParent);
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		mAttList.Add(gameObject.GetComponent<CardLevelUpAttCtrl>());
		return mAttList[mAttList.Count - 1];
	}

	private CardLevelUpAtt2Ctrl GetAtt2One(int index)
	{
		if (mAtt2List.Count > index)
		{
			mAtt2List[index].gameObject.SetActive(value: true);
			return mAtt2List[index];
		}
		GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/CardLevelUpUI/AttributeUnLock"));
		Transform transform = gameObject.transform;
		transform.SetParent(AttributeParent);
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		mAtt2List.Add(gameObject.GetComponent<CardLevelUpAtt2Ctrl>());
		return mAtt2List[mAtt2List.Count - 1];
	}

	private void OnClickClose()
	{
		if (onEventClose != null)
		{
			onEventClose();
		}
		WindowUI.CloseWindow(WindowID.WindowID_CardLevelUp);
	}

	protected override void OnClose()
	{
	}

	public override void OnHandleNotification(INotification notification)
	{
	}

	public override object OnGetEvent(string eventName)
	{
		return null;
	}

	public override void OnLanguageChange()
	{
	}
}
