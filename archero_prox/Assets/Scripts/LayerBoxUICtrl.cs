




using Dxx.Net;
using GameProtocol;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class LayerBoxUICtrl : MediatorCtrlBase
{
	private const string Ani_Info_Show = "Info_Show";

	private const string Ani_Info_Hide = "Info_Hide";

	public RectTransform window;

	public Text Text_Title;

	public Text Text_RewardsContent;

	public ButtonCtrl Button_Close;

	public ScrollIntLayerBoxCtrl mScrollInt;

	public Transform mScrollChild;

	public GameObject GoodsParent;

	public GameObject copyBox;

	public GameObject copyReward;

	public Text Text_Condition;

	public Text Text_Rewards;

	public ButtonCtrl Button_Get;

	public Text Text_Get;

	public Text Text_Target;

	public Text Text_Got;

	private int showCount = 10;

	private int count = 40;

	private float allWidth;

	private float itemWidth;

	private float offsetx = 360f;

	private float lastscrollpos;

	private float lastspeed;

	private int mCurrentIndex;

	private List<LayerRewardOneCtrl> mRewards = new List<LayerRewardOneCtrl>();

	private List<Box_ChapterBox> mDataList;

	private LayerBoxOneCtrl mChoose;

	private LocalUnityObjctPool mRewardPool;

	private int currentid;

	private bool bFirst;

	protected override void OnInit()
	{
		float fringeHeight = PlatformHelper.GetFringeHeight();
		window.anchoredPosition = new Vector2(0f, fringeHeight);
		Button_Close.onClick = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_LayerBox);
		};
		mRewardPool = LocalUnityObjctPool.Create(base.gameObject);
		mRewardPool.CreateCache<LayerRewardOneCtrl>(copyReward);
		mScrollInt.copyItem = copyBox;
		mScrollInt.mScrollChild = mScrollChild;
		mScrollInt.OnUpdateOne = UpdateOne;
		mScrollInt.OnUpdateSize = UpdateSize;
		mScrollInt.OnBeginDragEvent = OnBeginDrag;
		mScrollInt.OnScrollEnd = OnScrollEnd;
		copyBox.SetActive(value: false);
		copyReward.SetActive(value: false);
		Button_Get.onClick = delegate
		{
			SendLayer(currentid);
		};
		mDataList = LocalModelManager.Instance.Box_ChapterBox.GetCurrentList();
		mScrollInt.DragDisableForce = true;
	}

	protected override void OnOpen()
	{
		WindowUI.ShowCurrency(WindowID.WindowID_Currency);
		bFirst = true;
		mScrollInt.Init(mDataList.Count);
		InitUI();
	}

	private void InitUI()
	{
		UpdateReward();
	}

	private void UpdateOne(int index, LayerBoxOneCtrl one)
	{
		one.Init(mDataList[index]);
		if (index == 0 && mChoose == null)
		{
			mChoose = one;
		}
	}

	private void UpdateSize(int index, LayerBoxOneCtrl one)
	{
		Box_ChapterBox box_ChapterBox = mDataList[index];
	}

	private void OnScrollEnd(int index, LayerBoxOneCtrl one)
	{
		mChoose = one;
		UpdateUI();
	}

	private void OnBeginDrag()
	{
	}

	private void SendLayer(int id)
	{
		List<Drop_DropModel.DropData> list = LocalModelManager.Instance.Box_ChapterBox.GetDrops(id);
		CReqItemPacket itemPacket = NetManager.GetItemPacket(list);
		itemPacket.m_nPacketType = 4;
		itemPacket.m_nExtraInfo = (ushort)id;
		NetManager.SendInternal(itemPacket, SendType.eForceOnce, delegate(NetResponse response)
		{
#if ENABLE_NET_MANAGER
			if (response.IsSuccess)
#endif
			{
				if (itemPacket.m_nCoinAmount != 0)
				{
					LocalSave.Instance.Modify_Gold(itemPacket.m_nCoinAmount, updateui: false);
				}
				if (itemPacket.m_nDiamondAmount != 0)
				{
					LocalSave.Instance.Modify_Diamond(itemPacket.m_nDiamondAmount, updateui: false);
				}
				if (itemPacket.m_nNormalDiamondItem > 0)
				{
					LocalSave.Instance.Modify_DiamondExtraCount(LocalSave.TimeBoxType.BoxChoose_DiamondNormal, itemPacket.m_nNormalDiamondItem);
				}
				if (itemPacket.m_nLargeDiamondItem > 0)
				{
					LocalSave.Instance.Modify_DiamondExtraCount(LocalSave.TimeBoxType.BoxChoose_DiamondLarge, itemPacket.m_nLargeDiamondItem);
				}
				LocalSave.Instance.Stage_GetNextEnd();
				PlayRewards(list);
				Facade.Instance.SendNotification("MainUI_LayerUpdate");
			}
#if ENABLE_NET_MANAGER
			else if (response.error != null)
			{
				CInstance<TipsUIManager>.Instance.ShowCode(response.error.m_nStatusCode);
			}
#endif
		});
	}

	private void PlayRewards(List<Drop_DropModel.DropData> list)
	{
		mRewardPool.Collect<LayerRewardOneCtrl>();
		Button_Get.gameObject.SetActive(value: false);
		Text_Rewards.gameObject.SetActive(value: false);
		bool is_fly = false;
		bool flag = false;
		int i = 0;
		for (int num = list.Count; i < num; i++)
		{
			Drop_DropModel.DropData dropData = list[i];
			if (dropData.can_fly)
			{
				flag = true;
				CurrencyFlyCtrl.PlayGet((CurrencyType)dropData.id, dropData.count, GetRewardPosition(dropData.id), null, delegate
				{
					if (!is_fly)
					{
						UpdateReward();
						is_fly = true;
					}
				});
			}
		}
		if (!flag)
		{
			UpdateReward();
		}
		mRewards.Clear();
	}

	private Vector3 GetRewardPosition(int id)
	{
		int i = 0;
		for (int num = mRewards.Count; i < num; i++)
		{
			if (mRewards[i].id == id)
			{
				return mRewards[i].transform.position;
			}
		}
		return new Vector3((float)GameLogic.Width / 2f, (float)GameLogic.Height / 2f, 0f);
	}

	private void UpdateReward()
	{
		Text_Rewards.gameObject.SetActive(value: true);
		currentid = LocalSave.Instance.Stage_GetNextID();
		int nextLevel = LocalModelManager.Instance.Box_ChapterBox.GetNextLevel(currentid);
		string stageLayer = GameLogic.Hold.Language.GetStageLayer(nextLevel);
		Text_Condition.text = GameLogic.Hold.Language.GetLanguageByTID("Main_StageCount", stageLayer);
		Text_Got.gameObject.SetActive(value: false);
		int num = currentid;
		if (currentid > mDataList.Count)
		{
			currentid = mDataList.Count;
		}
		mRewardPool.Collect<LayerRewardOneCtrl>();
		List<Drop_DropModel.DropData> drops = LocalModelManager.Instance.Box_ChapterBox.GetDrops(currentid);
		drops.Sort(delegate(Drop_DropModel.DropData a, Drop_DropModel.DropData b)
		{
			if (a.id == 3)
			{
				return -1;
			}
			if (b.id == 3)
			{
				return 1;
			}
			if (a.id == 1)
			{
				return -1;
			}
			return (b.id == 1) ? 1 : (-1);
		});
		mRewards.Clear();
		float num2 = 160f;
		float num3 = (float)(-(drops.Count - 1)) / 2f * num2;
		int i = 0;
		for (int num4 = drops.Count; i < num4; i++)
		{
			LayerRewardOneCtrl layerRewardOneCtrl = mRewardPool.DeQueue<LayerRewardOneCtrl>();
			Drop_DropModel.DropData dropData = drops[i];
			layerRewardOneCtrl.Init(dropData.id, dropData.count);
			RectTransform rectTransform = layerRewardOneCtrl.transform as RectTransform;
			rectTransform.SetParentNormal(GoodsParent);
			rectTransform.anchoredPosition = new Vector2(num3 + (float)i * num2, 0f);
			mRewards.Add(layerRewardOneCtrl);
		}
		int maxLevel = LocalSave.Instance.mStage.MaxLevel;
		int nextLevel2 = LocalModelManager.Instance.Box_ChapterBox.GetNextLevel(currentid);
		string stageLayer2 = GameLogic.Hold.Language.GetStageLayer(nextLevel2);
		Text_Target.text = GameLogic.Hold.Language.GetLanguageByTID("LayerBox_Target", stageLayer2);
		bool flag = maxLevel >= nextLevel2;
		Text_Target.gameObject.SetActive(!flag);
		Button_Get.gameObject.SetActive(flag);
		if (!bFirst)
		{
			mScrollInt.GotoInt(currentid - 1, playanimation: true);
		}
		else
		{
			mScrollInt.GotoInt(currentid - 1);
		}
		if (num > mDataList.Count)
		{
			mRewardPool.Collect<LayerRewardOneCtrl>();
			mRewards.Clear();
			mScrollInt.GotoInt(mDataList.Count - 1);
			Text_Target.gameObject.SetActive(value: false);
			Text_Got.gameObject.SetActive(value: true);
			Button_Get.gameObject.SetActive(value: false);
		}
		bFirst = false;
	}

	private void UpdateUI()
	{
	}

	protected override void OnClose()
	{
		WindowUI.CloseCurrency();
		mScrollInt.DeInit();
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
		Text_Title.text = GameLogic.Hold.Language.GetLanguageByTID("LayerBox_Title");
		Text_RewardsContent.text = GameLogic.Hold.Language.GetLanguageByTID("LayerBox_Rewards");
		Text_Got.text = GameLogic.Hold.Language.GetLanguageByTID("LayerBox_Got");
		Text_Get.text = GameLogic.Hold.Language.GetLanguageByTID("LayerBox_Get");
	}
}
