




using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;

public class RoomGenerateChallenge102 : RoomGenerateBase
{
	private Sequence seq;

	private Stage_Level_activitylevel activitydata;

	private int waveid;

	protected override void OnInit()
	{
	}

	protected override void OnStartGame()
	{
		waveid = 0;
		StartWave();
	}

	protected override void OnStartGameEnd()
	{
		roomCtrl.OpenDoor(value: false);
	}

	protected override void OnEnterDoorBefore()
	{
	}

	protected override void OnEnterDoorAfter()
	{
		roomCtrl.OpenDoor(value: false);
	}

	private void StartWave()
	{
		activitydata = GameLogic.Hold.BattleData.GetActiveLevelData(waveid);
		seq = DOTween.Sequence();
		seq.AppendInterval(float.Parse(activitydata.Args[0]));
		seq.AppendCallback(CreateWave);
	}

	private void CreateWave()
	{
		waveid++;
		Room room = new Room();
		room.SetRoomID(waveid);
		string[] roomIDs = GameLogic.Hold.BattleData.GetActiveLevelData(waveid).RoomIDs;
		string tmx = RandomTmx(roomIDs);
		room.SetTmx(tmx);
		GameLogic.Release.MapCreatorCtrl.CreateMap(new MapCreator.Transfer
		{
			roomctrl = roomCtrl,
			roomid = base.currentRoomID,
			resourcesid = room.ResourcesID,
			tmxid = room.TMXID,
			delay = true
		});
		StartWave();
	}

	protected override string OnGetTmxID(int roomid)
	{
		string[] tmxIds = GameLogic.Hold.BattleData.mModeData.GetTmxIds(roomid, 0);
		return RandomTmx(tmxIds);
	}

	public override bool CanOpenDoor()
	{
		return false;
	}

	protected override void OnOpenDoor()
	{
	}

	protected override void OnDeInit()
	{
		if (seq != null)
		{
			seq.Kill();
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
