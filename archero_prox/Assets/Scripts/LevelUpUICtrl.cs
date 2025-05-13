




using DG.Tweening;
using PureMVC.Interfaces;
using PureMVC.Patterns;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpUICtrl : MediatorCtrlBase
{
	public Text Text_LevelUp;

	public Transform LevelUpItem;

	public Transform LevelItem;

	public Text Text_Level;

	public UILineCtrl mLineCtrl;

	public RectTransform rewardparent;

	public TapToCloseCtrl mCloseCtrl;

	public GameObject copyitems;

	public GameObject copyreward;

	private const float ShowScale = 1.5f;

	private const float playTime = 0.3f;

	private LevelUpProxy.Transfer mTransfer;

	private LocalUnityObjctPool mPool;

	private List<GoldTextCtrl> mRewards = new List<GoldTextCtrl>();

	private int adddiamond;

	protected override void OnInit()
	{
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<GoldTextCtrl>(copyreward);
		copyitems.SetActive(value: false);
	}

	protected override void OnOpen()
	{
		GameLogic.SetPause(pause: true);
		mCloseCtrl.Show(value: false);
		LevelUpItem.localScale = Vector3.zero;
		LevelItem.localScale = Vector3.zero;
		mLineCtrl.transform.localScale = Vector3.zero;
		mLineCtrl.SetFontSize(40);
		mLineCtrl.SetText(GameLogic.Hold.Language.GetLanguageByTID("levelup_rewards"));
		mPool.Collect<GoldTextCtrl>();
		mRewards.Clear();
		mCloseCtrl.OnClose = delegate
		{
			WindowUI.CloseWindow(WindowID.WindowID_LevelUp);
		};
		IProxy proxy = Facade.Instance.RetrieveProxy("LevelUpProxy");
		if (proxy == null)
		{
			SdkManager.Bugly_Report("LevelUpProxy", "WindowID_LevelUp LevelUpProxy dont transfer!");
		}
		mTransfer = (LevelUpProxy.Transfer)proxy.Data;
		Text_Level.text = mTransfer.level.ToString();
		Character_Level beanById = LocalModelManager.Instance.Character_Level.GetBeanById(mTransfer.level);
		adddiamond = 0;
		for (int i = 0; i < beanById.Rewards.Length; i++)
		{
			string str = beanById.Rewards[i];
			Drop_DropModel.DropSaveOneData dropOne = Drop_DropModel.GetDropOne(str);
			if (dropOne.type == 1)
			{
				GoldTextCtrl goldTextCtrl = mPool.DeQueue<GoldTextCtrl>();
				goldTextCtrl.SetCurrencyType(dropOne.id);
				goldTextCtrl.SetAdd(dropOne.count);
				RectTransform rectTransform = goldTextCtrl.transform as RectTransform;
				rectTransform.SetParentNormal(rewardparent);
				rectTransform.localScale = Vector3.zero;
				rectTransform.anchoredPosition = new Vector2(0f, mRewards.Count * -70);
				mRewards.Add(goldTextCtrl);
			}
		}
		LocalSave.Instance.Modify_drop(beanById.Rewards);
		InitUI();
	}

	private void InitUI()
	{
		Sequence sequence = DOTween.Sequence();
		sequence.SetUpdate(isIndependentUpdate: true);
		sequence.AppendCallback(delegate
		{
			LevelUpItem.localScale = Vector3.one * 1.5f;
			LevelUpItem.DOScale(Vector3.one, 0.225000009f).SetUpdate(isIndependentUpdate: true);
		});
		sequence.AppendInterval(0.3f);
		sequence.AppendCallback(delegate
		{
			LevelItem.localScale = Vector3.one * 1.5f;
			LevelItem.DOScale(Vector3.one, 0.225000009f).SetUpdate(isIndependentUpdate: true);
		});
		sequence.AppendInterval(0.3f);
		sequence.AppendCallback(delegate
		{
			mLineCtrl.transform.localScale = Vector3.one * 1.5f;
			mLineCtrl.transform.DOScale(Vector3.one, 0.225000009f).SetUpdate(isIndependentUpdate: true);
		});
		sequence.AppendInterval(0.3f);
		int i = 0;
		for (int count = mRewards.Count; i < count; i++)
		{
			int index = i;
			sequence.AppendCallback(delegate
			{
				if (index < mRewards.Count && (bool)mRewards[index])
				{
					mRewards[index].transform.localScale = Vector3.one * 1.5f;
					mRewards[index].transform.DOScale(Vector3.one, 0.225000009f).SetUpdate(isIndependentUpdate: true);
				}
			});
			sequence.AppendInterval(0.3f);
		}
		sequence.AppendCallback(delegate
		{
			mCloseCtrl.Show(value: true);
		});
	}

	protected override void OnClose()
	{
		GameLogic.SetPause(pause: false);
		if (mTransfer != null && mTransfer.onclose != null)
		{
			mTransfer.onclose();
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
		Text_LevelUp.text = GameLogic.Hold.Language.GetLanguageByTID("levelup_title", mTransfer.level);
	}
}
