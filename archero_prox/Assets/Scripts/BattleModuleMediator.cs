




using Dxx.Util;
using PureMVC.Interfaces;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BattleModuleMediator : WindowMediator, IMediator, INotifier
{
	public const string Event_GetGoldPosition = "Event_GetGoldPosition";

	public new const string NAME = "BattleModuleMediator";

	private static Dictionary<GameMode, MediatorCtrlBase> mModeCtrlList = new Dictionary<GameMode, MediatorCtrlBase>();

	private static MediatorCtrlBase mCurrentModeCtrl;

	public override List<string> OnListNotificationInterests
	{
		get
		{
			List<string> list = new List<string>();
			list.Add("BATTLE_GAMEOVER");
			list.Add("BATTLE_UI_BOSSHP_UPDATE");
			list.Add("BATTLE_LEVEL_UP");
			list.Add("BATTLE_EXP_UP");
			list.Add("BATTLE_CHOOSESKILL_TO_BATTLE_CLOSE");
			list.Add("BATTLE_ROOM_TYPE");
			list.Add("PUB_UI_UPDATE_CURRENCY");
			list.Add("Mode_Greedy_CurrentWaveOver");
			list.Add("Mode_Greedy_UpdateCurrentWave");
			list.Add("CurrentWaveOver");
			list.Add("UpdateCurrentWave");
			list.Add("Mode_Adventure_CurrentWaveOver");
			list.Add("Mode_Adventure_UpdateCurrentWave");
			list.Add("Currency_BattleKey");
			list.Add("UpdateWave");
			list.Add("BATTLE_GET_GOLD");
			list.Add("MatchDefenceTime_me_dead");
			list.Add("BattleUI_level_wave_update");
			return list;
		}
	}

	public BattleModuleMediator()
		: base("BattleUI")
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
			string text;
			switch (mode)
			{
			case GameMode.eLevel:
				text = "BattleLevelPanel";
				break;
			case GameMode.eChest1:
				text = "BattleLevelPanel";
				break;
			case GameMode.eGold1:
				text = "BattleLevelPanel";
				break;
			case GameMode.eChallenge101:
			case GameMode.eChallenge102:
			case GameMode.eChallenge103:
			case GameMode.eChallenge104:
				text = "BattleLevelPanel";
				break;
			case GameMode.eBomberman:
				text = "BattleLevelPanel";
				break;
			case GameMode.eBombDodge:
				text = "BattleChallengePanel101";
				break;
			case GameMode.eFlyDodge:
				text = "BattleLevelPanel";
				break;
			case GameMode.eMatchDefenceTime:
				text = "BattleMatchDefenceTime";
				break;
			default:
				throw new Exception(Utils.FormatString("In {0} the GameNode.{1} is not achieve!", GetType().ToString(), mode));
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>(Utils.FormatString("UIPanel/BattleUI/{0}", text)));
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
		else
		{
			mCurrentModeCtrl.gameObject.SetActive(value: true);
			mCurrentModeCtrl.transform.SetAsLastSibling();
		}
		GameLogic.Release.Mode.Init();
		CameraControlM.Instance.ResetCameraSize();
		mCurrentModeCtrl.Open();
		mCurrentModeCtrl.OnLanguageChange();
		GameLogic.Hold.BattleData.Challenge_Start();
	}

	protected override void OnRemoveAfter()
	{
		if (mCurrentModeCtrl != null)
		{
			mCurrentModeCtrl.Close();
			mCurrentModeCtrl.gameObject.SetActive(value: false);
		}
	}

	public override object GetEvent(string eventName)
	{
		if (mCurrentModeCtrl != null)
		{
			return mCurrentModeCtrl.OnGetEvent(eventName);
		}
		return null;
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
