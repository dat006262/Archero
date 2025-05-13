




using DG.Tweening;
using Dxx.Util;
using System;
using TableTool;

public class RoomGenerateMatchDefenceTime : RoomGenerateBase
{
	private Sequence seq;

	private Stage_Level_activitylevel activitydata;

	protected override void OnInit()
	{
	}

	protected override void OnStartGame()
	{
		base.currentRoomID = 0;
		maxRoomID = LocalModelManager.Instance.Stage_Level_activitylevel.GetMaxLayer();
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
		activitydata = GameLogic.Hold.BattleData.GetActiveLevelData(base.currentRoomID);
		seq = DOTween.Sequence();
		seq.AppendInterval(float.Parse(activitydata.Args[0]));
		seq.AppendCallback(CreateWave);
	}

	private void CreateWave()
	{
		ClearCurrentRoom();
		base.currentRoomID++;
		Room room = new Room();
		room.SetRoomID(base.currentRoomID);
		string[] tmxids = null;
		int num = (int)GameLogic.Hold.BattleData.Challenge_GetEvent("MatchDefenceTime_get_random_roomid_row");
		switch (num)
		{
		case 0:
			tmxids = GameLogic.Hold.BattleData.GetActiveLevelData(base.currentRoomID).RoomIDs;
			break;
		case 1:
			tmxids = GameLogic.Hold.BattleData.GetActiveLevelData(base.currentRoomID).RoomIDs1;
			break;
		case 2:
			tmxids = GameLogic.Hold.BattleData.GetActiveLevelData(base.currentRoomID).RoomIDs2;
			break;
		default:
			SdkManager.Bugly_Report("RoomGenerateMatchDefenceTime.CreateWave", Utils.FormatString("(NotifyConst.MatchDefenceTime.get_random_roomid_row[{0}] is invalid.", num));
			break;
		}
		XRandom random = (XRandom)GameLogic.Hold.BattleData.Challenge_GetEvent("MatchDefenceTime_get_xrandom");
		string tmx = RandomTmx(tmxids, random);
		room.SetTmx(tmx);
		GameLogic.Release.MapCreatorCtrl.CreateMap(new MapCreator.Transfer
		{
			roomctrl = roomCtrl,
			roomid = base.currentRoomID,
			resourcesid = room.ResourcesID,
			tmxid = room.TMXID,
			delay = true
		});
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
		if (base.CanOpenDoor())
		{
			CreateWave();
		}
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

	private void ClearCurrentRoom()
	{
		roomCtrl.ClearGoods();
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
