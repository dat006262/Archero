




using DG.Tweening;
using Dxx.Net;
using Dxx.Util;
using GameProtocol;
using PureMVC.Interfaces;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class AdHarvestUICtrl : MediatorCtrlBase
{
	public UILineCtrl mTitleCtrl;

	public Text Text_Gold;

	public Text Text_EquipExp;

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	public UILineCtrl mUILineCtrl;

	public Text Text_reward1;

	public Text Text_reward2;

	public ButtonCtrl Button_Harvest;

	public ScrollRectBase mScrollRect;

	private const int LineCount = 6;

	private const float WidthOne = 140f;

	private const float HeightOne = 140f;

	private AdHarvestBattleCtrl _battlectrl;

	private GameObject _harvestitem;

	private LocalUnityObjctPool mPool;

	private SequencePool mSeqPool = new SequencePool();

	private List<Drop_DropModel.DropData> mDataList = new List<Drop_DropModel.DropData>();

	private string adharvest_time;

	private int reward_interval;

	private float scrollwidth;

	private LoadSyncCtrl mLoadCtrl;

	private bool bCanReward;

	private long time;

	private long waittime;

	private AdHarvestBattleCtrl mBattleCtrl
	{
		get
		{
			if (_battlectrl == null)
			{
				GameObject gameObject = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/AdHarvestUI/harvest_battle"));
				gameObject.transform.parent = null;
				gameObject.transform.position = Vector3.zero;
				_battlectrl = gameObject.GetComponent<AdHarvestBattleCtrl>();
			}
			return _battlectrl;
		}
	}

	private GameObject harvestitem
	{
		get
		{
			if (_harvestitem == null)
			{
				_harvestitem = CInstance<UIResourceCreator>.Instance.GetPropOneEquip(base.transform).gameObject;
				_harvestitem.SetActive(value: false);
			}
			return _harvestitem;
		}
	}

	protected override void OnInit()
	{
		reward_interval = 3600;
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<PropOneEquip>(harvestitem);
		Vector2 sizeDelta = (mScrollRect.transform.parent as RectTransform).sizeDelta;
		scrollwidth = sizeDelta.x;
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_AdHarvest);
		};
		Button_Shadow.onClick = Button_Close.onClick;
		Button_Harvest.onClick = delegate
		{
			AdHarvestUICtrl adHarvestUICtrl = this;
			SdkManager.send_event_harvest("click", string.Empty, string.Empty, 0, 0);
			List<Drop_DropModel.DropData> list = LocalSave.Instance.mHarvest.GetList();
			CReqItemPacket itemPacket = NetManager.GetItemPacket(list);
			itemPacket.m_nPacketType = 17;
			NetManager.SendInternal(itemPacket, SendType.eForceOnce, delegate(NetResponse response)
			{
#if ENABLE_NET_MANAGER
				if (response.IsSuccess)
#endif
				{
					int num = 0;
					int num2 = 0;
					int i = 0;
					for (int count = list.Count; i < count; i++)
					{
						Drop_DropModel.DropData dropData = list[i];
						if (dropData.type == PropType.eCurrency)
						{
							if (dropData.id == 1)
							{
								num += dropData.count;
							}
						}
						else if (dropData.is_equipexp)
						{
							num2 += dropData.count;
						}
					}
					SdkManager.send_event_harvest("end", "success", string.Empty, num, num2);
					LocalSave.Instance.mHarvest.Get_to_pack();
					adHarvestUICtrl.Button_Close.onClick();
				}
#if ENABLE_NET_MANAGER
				else
				{
					SdkManager.send_event_harvest("end", "fail", "server_resp_error", 0, 0);
				}
#endif
			});
		};
		OnLanguageChange();
	}

	protected override void OnOpen()
	{
		SdkManager.send_event_harvest("show", string.Empty, string.Empty, 0, 0);
		InitUI();
	}

	private void InitUI()
	{
		Update();
		mSeqPool.Clear();
		mPool.Collect<PropOneEquip>();
		LocalSave.Instance.mHarvest.refresh_rewards();
		mDataList = LocalSave.Instance.mHarvest.GetList();
		Sequence s = mSeqPool.Get();
		int count = mDataList.Count;
		for (int i = 0; i < count; i++)
		{
			int index = i;
			s.AppendCallback(delegate
			{
				PropOneEquip propOneEquip = mPool.DeQueue<PropOneEquip>();
				RectTransform rectTransform = propOneEquip.transform as RectTransform;
				rectTransform.SetParentNormal(mScrollRect.content);
				rectTransform.SetLeftTop();
				propOneEquip.InitProp(mDataList[index]);
				float x = (float)(index % 6) * 140f + 10f;
				float y = (float)(-index / 6) * 140f - 10f;
				rectTransform.anchoredPosition = new Vector2(x, y);
			});
			s.AppendInterval(0.03f);
		}
		int num = MathDxx.CeilBig((float)count / 6f);
		RectTransform content = mScrollRect.content;
		Vector2 sizeDelta = mScrollRect.content.sizeDelta;
		content.sizeDelta = new Vector2(sizeDelta.x, 140f * (float)num);
		mScrollRect.enabled = (count > 12);
	}

	private void UpdateTime()
	{
	}

	private void Update()
	{
		time = LocalSave.Instance.mHarvest.get_harvest_time();
		waittime = reward_interval - time;
		mUILineCtrl.SetText(Utils.FormatString("{0} {1}", adharvest_time, Utils.GetSecond3String(time)));
		bCanReward = LocalSave.Instance.mHarvest.get_can_reward();
		Button_Harvest.SetEnable(bCanReward);
		Button_Harvest.enabled = bCanReward;
		if (bCanReward)
		{
			Text_reward1.text = GameLogic.Hold.Language.GetLanguageByTID("adharvest_button");
			Text_reward1.rectTransform.anchoredPosition = new Vector2(0f, 4f);
			Text_reward1.fontSize = 45;
			Text_reward2.enabled = false;
		}
		else
		{
			Text_reward2.enabled = true;
			Text_reward1.fontSize = 35;
			Text_reward1.text = GameLogic.Hold.Language.GetLanguageByTID("adharvest_reward1", Utils.GetSecond3String(waittime));
			Text_reward2.text = GameLogic.Hold.Language.GetLanguageByTID("adharvest_reward2");
			Text_reward1.rectTransform.anchoredPosition = new Vector2(0f, 24f);
			Text_reward2.rectTransform.anchoredPosition = new Vector2(0f, -16f);
		}
		if (LocalSave.Instance.mHarvest.refresh_rewards())
		{
			InitUI();
		}
	}

	protected override void OnClose()
	{
		if (mLoadCtrl != null)
		{
			mLoadCtrl.DeInit();
		}
		mSeqPool.Clear();
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
		string languageByTID = GameLogic.Hold.Language.GetLanguageByTID("adharvest_minute");
		int num = LocalSave.Instance.Card_GetHarvestGold();
		Text_Gold.text = Utils.FormatString("+{0}/{1}", num, languageByTID);
		Text_EquipExp.text = GameLogic.Hold.Language.GetLanguageByTID("adharvest_equipexp_drop");
		mTitleCtrl.SetText(GameLogic.Hold.Language.GetLanguageByTID("adharvest_title"));
		adharvest_time = GameLogic.Hold.Language.GetLanguageByTID("adharvest_time");
		Text_reward2.text = GameLogic.Hold.Language.GetLanguageByTID("adharvest_reward2");
	}
}
