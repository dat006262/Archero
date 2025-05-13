




using Dxx.Util;

public class RoomGenerateBombDodge : RoomGenerateChallenge101
{
	protected override string OnGetFirstRoomTMX()
	{
		int num = GameLogic.Random(1, 3);
		return Utils.FormatString("bombdodge{0:D2}", num);
	}

	protected override void OnStartGameEnd()
	{
		base.OnStartGameEnd();
		roomCtrl.OpenDoor(value: false);
	}
}
