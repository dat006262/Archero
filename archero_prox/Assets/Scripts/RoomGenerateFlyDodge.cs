




public class RoomGenerateFlyDodge : RoomGenerateChallenge101
{
	protected override void OnStartGameEnd()
	{
		base.OnStartGameEnd();
		roomCtrl.OpenDoor(value: false);
	}
}
