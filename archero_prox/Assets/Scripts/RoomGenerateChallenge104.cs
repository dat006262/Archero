




using Dxx.Util;
using System;

public class RoomGenerateChallenge104 : RoomGenerateBase
{
	protected override void OnInit()
	{
	}

	protected override void OnStartGame()
	{
	}

	protected override void OnStartGameEnd()
	{
	}

	protected override void OnEnterDoorBefore()
	{
	}

	protected override void OnEnterDoorAfter()
	{
	}

	protected override string OnGetTmxID(int roomid)
	{
		string[] tmxIds = GameLogic.Hold.BattleData.mModeData.GetTmxIds(roomid, 0);
		return RandomTmx(tmxIds);
	}

	protected override void OnOpenDoor()
	{
	}

	protected override void OnDeInit()
	{
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
		if (eventName != null)
		{
		}
		throw new Exception(Utils.FormatString("{0}.OnReceiveEvent Receive [{1}] is not expected!", GetType().ToString(), eventName));
	}
}
