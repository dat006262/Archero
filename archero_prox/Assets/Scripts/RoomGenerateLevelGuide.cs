




using Dxx.Util;

public class RoomGenerateLevelGuide : RoomGenerateBase
{
	protected override void OnInit()
	{
	}

	protected override void OnStartGame()
	{
		maxRoomID = 2;
	}

	protected override void OnStartGameEnd()
	{
	}

	protected override void OnEnterDoorBefore()
	{
	}

	protected override void OnEnterDoorAfter()
	{
		if (base.currentRoomID < maxRoomID)
		{
			roomCtrl.SetText(string.Empty);
			roomCtrl.LayerShow(value: false);
		}
		else
		{
			roomCtrl.LayerShow(value: true);
			roomCtrl.SetText("1");
		}
	}

	protected override string OnGetTmxID(int roomid)
	{
		return Utils.FormatString("Level_M_0_990{0}", roomid);
	}

	protected override void OnOpenDoor()
	{
	}

	protected override void OnEnd()
	{
		WindowUI.ShowLoading(delegate
		{
			mGuidEndAction();
		}, delegate
		{
		}, delegate
		{
		});
	}
}
