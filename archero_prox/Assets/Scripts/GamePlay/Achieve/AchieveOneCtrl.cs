




using Dxx.Util;
using System;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class AchieveOneCtrl : MonoBehaviour
{
	public GameObject unlockobj;

	public GameObject lockobj;

	public Text Text_Name;

	public Text Text_Info;

	public Text Text_Get;

	public ButtonCtrl Button_Get;

	public ProgressTextCtrl mProgressCtrl;

	public CanvasGroup mCanvasGroup;

	public Text Text_LockContent;

	public Transform rewardparent;

	public GameObject copyreward;

	public GameObject gotparent;

	public Action<int, AchieveOneCtrl> OnClickButton;

	[NonSerialized]
	public LocalSave.AchieveDataOne mData;

	private LocalUnityObjctPool mPool;

	private int mAchieveID;

	private int mIndex;

	private void Awake()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<GoldTextCtrl>(copyreward);
		copyreward.SetActive(value: false);
		Button_Get.onClick = delegate
		{
			if (OnClickButton != null)
			{
				OnClickButton(mIndex, this);
			}
		};
	}

	public void Init(int index, int achieveid)
	{
		mCanvasGroup.alpha = 1f;
		mAchieveID = achieveid;
		mIndex = index;
		mData = LocalSave.Instance.Achieve_Get(achieveid);
		Refresh();
	}

	public void Refresh()
	{
		Text_Get.text = GameLogic.Hold.Language.GetLanguageByTID("成就_领取");
		gotparent.SetActive(mData.isgot);
		rewardparent.gameObject.SetActive(!mData.isgot);
		Text_Name.text = Utils.FormatString("成就{0}", mData.mData.Index);
		if (mData.mData.IsGlobal)
		{
			Text_Info.text = mData.mCondition.GetConditionString();
		}
		else
		{
			Text_Info.text = Utils.FormatString("完成挑战{0}", mData.mData.Index);
		}
		bool isfinish = mData.isfinish;
		Button_Get.gameObject.SetActive(!mData.isgot && isfinish);
		mProgressCtrl.gameObject.SetActive(!mData.isgot && !isfinish);
		if (!isfinish)
		{
			mProgressCtrl.max = mData.mCondition.GetMax();
			mProgressCtrl.current = mData.mCondition.GetCurrent();
		}
		init_rewards();
	}

	private void init_rewards()
	{
		mPool.Collect<GoldTextCtrl>();
		if (!mData.isgot)
		{
			List<Drop_DropModel.DropData> rewards = mData.mData.GetRewards();
			float num = 0f;
			int num2 = rewards.Count - 1;
			while (num2 >= 0 && num2 < rewards.Count)
			{
				GoldTextCtrl goldTextCtrl = mPool.DeQueue<GoldTextCtrl>();
				Drop_DropModel.DropData dropData = rewards[num2];
				goldTextCtrl.SetCurrencyType(dropData.id);
				goldTextCtrl.SetValue(dropData.count);
				goldTextCtrl.gameObject.SetParentNormal(rewardparent);
				RectTransform rectTransform = goldTextCtrl.transform as RectTransform;
				rectTransform.anchoredPosition = new Vector2(num, 0f);
				num -= goldTextCtrl.GetWidth();
				num2--;
			}
		}
	}

	public void GetReward()
	{
	}
}
