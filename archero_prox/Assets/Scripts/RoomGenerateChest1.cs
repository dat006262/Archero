




using Dxx.Util;
using PureMVC.Patterns;
using System;
using TableTool;

public class RoomGenerateChest1 : RoomGenerateBase
{
	public const string Event_TurnTable_Monster = "Event_TurnTable_Monster";

	public const string Event_TurnTable_Boss = "Event_TurnTable_Boss";

	public const string GetEvent_TurnTable_DropID = "GetEvent_TurnTable_DropID";

	private WaveData mWave;

	private Stage_Level_activitylevel activitydata;

	protected override void OnInit()
	{
	}

	protected override void OnStartGame()
	{
		UpdateActivityData();
		maxRoomID = LocalModelManager.Instance.Stage_Level_activitylevel.GetMaxLayer();
	}

	protected override void OnEnterDoorBefore()
	{
	}

	protected override void OnEnterDoorAfter()
	{
		roomCtrl.OpenDoor(value: false);
		UpdateActivityData();
		Facade.Instance.RegisterProxy(new EventChest1Proxy(activitydata.Args));
	}

	private void UpdateActivityData()
	{
		activitydata = GameLogic.Hold.BattleData.ActiveLevelData;
	}

	private void StartWave()
	{
		mWave = new WaveData(0.5f, activitydata.Args);
		mWave.OnCreateWave = OnCreateWave;
		mWave.Start();
	}

	protected override string OnGetTmxID(int roomid)
	{
		return "0_0001";
	}

	public override bool CanOpenDoor()
	{
		return base.CanOpenDoor();
	}

	protected override void OnOpenDoor()
	{
		ShowBossDeadEvent();
	}

	protected override void OnEventClose(EventCloseTransfer data)
	{
		if (data.windowid == WindowID.WindowID_EventChect1)
		{
			TurnTableType turnTableType = (TurnTableType)data.data;
			if (turnTableType != TurnTableType.Boss)
			{
				roomCtrl.OpenDoor(value: true);
			}
		}
	}

	protected override void OnDeInit()
	{
		if (mWave != null)
		{
			mWave.Stop();
		}
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

	private void ShowBossDeadEvent()
	{
	}

	private void ShowEvent()
	{
	}

	protected override void OnPlayerHitted(long changehp)
	{
	}

	public override void PlayerDead()
	{
	}

	protected override void OnReceiveEvent(string eventName, object data)
	{
		if (eventName != null && (eventName == "Event_TurnTable_Monster" || eventName == "Event_TurnTable_Boss"))
		{
			StartWave();
			return;
		}
		throw new Exception(Utils.FormatString("{0}.OnReceiveEvent Receive [{1}] is not expected!", GetType().ToString(), eventName));
	}

	public void OnCreateWave()
	{
		string tmxid = RandomTmx(activitydata.RoomIDs);
		GameLogic.Release.MapCreatorCtrl.CreateMap(new MapCreator.Transfer
		{
			roomctrl = roomCtrl,
			roomid = base.currentRoomID,
			resourcesid = roomList[base.currentRoomID].ResourcesID,
			tmxid = tmxid,
			delay = true,
			roomtype = RoomType.eBoss
		});
	}

	protected override object OnGetEvent(string eventName, object data = null)
	{
		if (eventName != null && eventName == "GetEvent_TurnTable_DropID")
		{
			return int.Parse(activitydata.Args[2]);
		}
		return null;
	}
}
