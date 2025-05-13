




using Dxx.Util;
using PureMVC.Interfaces;
using System.Collections.Generic;
using UnityEngine;

public class GameOverModuleMediator : WindowMediator, IMediator, INotifier
{
	public new const string NAME = "GameOverModuleMediator";

	private static Dictionary<GameMode, MediatorCtrlBase> mModeCtrlList = new Dictionary<GameMode, MediatorCtrlBase>();

	private static MediatorCtrlBase mCurrentModeCtrl;

	public override List<string> OnListNotificationInterests => new List<string>();

	public GameOverModuleMediator()
		: base("GameOverUI")
	{
	}

	protected override void OnRegisterOnce()
	{
	}

	protected override void OnRegisterEvery()
	{
		GameMode mode = GameLogic.Hold.BattleData.GetMode();
		mCurrentModeCtrl = null;
		mModeCtrlList.TryGetValue(mode, out mCurrentModeCtrl);
		if (mCurrentModeCtrl == null)
		{
			string text = string.Empty;
			if (GameLogic.Hold.BattleData.isEnterSourceMain())
			{
				text = "GameOverLevel";
			}
			else if (GameLogic.Hold.BattleData.isEnterSourceMatch())
			{
				text = "GameOverMatchDefenceTime";
			}
			else
			{
				switch (mode)
				{
				case GameMode.eLevel:
				case GameMode.eBomberman:
				case GameMode.eBombDodge:
				case GameMode.eFlyDodge:
					text = "GameOverLevel";
					break;
				case GameMode.eGold1:
					text = "GameOverLevel";
					break;
				case GameMode.eChest1:
					text = "GameOverLevel";
					break;
				case GameMode.eChallenge101:
				case GameMode.eChallenge102:
				case GameMode.eChallenge103:
				case GameMode.eChallenge104:
					text = "GameOverChallenge";
					break;
				case GameMode.eMatchDefenceTime:
					text = "GameOverMatchDefenceTime";
					break;
				default:
					SdkManager.Bugly_Report("GameOverModuleMediator", Utils.FormatString("OnRegisterOnce In {0} the GameNode.{1} is not achieve!", GetType().ToString(), mode));
					break;
				}
			}
			GameObject gameObject = Object.Instantiate(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/GameOverUI/{0}", text)));
			gameObject.SetParentNormal(_MonoView.transform);
			mCurrentModeCtrl = gameObject.GetComponentInChildren<MediatorCtrlBase>();
			mCurrentModeCtrl.Init();
			if (mModeCtrlList.ContainsKey(mode))
			{
				mModeCtrlList[mode] = mCurrentModeCtrl;
			}
			else
			{
				mModeCtrlList.Add(mode, mCurrentModeCtrl);
			}
		}
		GameLogic.SetPause(pause: true);
		mCurrentModeCtrl.Open();
		mCurrentModeCtrl.OnLanguageChange();
		LocalSave.Instance.SaveExtra.overopencount++;
		LocalSave.Instance.SaveExtra.Refresh();
	}

	protected override void OnRemoveAfter()
	{
		GameLogic.Hold.BattleData.ActiveID = 0;
		GameLogic.Hold.BattleData.Challenge_DeInit();
		GameLogic.SetInGame(gaming: false);
		GameLogic.SetPause(pause: false);
		if (mCurrentModeCtrl != null)
		{
			mCurrentModeCtrl.Close();
		}
	}

	public override void OnHandleNotification(INotification notification)
	{
		if (mCurrentModeCtrl != null)
		{
			mCurrentModeCtrl.OnHandleNotification(notification);
		}
	}

	protected override void OnLanguageChange()
	{
		if (mCurrentModeCtrl != null)
		{
			mCurrentModeCtrl.OnLanguageChange();
		}
	}
}
