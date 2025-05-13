




using Dxx.Util;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ReleaseModeManager
{
	private GameMode mMode;

	private DeadGoodMgr mDeadGoodMgr = new DeadGoodMgr();

	private Transform goodsParent;

	private GameObject MoveJoy;

	private float mStartTime;

	private RoomGenerateBase _RoomGenerate;

	public Action<RoomGenerateBase.Room> OnGotoNextRoom;

#if ENABLE_COMPILER_GENERATED
	[CompilerGenerated]
#endif
	private static Action<RoomGenerateBase.Room> _003C_003Ef__mg_0024cache0;

	public RoomGenerateBase RoomGenerate => _RoomGenerate;

	public void Init()
	{
		GameLogic.SetInGame(gaming: true);
		Updater.GetUpdater().Init();
		GameLogic.Hold.BattleDataReset();
		GameLogic.SelfAttribute.ClearBattle();
		GameLogic.Release.Form.InitData();
		OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Combine(OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoomEvent));
		OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Combine(OnGotoNextRoom, new Action<RoomGenerateBase.Room>(SkillAloneAttrGoodBase.OnGotoNextRoom));
		SkillAloneAttrGoodBase.InitData();
		mStartTime = Time.unscaledTime;
		mMode = GameLogic.Hold.BattleData.GetMode();
		mDeadGoodMgr.Init();
		CreatePlayer();
		LocalSave.Instance.BattleIn_Restore();
		CreateJoy();
		SwitchMode();
		startdrop();
	}

	public void DeInit()
	{
		SkillAloneAttrGoodBase.DeInitData();
		OnGotoNextRoom = (Action<RoomGenerateBase.Room>)Delegate.Remove(OnGotoNextRoom, new Action<RoomGenerateBase.Room>(OnGotoNextRoomEvent));
		mDeadGoodMgr.DeInit();
		if (_RoomGenerate != null)
		{
			_RoomGenerate.DeInit();
			_RoomGenerate = null;
		}
	}

	private void OnGotoNextRoomEvent(RoomGenerateBase.Room room)
	{
		GameLogic.Release.MapEffect.MapCache();
		mDeadGoodMgr.DeInit();
	}

	public int GetCurrentRoomID()
	{
		if (RoomGenerate != null)
		{
			return RoomGenerate.GetCurrentRoomID();
		}
		return 0;
	}

	public GameMode GetMode()
	{
		return mMode;
	}

	private void SwitchMode()
	{
        Debug.Log("@LOG SwitchMode");
        if (GameLogic.Hold.Guide.GetNeedGuide())
		{
			_RoomGenerate = new RoomGenerateLevelGuide();
		}
		else
		{
			SwitchModeNotGuide();
		}
		RoomGenerate.Init();
		RoomGenerate.StartGame();
		RoomGenerate.SetGuideEndAction(GuideEndAction);
	}

	private void SwitchModeNotGuide()
	{
		switch (mMode)
		{
		case GameMode.eChallenge101:
			_RoomGenerate = new RoomGenerateChallenge101();
			break;
		case GameMode.eChallenge102:
			_RoomGenerate = new RoomGenerateChallenge102();
			break;
		case GameMode.eChallenge103:
			_RoomGenerate = new RoomGenerateChallenge103();
			break;
		case GameMode.eChallenge104:
			_RoomGenerate = new RoomGenerateChallenge104();
			break;
		case GameMode.eLevel:
			_RoomGenerate = new RoomGenerateLevel();
			break;
		case GameMode.eChest1:
			_RoomGenerate = new RoomGenerateChest1();
			break;
		case GameMode.eGold1:
			_RoomGenerate = new RoomGenerateGold1();
			break;
		case GameMode.eBomberman:
			_RoomGenerate = new RoomGenerateChallenge101();
			break;
		case GameMode.eBombDodge:
			_RoomGenerate = new RoomGenerateBombDodge();
			GameLogic.Self.SetCollidersScale(0.7f);
			break;
		case GameMode.eFlyDodge:
			_RoomGenerate = new RoomGenerateFlyDodge();
			break;
		case GameMode.eMatchDefenceTime:
			_RoomGenerate = new RoomGenerateMatchDefenceTime();
			break;
		default:
			SdkManager.Bugly_Report("ReleaseModeManager.cs", Utils.FormatString("SwitchModeNotGuide In {0} the GameNode.{1} is not achieve!", GetType().ToString(), mMode));
			break;
		}
	}

	private void GuideEndAction()
	{
		RoomGenerate.DeInit();
		SwitchModeNotGuide();
		RoomGenerate.Init();
		RoomGenerate.StartGame();
		RoomGenerate.EnterDoor();
	}

	private void CreatePlayer()
	{
        Debug.Log("@LOG CreatePlayer");
		GameObject gameObject = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("Game/Player/PlayerNode"));
        gameObject.transform.parent = GameNode.m_Battle.transform;
		int id = 1001;
		GameLogic.SelfAttribute.Init();
        EntityHero component = gameObject.GetComponent<EntityHero>();
		component.Init(id);
        component.transform.position = new Vector3(0f, 1000f, 0f);
		GameLogic.SelfAttribute.InitBabies();
        CameraControlM.Instance.ResetCameraPosition();
    }

    private void CreateJoy()
	{
        Debug.Log("@LOG CreateJoy");
        MoveJoy = UnityEngine.Object.Instantiate(ResourceManager.Load<GameObject>("Game/UI/MoveJoy"));
		MoveJoy.transform.SetParent(GameNode.m_Joy);
		MoveJoy.transform.localPosition = Vector3.zero;
		MoveJoy.transform.localScale = Vector3.one;
		MoveJoy.SetActive(value: false);
	}

	private void startdrop()
	{
        Debug.Log("@LOG startdrop");
        HeroDropCtrl heroDropCtrl = new HeroDropCtrl();
		heroDropCtrl.Init();
		heroDropCtrl.StartDrop();
	}

	public GameObject GetMoveJoy()
	{
		return MoveJoy;
	}

	public void SetGoodsParent(Transform parent)
	{
		goodsParent = parent;
	}

	public void EnterDoor()
	{
		RoomGenerate.EnterDoor();
	}

	public void CreateGoods(Vector3 pos, List<BattleDropData> goodslist, int radius)
	{
		GameLogic.Hold.Sound.PlayBattleSpecial(5000009, pos);
		mDeadGoodMgr.StartDrop(pos, goodslist, radius, goodsParent);
	}

	public void PlayerDead()
	{
		RoomGenerate.PlayerDead();
	}
}
