




public class BuffAlone1002 : BuffAloneBase
{
	protected override void OnStart()
	{
	}

	protected override void OnRemove()
	{
	}

	protected override void OnResetBuffTime()
	{
		GameLogic.Hold.Sound.PlayBattleSpecial(5000012, m_Entity.position);
	}
}
