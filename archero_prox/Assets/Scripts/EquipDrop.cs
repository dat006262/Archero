




public class EquipDrop : GoodsDrop
{
	protected override string JumpAnimation => "EquipJump1";

	protected override void OnInit()
	{
		Drop_jumpTime = 1f;
	}
}
