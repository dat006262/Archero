




using Dxx.Util;
using System;
using TableTool;

public class RoomGenerateLevel : RoomGenerateBase
{
	private int opendoorIndex;

	private bool bShowMysticShop;

	private int bossdeadecentid = -1;

	protected override void OnInit()
	{
	}

	protected override void OnStartGame()
	{
		opendoorIndex = -1;
		maxRoomID = LocalModelManager.Instance.Stage_Level_stagechapter.GetCurrentMaxLevel(GameLogic.Hold.BattleData.Level_CurrentStage);
		if (LocalModelManager.Instance.Stage_Level_stagechapter.is_wave_room())
		{
			GameLogic.Release.MapCreatorCtrl.waveroom_battlecache_init();
		}
	}

	protected override void OnStartGameEnd()
	{
		room_update();
		if (GameLogic.Hold.BattleData.GetMode() == GameMode.eLevel && base.currentRoomID == 0 && GameLogic.Hold.BattleData.Level_CurrentStage > 1 && !LocalSave.Instance.BattleIn_GetGoldTurn())
		{
			GameLogic.Release.MapCreatorCtrl.CreateGoodExtra(9013, 5, 1);
		}
	}

	private void room_update()
	{
		if (base.currentRoomID == 0)
		{
			roomCtrl.SetText(string.Empty);
		}
		else
		{
			roomCtrl.SetText(base.currentRoomID.ToString());
		}
	}

	protected override void OnEnterDoorBefore()
	{
		if (LocalModelManager.Instance.Stage_Level_stagechapter.is_wave_room())
		{
			GameLogic.Release.MapCreatorCtrl.waveroom_killseq();
		}
	}

	protected override void OnEnterDoorAfter()
	{
		room_update();
	}

	protected override string OnGetTmxID(int roomid)
	{
		int level_CurrentStage = GameLogic.Hold.BattleData.Level_CurrentStage;
		string[] tmxIds = GameLogic.Hold.BattleData.mModeData.GetTmxIds(roomid, LocalSave.Instance.SaveExtra.GetLayerCount(level_CurrentStage, roomid));
		return RandomTmx(tmxIds);
	}

	protected override bool gotonextdoor_canopen()
	{
		if (LocalModelManager.Instance.Stage_Level_stagechapter.is_wave_room())
		{
			if (base.currentRoomID == 0)
			{
				return true;
			}
			if (GameLogic.Release.MapCreatorCtrl.waveroom_is_clear())
			{
				return true;
			}
			return false;
		}
		return base.gotonextdoor_canopen();
	}

	public override bool CanOpenDoor()
	{
		if (LocalModelManager.Instance.Stage_Level_stagechapter.is_wave_room())
		{
			if (base.CanOpenDoor())
			{
				GameLogic.Release.MapCreatorCtrl.waveroom_currentwave_clear();
			}
			if (GameLogic.Release.MapCreatorCtrl.waveroom_is_clear())
			{
				return base.CanOpenDoor();
			}
			return false;
		}
		return base.CanOpenDoor();
	}

	protected override void OnOpenDoor()
	{
		if (bossdeadecentid == base.currentRoomID)
		{
			return;
		}
		bossdeadecentid = base.currentRoomID;
		if (base.currentRoomID % 5 == 0 || base.currentRoomID <= 0)
		{
			return;
		}
		if (!bShowMysticShop)
		{
			if (LocalModelManager.Instance.Shop_MysticShop.RandomShop(GameLogic.Hold.BattleData.Level_CurrentStage, base.currentRoomID, roomList[base.currentRoomID].RoomType))
			{
				bShowMysticShop = true;
				GameLogic.Release.MapCreatorCtrl.CreateOneGoods(9004, 5, 1);
			}
		}
		else
		{
			LocalModelManager.Instance.Shop_MysticShop.AddRatio(GameLogic.Hold.BattleData.Level_CurrentStage);
		}
	}

	protected override void OnEventClose(EventCloseTransfer data)
	{
	}

	protected override void OnDeInit()
	{
		RoomGenerateBase.CacheMap(currentMap);
	}

	private bool IsBossRoom(int roomid)
	{
		return LocalModelManager.Instance.Stage_Level_stagechapter.IsMaxLevel(GameLogic.Hold.BattleData.Level_CurrentStage, roomid);
	}

	public override bool IsLastRoom()
	{
		return LocalModelManager.Instance.Stage_Level_stagechapter.IsMaxLevel(GameLogic.Hold.BattleData.Level_CurrentStage, base.currentRoomID);
	}

	protected override void OnMonsterDead(EntityBase entity)
	{
	}

	private void ShowEvent()
	{
		if (!GameLogic.Hold.Guide.GetNeedGuide() && GameLogic.Release.Entity.GetActiveEntityCount() == 0)
		{
			int goodsID = 9009;
			if (IsBossRoom(base.currentRoomID + 1))
			{
				goodsID = 9003;
			}
			GameLogic.Release.MapCreatorCtrl.CreateOneGoods(goodsID, 5, 3);
		}
	}

	public override void PlayerDead()
	{
	}

	protected override void OnEnd()
	{
		base.OnEnd();
	}

	protected override void OnReceiveEvent(string eventName, object data)
	{
		if (eventName != null)
		{
		}
		throw new Exception(Utils.FormatString("{0}.OnReceiveEvent Receive [{1}] is not expected!", GetType().ToString(), eventName));
	}
}
