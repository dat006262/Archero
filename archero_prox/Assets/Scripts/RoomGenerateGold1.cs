




using Dxx.Util;
using System;
using TableTool;

public class RoomGenerateGold1 : RoomGenerateBase
{
	private WaveData mWave;

	private Stage_Level_activitylevel activitydata;

	protected override void OnInit()
	{
	}

	protected override void OnStartGame()
	{
		maxRoomID = LocalModelManager.Instance.Stage_Level_activitylevel.GetMaxLayer();
	}

	protected override void OnStartGameEnd()
	{
	}

	protected override void OnEnterDoorBefore()
	{
	}

	protected override void OnEnterDoorAfter()
	{
		roomCtrl.OpenDoor(value: false);
		StartCurrentRoomWave();
	}

	private void StartCurrentRoomWave()
	{
		activitydata = GameLogic.Hold.BattleData.GetActiveLevelData(base.currentRoomID);
		mWave = new WaveData(0f, activitydata.Args);
		mWave.OnCreateWave = OnCreateWave;
		mWave.Start();
	}

	protected override string OnGetTmxID(int roomid)
	{
		if (roomid == 0)
		{
			return "emptyroom";
		}
		Stage_Level_activitylevel activeLevelData = GameLogic.Hold.BattleData.GetActiveLevelData(roomid);
		int num = GameLogic.Random(0, activeLevelData.RoomIDs.Length);
		return activeLevelData.RoomIDs[num];
	}

	public override bool CanOpenDoor()
	{
		if (mWave != null && mWave.IsEnd && base.CanOpenDoor())
		{
			return true;
		}
		return false;
	}

	protected override void OnOpenDoor()
	{
	}

	protected override void OnDeInit()
	{
		if (mWave != null)
		{
			mWave.Stop();
		}
	}

	public void OnCreateWave()
	{
		string tmxid = Utils.FormatString("{0:D2}", base.currentRoomID);
		GameLogic.Release.MapCreatorCtrl.CreateMap(new MapCreator.Transfer
		{
			roomctrl = roomCtrl,
			roomid = base.currentRoomID,
			resourcesid = roomList[base.currentRoomID].ResourcesID,
			tmxid = tmxid,
			delay = true
		});
	}

	private void ClearCurrentRoom()
	{
		roomCtrl.Clear();
	}

	protected override void OnEventClose(EventCloseTransfer data)
	{
	}

	protected override void OnReceiveEvent(string eventName, object data)
	{
		if (eventName != null && eventName == "Mode_Adventure_CreateNextWave")
		{
			return;
		}
		throw new Exception(Utils.FormatString("{0}.OnReceiveEvent Receive [{1}] is not expected!", GetType().ToString(), eventName));
	}
}
