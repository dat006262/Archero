




using DG.Tweening;
using PureMVC.Interfaces;
using System.Collections.Generic;
using TableTool;
using UnityEngine;
using UnityEngine.UI;

public class ChallengeUICtrl : MediatorCtrlBase
{
	public Text Text_Title;

	public ButtonCtrl Button_Close;

	public ButtonCtrl Button_Shadow;

	public ChallengeInfoCtrl mInfoCtrl;

	public ScrollRectBase mScrollRect;

	public GridLayoutGroup scrollGroup;

	public GameObject copyitems;

	public GameObject copyitem;

	private LocalUnityObjctPool mPool;

	private List<Stage_Level_activity> mList;

	private int mCurrentID;

	protected override void OnInit()
	{
		Button_Close.onClick = delegate
		{
			GameLogic.Hold.BattleData.Challenge_DeInit();
			WindowUI.CloseWindow(WindowID.WindowID_Challenge);
		};
		Button_Shadow.onClick = Button_Close.onClick;
		copyitems.SetActive(value: false);
		mPool = LocalUnityObjctPool.Create(base.gameObject);
		mPool.CreateCache<ChallengeOneCtrl>(copyitem);
	}

	protected override void OnOpen()
	{
		InitUI();
	}

	private void InitUI()
	{
		mPool.Collect<ChallengeOneCtrl>();
		mCurrentID = LocalSave.Instance.Challenge_GetID();
		mInfoCtrl.transform.localScale = Vector3.zero;
		mList = LocalModelManager.Instance.Stage_Level_activity.GetChallengeList();
		int count = mList.Count;
		for (int i = 0; i < count; i++)
		{
			ChallengeOneCtrl challengeOneCtrl = mPool.DeQueue<ChallengeOneCtrl>();
			challengeOneCtrl.transform.SetParentNormal(mScrollRect.content);
			challengeOneCtrl.Init(i, mList[i], count);
		}
		mScrollRect.SetWhole(scrollGroup, count);
		mScrollRect.UseDrag = false;
		if (LocalSave.Instance.Challenge_IsFirstIn())
		{
			int beforeid = mCurrentID - 2101 - 1;
			mScrollRect.SetPage(beforeid, animate: false);
			mInfoCtrl.Init(mCurrentID - 1);
			PlayInfo(show: true);
			WindowUI.ShowMask(value: true);
			DOTween.Sequence().AppendInterval(0.5f).AppendCallback(delegate
			{
				LocalSave.Instance.Challenge_SetFirstIn();
				PlayInfo(show: false);
				mScrollRect.SetPage(beforeid + 1, animate: true, MoveToNext);
				WindowUI.ShowMask(value: false);
			});
		}
		else
		{
			mScrollRect.SetPage(mCurrentID - 2101, animate: false);
			mInfoCtrl.Init(mCurrentID);
			PlayInfo(show: true);
		}
	}

	private void PlayInfo(bool show)
	{
		if (show)
		{
			mInfoCtrl.transform.localScale = Vector3.zero;
			DOTween.Sequence().Append(mInfoCtrl.transform.DOScale(1f, 0.25f).SetEase(Ease.OutBack));
		}
		else
		{
			mInfoCtrl.transform.localScale = Vector3.one;
			DOTween.Sequence().Append(mInfoCtrl.transform.DOScale(0f, 0.25f).SetEase(Ease.InBack));
		}
	}

	private void MoveToNext()
	{
		PlayInfo(show: true);
		mInfoCtrl.Init(mCurrentID);
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
