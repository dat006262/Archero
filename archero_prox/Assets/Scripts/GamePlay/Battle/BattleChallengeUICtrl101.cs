




using PureMVC.Interfaces;
using UnityEngine;

public class BattleChallengeUICtrl101 : BattleLevelUICtrl
{
	public Transform parent;

	private ChallengeHideCtrl mHideCtrl;

	protected override void OnInit()
	{
		base.OnInit();
	}

	protected override void OnOpen()
	{
		if (mExpCtrl != null)
		{
			mExpCtrl.SetDropExp(GameLogic.Hold.BattleData.Challenge_DropExp());
		}
		base.OnOpen();
		GameLogic.Hold.BattleData.Challenge_SetUIParent(parent);
		if (GameLogic.Hold.BattleData.Challenge_MonsterHide())
		{
			if (mHideCtrl == null)
			{
				GameObject gameObject = Object.Instantiate(ResourceManager.Load<GameObject>("UIPanel/BattleUI/ChallengeHide"));
				mHideCtrl = gameObject.GetComponent<ChallengeHideCtrl>();
			}
			mHideCtrl.transform.SetParentNormal(GameNode.m_Front);
			mHideCtrl.gameObject.SetActive(value: true);
			mHideCtrl.Init();
		}
	}

	protected override void OnClose()
	{
		base.OnClose();
		if (mHideCtrl != null)
		{
			mHideCtrl.DeInit();
			mHideCtrl.gameObject.SetActive(value: false);
		}
	}

	public override object OnGetEvent(string eventName)
	{
		return base.OnGetEvent(eventName);
	}

	public override void OnHandleNotification(INotification notification)
	{
		base.OnHandleNotification(notification);
	}

	public override void OnLanguageChange()
	{
		base.OnLanguageChange();
	}
}
